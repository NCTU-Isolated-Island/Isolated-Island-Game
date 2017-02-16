using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataParameters;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.LandmarkRoomOperationHandlers
{
    public class LandmarkRoomFetchDataResolver : LandmarkRoomOperationHandler
    {
        internal readonly Dictionary<LandmarkRoomFetchDataCode, LandmarkRoomFetchDataHandler> fetchTable;

        public LandmarkRoomFetchDataResolver(LandmarkRoom subject) : base(subject, 2)
        {
            fetchTable = new Dictionary<LandmarkRoomFetchDataCode, LandmarkRoomFetchDataHandler>
            {
                { LandmarkRoomFetchDataCode.AllMultiplayerSynthesizeParticipantInfos, new FetchAllMultiplayerSynthesizeParticipantInfosHandler(subject) }
            };
        }

        internal override bool Handle(CommunicationInterface communicationInterface, LandmarkRoomOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(communicationInterface, operationCode, parameters))
            {
                string debugMessage;
                LandmarkRoomFetchDataCode fetchCode = (LandmarkRoomFetchDataCode)parameters[(byte)FetchDataParameterCode.FetchDataCode];
                Dictionary<byte, object> resolvedParameters = (Dictionary<byte, object>)parameters[(byte)FetchDataParameterCode.Parameters];
                if (fetchTable.ContainsKey(fetchCode))
                {
                    return fetchTable[fetchCode].Handle(communicationInterface, fetchCode, resolvedParameters);
                }
                else
                {
                    debugMessage = string.Format("LandmarkRoom Fetch Operation Not Exist Fetch Code: {0}", fetchCode);
                    SendError(communicationInterface, operationCode, ErrorCode.InvalidOperation, debugMessage);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        internal void SendOperation(LandmarkRoomFetchDataCode fetchCode, Dictionary<byte, object> parameters)
        {
            subject.OperationManager.SendFetchDataOperation(fetchCode, parameters);
        }

        public void AllMultiplayerSynthesizeParticipantInfos()
        {
            SendOperation(LandmarkRoomFetchDataCode.AllMultiplayerSynthesizeParticipantInfos, new Dictionary<byte, object>());
        }
    }
}
