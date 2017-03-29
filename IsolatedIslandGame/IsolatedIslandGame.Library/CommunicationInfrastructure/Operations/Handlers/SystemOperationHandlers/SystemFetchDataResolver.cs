using IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.SystemOperationHandlers.FetchDataHandlers;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataParameters;
using IsolatedIslandGame.Protocol.Communication.FetchDataParameters.System;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.SystemOperationHandlers
{
    public class SystemFetchDataResolver : SystemOperationHandler
    {
        internal readonly Dictionary<SystemFetchDataCode, SystemFetchDataHandler> fetchTable;

        public SystemFetchDataResolver(SystemManager subject) : base(subject, 2)
        {
            fetchTable = new Dictionary<SystemFetchDataCode, SystemFetchDataHandler>
            {
                { SystemFetchDataCode.Item, new FetchItemHandler(subject) },
                { SystemFetchDataCode.AllItems, new FetchAllItemsHandler(subject) },
                { SystemFetchDataCode.AllVessels, new FetchAllVesselsHandler(subject) },
                { SystemFetchDataCode.Vessel, new FetchVesselHandler(subject) },
                { SystemFetchDataCode.VesselWithOwnerPlayerID, new FetchVesselWithOwnerPlayerIDHandler(subject) },
                { SystemFetchDataCode.VesselDecorations, new FetchVesselDecorationsHandler(subject) },
                { SystemFetchDataCode.IslandTotalScore, new FetchIslandTotalScoreHandler(subject) },
                { SystemFetchDataCode.IslandTodayMaterialRanking, new FetchIslandTodayMaterialRankingHandler(subject) },
                { SystemFetchDataCode.IslandPlayerScoreRanking, new FetchIslandPlayerScoreRankingHandler(subject) },
                { SystemFetchDataCode.AllLandmarks, new FetchAllLandmarksHandler(subject) },
                { SystemFetchDataCode.ClientFunctionCheckTable, new FetchClientFunctionCheckTableHandler(subject) },
                { SystemFetchDataCode.WorldChannelMessages, new FetchWorldChannelMessagesHandler(subject) }
            };
        }

        internal override bool Handle(CommunicationInterface communicationInterface, SystemOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(communicationInterface, operationCode, parameters))
            {
                string debugMessage;
                SystemFetchDataCode fetchCode = (SystemFetchDataCode)parameters[(byte)FetchDataParameterCode.FetchDataCode];
                Dictionary<byte, object> resolvedParameters = (Dictionary<byte, object>)parameters[(byte)FetchDataParameterCode.Parameters];
                if (fetchTable.ContainsKey(fetchCode))
                {
                    return fetchTable[fetchCode].Handle(communicationInterface, fetchCode, resolvedParameters);
                }
                else
                {
                    debugMessage = string.Format("System Fetch Operation Not Exist Fetch Code: {0}", fetchCode);
                    SendError(communicationInterface, operationCode, ErrorCode.InvalidOperation, debugMessage);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        internal void SendOperation(SystemFetchDataCode fetchCode, Dictionary<byte, object> parameters)
        {
            subject.OperationManager.SendFetchDataOperation(fetchCode, parameters);
        }

        public void FetchItem(int itemID)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)FetchItemParameterCode.ItemID, itemID }
            };
            SendOperation(SystemFetchDataCode.Item, parameters);
        }
        public void FetchAllItems()
        {
            SendOperation(SystemFetchDataCode.AllItems, new Dictionary<byte, object>());
        }
        public void FetchAllVessels()
        {
            SendOperation(SystemFetchDataCode.AllVessels, new Dictionary<byte, object>());
        }
        public void FetchVessel(int vesselID)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)FetchVesselParameterCode.VesselID, vesselID }
            };
            SendOperation(SystemFetchDataCode.Vessel, parameters);
        }
        public void FetchVesselWithOwnerPlayerID(int ownerPlayerID)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)FetchVesselWithOwnerPlayerIDParameterCode.OwnerPlayerID, ownerPlayerID }
            };
            SendOperation(SystemFetchDataCode.VesselWithOwnerPlayerID, parameters);
        }
        public void FetchVesselDecorations(int vesselID)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)FetchVesselDecorationsParameterCode.VesselID, vesselID }
            };
            SendOperation(SystemFetchDataCode.VesselDecorations, parameters);
        }
        public void FetchIslandTotalScore()
        {
            SendOperation(SystemFetchDataCode.IslandTotalScore, new Dictionary<byte, object>());
        }
        public void FetchIslandTodayMaterialRanking()
        {
            SendOperation(SystemFetchDataCode.IslandTodayMaterialRanking, new Dictionary<byte, object>());
        }
        public void FetchIslandPlayerScoreRanking()
        {
            SendOperation(SystemFetchDataCode.IslandPlayerScoreRanking, new Dictionary<byte, object>());
        }
        public void FetchAllLandmarks()
        {
            SendOperation(SystemFetchDataCode.AllLandmarks, new Dictionary<byte, object>());
        }
        public void FetchClientFunctionCheckTable()
        {
            SendOperation(SystemFetchDataCode.ClientFunctionCheckTable, new Dictionary<byte, object>());
        }
        public void FetchWorldChannelMessages()
        {
            SendOperation(SystemFetchDataCode.WorldChannelMessages, new Dictionary<byte, object>());
        }
    }
}
