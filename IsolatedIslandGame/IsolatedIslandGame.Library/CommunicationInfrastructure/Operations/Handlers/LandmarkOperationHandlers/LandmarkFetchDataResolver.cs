using IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.LandmarkOperationHandlers.FetchDataHandlers;
using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataParameters;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.LandmarkOperationHandlers
{
    public class LandmarkFetchDataResolver : LandmarkOperationHandler
    {
        internal readonly Dictionary<LandmarkFetchDataCode, LandmarkFetchDataHandler> fetchTable;

        public LandmarkFetchDataResolver(Landmark subject) : base(subject, 2)
        {
            fetchTable = new Dictionary<LandmarkFetchDataCode, LandmarkFetchDataHandler>
            {
                { LandmarkFetchDataCode.AllLandmarkRooms, new FetchAllLandmarkRoomsHandler(subject) }
            };
        }

        internal override bool Handle(CommunicationInterface communicationInterface, LandmarkOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(communicationInterface, operationCode, parameters))
            {
                string debugMessage;
                LandmarkFetchDataCode fetchCode = (LandmarkFetchDataCode)parameters[(byte)FetchDataParameterCode.FetchDataCode];
                Dictionary<byte, object> resolvedParameters = (Dictionary<byte, object>)parameters[(byte)FetchDataParameterCode.Parameters];
                if (fetchTable.ContainsKey(fetchCode))
                {
                    return fetchTable[fetchCode].Handle(communicationInterface, fetchCode, resolvedParameters);
                }
                else
                {
                    debugMessage = string.Format("Landmark Fetch Operation Not Exist Fetch Code: {0}", fetchCode);
                    SendError(communicationInterface, operationCode, ErrorCode.InvalidOperation, debugMessage);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        internal void SendOperation(LandmarkFetchDataCode fetchCode, Dictionary<byte, object> parameters)
        {
            subject.OperationManager.SendFetchDataOperation(fetchCode, parameters);
        }

        public void FetchAllLandmarkRooms()
        {
            SendOperation(LandmarkFetchDataCode.AllLandmarkRooms, new Dictionary<byte, object>());
        }
    }
}
