﻿using IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.SystemOperationHandlers.FetchDataHandlers;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataParameters.System;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.SystemOperationHandlers
{
    public class SystemFetchDataResolver : FetchDataResolver<SystemManager, SystemOperationCode, SystemFetchDataCode>
    {
        public SystemFetchDataResolver(SystemManager subject) : base(subject)
        {
            fetchTable.Add(SystemFetchDataCode.Item, new FetchItemHandler(subject));
        }

        internal override void SendResponse(SystemOperationCode operationCode, Dictionary<byte, object> parameter)
        {
            subject.ResponseManager.SendResponse(operationCode, ErrorCode.NoError, null, parameter);
        }
        internal override void SendError(SystemOperationCode operationCode, ErrorCode errorCode, string debugMessage)
        {
            base.SendError(operationCode, errorCode, debugMessage);
            Dictionary<byte, object> parameters = new Dictionary<byte, object>();
            subject.ResponseManager.SendResponse(operationCode, errorCode, debugMessage, parameters);
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
    }
}
