using IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers;
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
        public void UpdateVesselTransform(float locationX, float locationZ, float rotationEulerAngleX, float rotationEulerAngleY, float rotationEulerAngleZ)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)UpdateVesselTransformParameterCode.LocationX, locationX },
                { (byte)UpdateVesselTransformParameterCode.LocatiomZ, locationZ },
                { (byte)UpdateVesselTransformParameterCode.EulerAngleX, rotationEulerAngleX },
                { (byte)UpdateVesselTransformParameterCode.EulerAngleY, rotationEulerAngleY },
                { (byte)UpdateVesselTransformParameterCode.EulerAngleZ, rotationEulerAngleZ }
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
    }
}
