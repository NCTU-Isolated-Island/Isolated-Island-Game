using UnityEngine;
using System;

namespace IsolatedIslandGame.Library.Items
{
    public class Decoration
    {
        public int DecorationID { get; private set; }
        public Material Material { get; private set; }

        public  Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; }

        private event Action<Decoration> onDecorationUpdate;
        public event Action<Decoration> OnDecorationUpdate { add { onDecorationUpdate += value; } remove { onDecorationUpdate -= value; } }

        public Decoration(int decorationID, Material material, Vector3 position, Quaternion rotation)
        {
            DecorationID = decorationID;
            Material = material;
            Position = position;
            Rotation = rotation;
        }

        public void UpdateDecoration(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
            onDecorationUpdate?.Invoke(this);
        }
    }
}
