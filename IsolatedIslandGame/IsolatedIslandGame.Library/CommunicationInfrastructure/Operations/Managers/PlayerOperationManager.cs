using IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.Player;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.User;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Managers
{
    public class PlayerOperationManager
    {
        private readonly Dictionary<PlayerOperationCode, OperationHandler<Player, PlayerOperationCode>> operationTable;
        protected readonly Player player;
        public PlayerFetchDataResolver FetchDataResolver { get; protected set; }

        internal PlayerOperationManager(Player player)
        {
            this.player = player;
            FetchDataResolver = new PlayerFetchDataResolver(player);
            operationTable = new Dictionary<PlayerOperationCode, OperationHandler<Player, PlayerOperationCode>>
            {
                { PlayerOperationCode.FetchData, FetchDataResolver },
                { PlayerOperationCode.CreateCharacter, new CreateCharacterHandler(player) },
                { PlayerOperationCode.DrawMaterial, new DrawMaterialHandler(player) },
            };
        }

        internal void Operate(PlayerOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (operationTable.ContainsKey(operationCode))
            {
                if (!operationTable[operationCode].Handle(operationCode, parameters))
                {
                    LogService.ErrorFormat("Player Operation Error: {0} from Identity: {1}", operationCode, player.IdentityInformation);
                }
            }
            else
            {
                LogService.ErrorFormat("Unknow Player Operation:{0} from Identity: {1}", operationCode, player.IdentityInformation);
            }
        }

        internal void SendOperation(PlayerOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            Dictionary<byte, object> operationData = new Dictionary<byte, object>
            {
                { (byte)PlayerOperationParameterCode.PlayerID, player.PlayerID },
                { (byte)PlayerOperationParameterCode.OperationCode, (byte)operationCode },
                { (byte)PlayerOperationParameterCode.Parameters, parameters }
            };
            player.User.OperationManager.SendOperation(UserOperationCode.PlayerOperation, operationData);
        }

        public void CreateCharacter(string nickname, string signature, GroupType groupType)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)CreateCharacterParameterCode.Nickname, nickname },
                { (byte)CreateCharacterParameterCode.Signature, signature },
                { (byte)CreateCharacterParameterCode.GroupType, groupType }
            };
            SendOperation(PlayerOperationCode.CreateCharacter, parameters);
        }
        public void DrawMaterial()
        {
            SendOperation(PlayerOperationCode.DrawMaterial, new Dictionary<byte, object>());
        }
    }
}
