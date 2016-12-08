using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using System.Collections.Generic;
using UnityEngine;

namespace IsolatedIslandGame.Library
{
    public class Vessel
    {
        public int VesselID { get; private set; }
        public int OwnerPlayerID { get; private set; }
        public string Name { get; private set; }
        public float LocationX { get; private set; }
        public float LocationZ { get; private set; }
        public Quaternion Rotation { get; private set; }

        private Dictionary<int, Decoration> decorationDictionary;
        public int DecorationCount { get { return decorationDictionary.Count; } }
        public IEnumerable<Decoration> Decorations { get { return decorationDictionary.Values; } }

        public delegate void DecorationChangeEventHandler(int vesselID, Decoration decoration, DataChangeType changeType);
        private event DecorationChangeEventHandler onDecorationChange;
        public event DecorationChangeEventHandler OnDecorationChange { add { onDecorationChange += value; } remove { onDecorationChange -= value; } }

        public delegate void VesselTransformUpdatedEventHandler(int vesselID, float locationX, float locationY, Quaternion rotation);
        private event VesselTransformUpdatedEventHandler onVesselTransformUpdated;
        public event VesselTransformUpdatedEventHandler OnVesselTransformUpdated { add { onVesselTransformUpdated += value; } remove { onVesselTransformUpdated -= value; } }

        public Vessel(int vesselID, int ownerPlayerID, string name, float locationX, float locationZ, Quaternion roration)
        {
            VesselID = vesselID;
            OwnerPlayerID = ownerPlayerID;
            Name = name;
            LocationX = locationX;
            LocationZ = locationZ;
            Rotation = roration;

            decorationDictionary = new Dictionary<int, Decoration>();
        }
        public void UpdateTransform(float locationX, float locationZ, Quaternion rotation)
        {
            LocationX = locationX;
            LocationZ = locationZ;
            Rotation = rotation;
            onVesselTransformUpdated?.Invoke(VesselID, LocationX, LocationZ, Rotation);
        }
        public bool ContainsDecoration(int decorationID)
        {
            return decorationDictionary.ContainsKey(decorationID);
        }
        public Decoration FindDecoration(int decorationID)
        {
            if (ContainsDecoration(decorationID))
            {
                return decorationDictionary[decorationID];
            }
            else
            {
                return null;
            }
        }
        public void AddDecoration(Decoration decoration)
        {
            if (!ContainsDecoration(decoration.DecorationID))
            {
                decorationDictionary.Add(decoration.DecorationID, decoration);
                onDecorationChange?.Invoke(VesselID, decoration, DataChangeType.Add);
                decoration.OnDecorationUpdate += UpdateDecoration;
            }
            else
            {
                decorationDictionary[decoration.DecorationID].UpdateDecoration(decoration.Position, decoration.Rotation);
            }
        }
        public bool RemoveDecoration(int decorationID)
        {
            if (ContainsDecoration(decorationID))
            {
                Decoration decoration = decorationDictionary[decorationID];
                decorationDictionary.Remove(decorationID);
                onDecorationChange?.Invoke(VesselID, decoration, DataChangeType.Remove);
                return true;
            }
            else
            {
                return false;
            }
        }
        private void UpdateDecoration(Decoration decoration)
        {
            onDecorationChange?.Invoke(VesselID, decoration, DataChangeType.Update);
        }
    }
}
