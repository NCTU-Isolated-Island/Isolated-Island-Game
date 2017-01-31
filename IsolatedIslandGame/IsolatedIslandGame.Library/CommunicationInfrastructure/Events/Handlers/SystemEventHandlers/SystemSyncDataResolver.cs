using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.SystemEventHandlers.SyncDataHandlers;
using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataParameters.System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.SystemEventHandlers
{
    public class SystemSyncDataResolver : SyncDataResolver<SystemManager, SystemEventCode, SystemSyncDataCode>
    {
        internal SystemSyncDataResolver(SystemManager systemManager) : base(systemManager)
        {
            syncTable.Add(SystemSyncDataCode.VesselChange, new SyncVesselChangeHandler(subject));
            syncTable.Add(SystemSyncDataCode.VesselTransform, new SyncVesselTransformHandler(subject));
            syncTable.Add(SystemSyncDataCode.VesselDecorationChange, new SyncVesselDecorationChangeHandler(subject));
            syncTable.Add(SystemSyncDataCode.PlayerInformation, new SyncPlayerInformationHandler(subject));
            syncTable.Add(SystemSyncDataCode.IslandTotalScoreUpdated, new SyncIslandTotalScoreUpdatedHandler(subject));
            syncTable.Add(SystemSyncDataCode.IslandTodayMaterialRankingUpdated, new SyncIslandTodayMaterialRankingUpdatedHandler(subject));
            syncTable.Add(SystemSyncDataCode.IslandPlayerScoreRankingUpdated, new SyncIslandPlayerScoreRankingUpdatedHandler(subject));
        }

        internal override void SendSyncData(SystemSyncDataCode syncCode, Dictionary<byte, object> parameters)
        {
            subject.EventManager.SendSyncDataEvent(syncCode, parameters);
        }

        public void SyncVesselChange(DataChangeType changeType, Vessel vessel)
        {
            PlayerInformation playerInformation;
            if(PlayerInformationManager.Instance.FindPlayerInformation(vessel.OwnerPlayerID, out playerInformation))
            {
                subject.EventManager.SyncDataResolver.SyncPlayerInformation(playerInformation);
            }
            var parameters = new Dictionary<byte, object>
            {
                { (byte)SyncVesselChangeParameterCode.DataChangeType, (byte)changeType },
                { (byte)SyncVesselChangeParameterCode.VesselID, vessel.VesselID },
                { (byte)SyncVesselChangeParameterCode.OwnerPlayerID, vessel.OwnerPlayerID },
                { (byte)SyncVesselChangeParameterCode.LocationX, vessel.LocationX },
                { (byte)SyncVesselChangeParameterCode.LocationZ, vessel.LocationZ },
                { (byte)SyncVesselChangeParameterCode.EulerAngleY, vessel.RotationEulerAngleY },
                { (byte)SyncVesselChangeParameterCode.LocatedOceanType, vessel.LocatedOceanType }
            };
            SendSyncData(SystemSyncDataCode.VesselChange, parameters);
        }
        public void SyncVesselTransform(int vesselID, float locationX, float locationY, float rotationEulerAngleY, OceanType locatedOceanType)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)SyncVesselTransformParameterCode.VesselID, vesselID },
                { (byte)SyncVesselTransformParameterCode.LocationX, locationX },
                { (byte)SyncVesselTransformParameterCode.LocationZ, locationY },
                { (byte)SyncVesselTransformParameterCode.EulerAngleY, rotationEulerAngleY },
                { (byte)SyncVesselTransformParameterCode.LocatedOceanType, locatedOceanType }
            };
            SendSyncData(SystemSyncDataCode.VesselTransform, parameters);
        }
        public void SyncVesselDecorationChange(DataChangeType changeType, int vesselID, Decoration decoration)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)SyncVesselDecorationChangeParameterCode.DataChangeType, (byte)changeType },
                { (byte)SyncVesselDecorationChangeParameterCode.VesselID, vesselID },
                { (byte)SyncVesselDecorationChangeParameterCode.DecorationID, decoration.DecorationID },
                { (byte)SyncVesselDecorationChangeParameterCode.MaterialItemID, decoration.Material.ItemID },
                { (byte)SyncVesselDecorationChangeParameterCode.PositionX, decoration.PositionX },
                { (byte)SyncVesselDecorationChangeParameterCode.PositionY, decoration.PositionY },
                { (byte)SyncVesselDecorationChangeParameterCode.PositionZ, decoration.PositionZ },
                { (byte)SyncVesselDecorationChangeParameterCode.EulerAngleX, decoration.RotationEulerAngleX },
                { (byte)SyncVesselDecorationChangeParameterCode.EulerAngleY, decoration.RotationEulerAngleY },
                { (byte)SyncVesselDecorationChangeParameterCode.EulerAngleZ, decoration.RotationEulerAngleZ }
            };
            SendSyncData(SystemSyncDataCode.VesselDecorationChange, parameters);
        }
        public void SyncPlayerInformation(PlayerInformation playerInformation)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)SyncPlayerInformationParameterCode.PlayerID, playerInformation.playerID },
                { (byte)SyncPlayerInformationParameterCode.Nickname, playerInformation.nickname },
                { (byte)SyncPlayerInformationParameterCode.Signature, playerInformation.signature },
                { (byte)SyncPlayerInformationParameterCode.GroupType, (byte)playerInformation.groupType },
                { (byte)SyncPlayerInformationParameterCode.VesselID, playerInformation.vesselID }
            };
            SendSyncData(SystemSyncDataCode.PlayerInformation, parameters);
        }
        public void SyncIslandTotalScoreUpdated(GroupType groupType, int totalScore)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)SyncIslandTotalScoreUpdatedParameterCode.GroupType, (byte)groupType },
                { (byte)SyncIslandTotalScoreUpdatedParameterCode.TotalScore, totalScore }
            };
            SendSyncData(SystemSyncDataCode.IslandTotalScoreUpdated, parameters);
        }
        public void SyncIslandTodayMaterialRankingUpdated(Island.PlayerMaterialInfo info)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)SyncIslandTodayMaterialRankingUpdatedParameterCode.PlayerID, info.playerID },
                { (byte)SyncIslandTodayMaterialRankingUpdatedParameterCode.MaterialItemID, info.materialItemID }
            };
            SendSyncData(SystemSyncDataCode.IslandTodayMaterialRankingUpdated, parameters);
        }
        public void SyncIslandPlayerScoreRankingUpdated(Island.PlayerScoreInfo info)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)SyncIslandPlayerScoreRankingUpdatedParameterCode.PlayerID, info.playerID },
                { (byte)SyncIslandPlayerScoreRankingUpdatedParameterCode.Score, info.score }
            };
            SendSyncData(SystemSyncDataCode.IslandPlayerScoreRankingUpdated, parameters);
        }
    }
}
