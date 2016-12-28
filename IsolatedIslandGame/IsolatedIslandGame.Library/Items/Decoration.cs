using System;

namespace IsolatedIslandGame.Library.Items
{
    public class Decoration
    {
        public int DecorationID { get; private set; }
        public Material Material { get; private set; }

        public  float PositionX { get; private set; }
        public float PositionY { get; private set; }
        public float PositionZ { get; private set; }
        public float RotationEulerAngleX { get; private set; }
        public float RotationEulerAngleY { get; private set; }
        public float RotationEulerAngleZ { get; private set; }

        private event Action<Decoration> onDecorationUpdate;
        public event Action<Decoration> OnDecorationUpdate { add { onDecorationUpdate += value; } remove { onDecorationUpdate -= value; } }

        public Decoration(int decorationID, Material material, float positionX, float positionY, float positionZ, float rotationEulerAngleX, float rotationEulerAngleY, float rotationEulerAngleZ)
        {
            DecorationID = decorationID;
            Material = material;
            PositionX = positionX;
            PositionY = positionY;
            PositionZ = positionZ;
            RotationEulerAngleX = rotationEulerAngleX;
            RotationEulerAngleY = rotationEulerAngleY;
            RotationEulerAngleZ = rotationEulerAngleZ;

            ItemManager.Instance.OnItemUpdate += UpdateMaterial;
        }

        ~Decoration()
        {
            ItemManager.Instance.OnItemUpdate -= UpdateMaterial;
        }

        private void UpdateMaterial(Item material)
        {
            if (material.ItemID == Material.ItemID)
            {
                Material = material as Material;
            }
        }

        public void UpdateDecoration(float positionX, float positionY, float positionZ, float rotationEulerAngleX, float rotationEulerAngleY, float rotationEulerAngleZ)
        {
            PositionX = positionX;
            PositionY = positionY;
            PositionZ = positionZ;
            RotationEulerAngleX = rotationEulerAngleX;
            RotationEulerAngleY = rotationEulerAngleY;
            RotationEulerAngleZ = rotationEulerAngleZ;
            onDecorationUpdate?.Invoke(this);
        }
    }
}
