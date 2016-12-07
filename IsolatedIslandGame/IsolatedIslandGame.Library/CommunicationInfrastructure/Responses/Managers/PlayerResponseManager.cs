using IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.PlayerResponseHandlers;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.ResponseParameters.User;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Managers
{
    public class PlayerResponseManager
    {
        protected readonly Dictionary<PlayerOperationCode, ResponseHandler<Player, PlayerOperationCode>> operationTable;
        protected readonly Player player;

        public PlayerResponseManager(Player player)
        {
            this.player = player;
            operationTable = new Dictionary<PlayerOperationCode, ResponseHandler<Player, PlayerOperationCode>>
            {
                { PlayerOperationCode.FetchData, new PlayerFetchDataResponseResolver(player) },
                { PlayerOperationCode.CreateCharacter, new CreateCharacterResponseHandler(player) },
                { PlayerOperationCode.DrawMaterial, new DrawMaterialResponseHandler(player) },
            };
        }

        public void Operate(PlayerOperationCode operationCode, ErrorCode returnCode, string debugMessage, Dictionary<byte, object> parameters)
        {
            if (operationTable.ContainsKey(operationCode))
            {
                if (!operationTable[operationCode].Handle(operationCode, returnCode, debugMessage, parameters))
                {
                    LogService.ErrorFormat("Player Response Error: {0} from Identity: {1}", operationCode, player.IdentityInformation);
                }
            }
            else
            {
                LogService.ErrorFormat("Unknow Player Response:{0} from Identity: {1}", operationCode, player.IdentityInformation);
            }
        }

        internal void SendResponse(PlayerOperationCode operationCode, ErrorCode errorCode, string debugMessage, Dictionary<byte, object> parameters)
        {
            Dictionary<byte, object> responseData = new Dictionary<byte, object>
            {
                { (byte)PlayerResponseParameterCode.PlayerID, player.PlayerID },
                { (byte)PlayerResponseParameterCode.OperationCode, (byte)operationCode },
                { (byte)PlayerResponseParameterCode.ReturnCode, (short)errorCode },
                { (byte)PlayerResponseParameterCode.DebugMessage, debugMessage },
                { (byte)PlayerResponseParameterCode.Parameters, parameters }
            };
            player.User.ResponseManager.SendResponse(UserOperationCode.PlayerOperation, ErrorCode.NoError, null, responseData);
        }
    }
}
