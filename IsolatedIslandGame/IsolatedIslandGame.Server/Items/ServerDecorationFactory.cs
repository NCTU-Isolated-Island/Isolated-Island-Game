using System;
using IsolatedIslandGame.Database;
using IsolatedIslandGame.Library.Items;

namespace IsolatedIslandGame.Server.Items
{
    public class ServerDecorationFactory : DecorationFactory
    {
        public override Decoration CreateDecoration(int vesselID, Material material, float positionX, float positionY, float positionZ, float rotationEulerAngleX, float rotationEulerAngleY, float rotationEulerAngleZ)
        {
            return DatabaseService.RepositoryList.DecorationRepository.Create(
                vesselID: vesselID,
                materialItemID: material.ItemID,
                positionX: positionX,
                positionY: positionY,
                positionZ: positionZ,
                eulerAngleX: rotationEulerAngleX,
                eulerAngleY: rotationEulerAngleX,
                eulerAngleZ: rotationEulerAngleX);
        }

        public override void DeleteDecoration(int decorationID)
        {
            DatabaseService.RepositoryList.DecorationRepository.Delete(decorationID);
        }
    }
}
