using IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.PlayerResponseHandlers;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Managers
{
    public class PlayerResponseManager
    {
        protected readonly Dictionary<PlayerOperationCode, ResponseHandler<Player, PlayerOperationCode>> operationTable;
        protected readonly Player player;

        public delegate void CreateCharacterResponseEventHandler(ErrorCode returnCode, Player player);
        private event CreateCharacterResponseEventHandler onCreateCharacterResponse;
        public event CreateCharacterResponseEventHandler OnCreateCharacterResponse { add { onCreateCharacterResponse += value; } remove { onCreateCharacterResponse -= value; } }

        public delegate void DrawMaterialResponseEventHandler(ErrorCode returnCode, Item material, int count);
        private event DrawMaterialResponseEventHandler onDrawMaterialResponse;
        public event DrawMaterialResponseEventHandler OnDrawMaterialResponse { add { onDrawMaterialResponse += value; } remove { onDrawMaterialResponse -= value; } }

        public delegate void SynthesizeMaterialResponseEventHandler(ErrorCode returnCode, Blueprint.ElementInfo[] requirements, Blueprint.ElementInfo[] products);
        private event SynthesizeMaterialResponseEventHandler onSynthesizeMaterialResponse;
        public event SynthesizeMaterialResponseEventHandler OnSynthesizeMaterialResponse { add { onSynthesizeMaterialResponse += value; } remove { onSynthesizeMaterialResponse -= value; } }

        public delegate void UseBlueprintResponseEventHandler(ErrorCode returnCode, Blueprint blueprint);
        private event UseBlueprintResponseEventHandler onUseBlueprintResponse;
        public event UseBlueprintResponseEventHandler OnUseBlueprintResponse { add { onUseBlueprintResponse += value; } remove { onUseBlueprintResponse -= value; } }

        public PlayerResponseManager(Player player)
        {
            this.player = player;
            operationTable = new Dictionary<PlayerOperationCode, ResponseHandler<Player, PlayerOperationCode>>
            {
                { PlayerOperationCode.FetchData, new PlayerFetchDataResponseResolver(player) },
                { PlayerOperationCode.CreateCharacter, new CreateCharacterResponseHandler(player) },
                { PlayerOperationCode.DrawMaterial, new DrawMaterialResponseHandler(player) },
                { PlayerOperationCode.SynthesizeMaterial, new SynthesizeMaterialResponseHandler(player) },
                { PlayerOperationCode.UseBlueprint, new UseBlueprintResponseHandler(player) },
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
            player.User.ResponseManager.SendPlayerResponse(player, operationCode, errorCode, debugMessage, parameters);
        }

        internal void DrawMaterialResponse(ErrorCode returnCode, int itemID, int itemCount)
        {
            Item item;
            if (ItemManager.Instance.FindItem(itemID, out item))
            {
                onDrawMaterialResponse?.Invoke(returnCode, item, itemCount);
            }
        }
        internal void SynthesizeMaterialResponse(ErrorCode returnCode, Blueprint.ElementInfo[] requirements, Blueprint.ElementInfo[] products)
        {
            onSynthesizeMaterialResponse?.Invoke(returnCode, requirements, products);
        }
        internal void UseBlueprintResponse(ErrorCode returnCode, Blueprint blueprint)
        {
            onUseBlueprintResponse?.Invoke(returnCode, blueprint);
        }
        internal void CreateCharacterResponse(ErrorCode returnCode, Player player)
        {
            onCreateCharacterResponse?.Invoke(returnCode, player);
        }
    }
}
