using IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers;
using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataParameters;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.Player;
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
                { PlayerOperationCode.UpdateVesselTransform, new UpdateVesselTransformHandler(player) },
                { PlayerOperationCode.AddDecorationToVessel, new AddDecorationToVesselHandler(player) },
                { PlayerOperationCode.RemoveDecorationFromVessel, new RemoveDecorationFromVesselHandler(player) },
                { PlayerOperationCode.UpdateDecorationOnVessel, new UpdateDecorationOnVesselHandler(player) },
                { PlayerOperationCode.SynthesizeMaterial, new SynthesizeMaterialHandler(player) },
                { PlayerOperationCode.UseBlueprint, new UseBlueprintHandler(player) },
                { PlayerOperationCode.InviteFriend, new InviteFriendHandler(player) },
                { PlayerOperationCode.AcceptFriend, new AcceptFriendHandler(player) },
                { PlayerOperationCode.DeleteFriend, new DeleteFriendHandler(player) },
                { PlayerOperationCode.SendMessage, new SendMessageHandler(player) },
                { PlayerOperationCode.TransactionRequest, new TransactionRequestHandler(player) },
                { PlayerOperationCode.AcceptTransaction, new AcceptTransactionHandler(player) },
                { PlayerOperationCode.ChangeTransactionItem, new ChangeTransactionItemHandler(player) },
                { PlayerOperationCode.ConfirmTransaction, new ConfirmTransactionHandler(player) },
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
            player.User.OperationManager.SendPlayerOperation(player, operationCode, parameters);
        }

        internal void SendFetchDataOperation(PlayerFetchDataCode fetchCode, Dictionary<byte, object> parameters)
        {
            Dictionary<byte, object> fetchDataParameters = new Dictionary<byte, object>
            {
                { (byte)FetchDataParameterCode.FetchDataCode, (byte)fetchCode },
                { (byte)FetchDataParameterCode.Parameters, parameters }
            };
            SendOperation(PlayerOperationCode.FetchData, fetchDataParameters);
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
        public void UpdateVesselTransform(float locationX, float locationZ, float rotationEulerAngleY)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)UpdateVesselTransformParameterCode.LocationX, locationX },
                { (byte)UpdateVesselTransformParameterCode.LocatiomZ, locationZ },
                { (byte)UpdateVesselTransformParameterCode.EulerAngleY, rotationEulerAngleY }
            };
            SendOperation(PlayerOperationCode.UpdateVesselTransform, parameters);
        }
        public void AddDecorationToVessel(int materialItemID, float positionX, float positionY, float positionZ, float rotationEulerAngleX, float rotationEulerAngleY, float rotationEulerAngleZ)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)AddDecorationToVesselParameterCode.MaterialItemID, materialItemID },
                { (byte)AddDecorationToVesselParameterCode.PositionX, positionX },
                { (byte)AddDecorationToVesselParameterCode.PositionY, positionY },
                { (byte)AddDecorationToVesselParameterCode.PositionZ, positionZ },
                { (byte)AddDecorationToVesselParameterCode.RotationEulerAngleX, rotationEulerAngleX },
                { (byte)AddDecorationToVesselParameterCode.RotationEulerAngleY, rotationEulerAngleY },
                { (byte)AddDecorationToVesselParameterCode.RotationEulerAngleZ, rotationEulerAngleZ }
            };
            SendOperation(PlayerOperationCode.AddDecorationToVessel, parameters);
        }
        public void RemoveDecorationFromVessel(int decorationID)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)RemoveDecorationFromVesselParameterCode.DecorationID, decorationID }
            };
            SendOperation(PlayerOperationCode.RemoveDecorationFromVessel, parameters);
        }
        public void UpdateDecorationOnVessel(int decorationID, float positionX, float positionY, float positionZ, float rotationEulerAngleX, float rotationEulerAngleY, float rotationEulerAngleZ)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)UpdateDecorationOnVesselParameterCode.DecorationID, decorationID },
                { (byte)UpdateDecorationOnVesselParameterCode.PositionX, positionX },
                { (byte)UpdateDecorationOnVesselParameterCode.PositionY, positionY },
                { (byte)UpdateDecorationOnVesselParameterCode.PositionZ, positionZ },
                { (byte)UpdateDecorationOnVesselParameterCode.RotationEulerAngleX, rotationEulerAngleX },
                { (byte)UpdateDecorationOnVesselParameterCode.RotationEulerAngleY, rotationEulerAngleY },
                { (byte)UpdateDecorationOnVesselParameterCode.RotationEulerAngleZ, rotationEulerAngleZ }
            };
            SendOperation(PlayerOperationCode.UpdateDecorationOnVessel, parameters);
        }
        public void SynthesizeMaterial(Blueprint.ElementInfo[] elementInfos)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)SynthesizeMaterialParameterCode.BlueprintElementInfos, elementInfos }
            };
            SendOperation(PlayerOperationCode.SynthesizeMaterial, parameters);
        }
        public void UseBlueprint(int blueprintID)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)UseBlueprintParameterCode.BlueprintID, blueprintID }
            };
            SendOperation(PlayerOperationCode.UseBlueprint, parameters);
        }
        public void InviteFriend(int accepterPlayerID)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)InviteFriendParameterCode.AccepterPlayerID, accepterPlayerID }
            };
            SendOperation(PlayerOperationCode.InviteFriend, parameters);
        }
        public void AcceptFriend(int inviterPlayerID)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)AcceptFriendParameterCode.InviterPlayerID, inviterPlayerID }
            };
            SendOperation(PlayerOperationCode.AcceptFriend, parameters);
        }
        public void DeleteFriend(int targetPlayerID)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)DeleteFriendParameterCode.TargetPlayerID, targetPlayerID }
            };
            SendOperation(PlayerOperationCode.DeleteFriend, parameters);
        }
        public void SendMessage(int receiverPlayerID, string content)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)SendMessageParameterCode.ReceiverPlayerID, receiverPlayerID },
                { (byte)SendMessageParameterCode.Content, content }
            };
            SendOperation(PlayerOperationCode.SendMessage, parameters);
        }
        public void TransactionRequest(int accepterPlayerID)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)TransactionRequestParameterCode.AccepterPlayerID, accepterPlayerID }
            };
            SendOperation(PlayerOperationCode.TransactionRequest, parameters);
        }
        public void AcceptTransaction(int requesterPlayerID)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)AcceptTransactionParameterCode.RequesterPlayerID, requesterPlayerID }
            };
            SendOperation(PlayerOperationCode.AcceptTransaction, parameters);
        }
        public void ChangeTransactionItem(int transactionID, DataChangeType changeType, TransactionItemInfo info)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)ChangeTransactionItemParameterCode.TransactionID, transactionID },
                { (byte)ChangeTransactionItemParameterCode.DataChangeType, (byte)changeType },
                { (byte)ChangeTransactionItemParameterCode.ItemID, info.Item.ItemID },
                { (byte)ChangeTransactionItemParameterCode.ItemCount, info.Count },
                { (byte)ChangeTransactionItemParameterCode.PositionIndex, info.PositionIndex }
            };
            SendOperation(PlayerOperationCode.ChangeTransactionItem, parameters);
        }
        public void ConfirmTransaction(int transactionID)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)ConfirmTransactionParameterCode.TransactionID, transactionID }
            };
            SendOperation(PlayerOperationCode.ConfirmTransaction, parameters);
        }
    }
}
