using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.PlayerEventHandlers;
using IsolatedIslandGame.Library.TextData;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.EventParameters.Player;
using IsolatedIslandGame.Protocol.Communication.SyncDataCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataParameters;
using System.Collections.Generic;
using System.Linq;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Managers
{
    public class PlayerEventManager
    {
        private readonly Dictionary<PlayerEventCode, EventHandler<Player, PlayerEventCode>> eventTable;
        protected readonly Player player;
        public PlayerSyncDataResolver SyncDataResolver { get; protected set; }

        internal PlayerEventManager(Player player)
        {
            this.player = player;
            SyncDataResolver = new PlayerSyncDataResolver(player);
            eventTable = new Dictionary<PlayerEventCode, EventHandler<Player, PlayerEventCode>>
            {
                { PlayerEventCode.SyncData, SyncDataResolver },
                { PlayerEventCode.GetBlueprint, new GetBlueprintHandler(player) },
                { PlayerEventCode.GetPlayerConversation, new GetPlayerConversationHandler(player) },
                { PlayerEventCode.TransactionRequest, new TransactionRequestHandler(player) },
                { PlayerEventCode.StartTransaction, new StartTransactionHandler(player) },
                { PlayerEventCode.EndTransaction, new EndTransactionHandler(player) },
            };
        }

        internal void Operate(PlayerEventCode eventCode, Dictionary<byte, object> parameters)
        {
            if (eventTable.ContainsKey(eventCode))
            {
                if (!eventTable[eventCode].Handle(eventCode, parameters))
                {
                    LogService.ErrorFormat("Player Event Error: {0} from PlayerID: {1}", eventCode, player.PlayerID);
                }
            }
            else
            {
                LogService.ErrorFormat("Unknow Player Event:{0} from PlayerID: {1}", eventCode, player.PlayerID);
            }
        }

        internal void SendEvent(PlayerEventCode eventCode, Dictionary<byte, object> parameters)
        {
            player.User.EventManager.SendPlayerEvent(player, eventCode, parameters);
        }

        public void ErrorInform(string title, string message)
        {
            player.User.EventManager.ErrorInform(title, message);
        }

        internal void SendSyncDataEvent(PlayerSyncDataCode syncCode, Dictionary<byte, object> parameters)
        {
            Dictionary<byte, object> syncDataParameters = new Dictionary<byte, object>
            {
                { (byte)SyncDataEventParameterCode.SyncCode, (byte)syncCode },
                { (byte)SyncDataEventParameterCode.Parameters, parameters }
            };
            SendEvent(PlayerEventCode.SyncData, syncDataParameters);
        }

        public void GetBlueprint(Blueprint blueprint)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)GetBlueprintParameterCode.BlueprintID, blueprint.BlueprintID },
                { (byte)GetBlueprintParameterCode.BlueprintID, blueprint.IsOrderless },
                { (byte)GetBlueprintParameterCode.BlueprintID, blueprint.IsBlueprintRequired },
                { (byte)GetBlueprintParameterCode.Requirements, blueprint.Requirements.ToArray() },
                { (byte)GetBlueprintParameterCode.Products, blueprint.Products.ToArray() }
            };
            SendEvent(PlayerEventCode.GetBlueprint, parameters);
        }
        public void GetPlayerConversation(PlayerConversation playerConversation)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)GetPlayerConversationParameterCode.PlayerMessageID, playerConversation.message.playerMessageID },
                { (byte)GetPlayerConversationParameterCode.SenderPlayerID, playerConversation.message.senderPlayerID },
                { (byte)GetPlayerConversationParameterCode.SendTime, playerConversation.message.sendTime.ToBinary() },
                { (byte)GetPlayerConversationParameterCode.Content, playerConversation.message.content },
                { (byte)GetPlayerConversationParameterCode.HasRead, playerConversation.hasRead }
            };
            SendEvent(PlayerEventCode.GetPlayerConversation, parameters);
        }
        public void TransactionRequest(int requesterPlayerID)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)TransactionRequestParameterCode.RequesterPlayerID, requesterPlayerID }
            };
            SendEvent(PlayerEventCode.TransactionRequest, parameters);
        }
        public void StartTransaction(Transaction transaction)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)StartTransactionParameterCode.TransactionID, transaction.TransactionID },
                { (byte)StartTransactionParameterCode.RequesterPlayerID, transaction.RequesterPlayerID },
                { (byte)StartTransactionParameterCode.AccepterPlayerID, transaction.AccepterPlayerID }
            };
            SendEvent(PlayerEventCode.StartTransaction, parameters);
        }
        public void EndTransaction(int transactionID, bool isSuccessful)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)EndTransactionParameterCode.TransactionID, transactionID },
                { (byte)EndTransactionParameterCode.IsSuccessful, isSuccessful }
            };
            SendEvent(PlayerEventCode.EndTransaction, parameters);
        }
    }
}
