using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library
{
    public class Vessel
    {
        public int VesselID { get; private set; }
        public PlayerInformation PlayerInformation { get; private set; }
        public float LocationX { get; private set; }
        public float LocationZ { get; private set; }
        public float RotationEulerAngleY { get; private set; }

        private Dictionary<int, Decoration> decorationDictionary;
        public int DecorationCount { get { return decorationDictionary.Count; } }
        public IEnumerable<Decoration> Decorations { get { return decorationDictionary.Values; } }

        public delegate void DecorationChangeEventHandler(DataChangeType changeType, int vesselID, Decoration decoration);
        private event DecorationChangeEventHandler onDecorationChange;
        public event DecorationChangeEventHandler OnDecorationChange { add { onDecorationChange += value; } remove { onDecorationChange -= value; } }

        public delegate void VesselTransformUpdatedEventHandler(int vesselID, float locationX, float locationY,float rotationEulerAngleY);
        private event VesselTransformUpdatedEventHandler onVesselTransformUpdated;
        public event VesselTransformUpdatedEventHandler OnVesselTransformUpdated { add { onVesselTransformUpdated += value; } remove { onVesselTransformUpdated -= value; } }

        private event Action<Vessel> onVesselFullDataUpdated;
        public event Action<Vessel> OnVesselFullDataUpdated { add { onVesselFullDataUpdated += value; } remove { onVesselFullDataUpdated -= value; } }

        public Vessel(int vesselID, PlayerInformation playerInformation, float locationX, float locationZ, float rotationEulerAngleY)
        {
            VesselID = vesselID;
            PlayerInformation = playerInformation;
            LocationX = locationX;
            LocationZ = locationZ;
            RotationEulerAngleY = rotationEulerAngleY;

            decorationDictionary = new Dictionary<int, Decoration>();
        }
        public void UpdateFullData(Vessel vessel)
        {
            VesselID = vessel.VesselID;
            PlayerInformation = vessel.PlayerInformation;
            LocationX = vessel.LocationX;
            LocationZ = vessel.LocationZ;
            RotationEulerAngleY = vessel.RotationEulerAngleY;

            onVesselFullDataUpdated?.Invoke(this);
        }
        public void UpdateTransform(float locationX, float locationZ, float rotationEulerAngleY)
        {
            LocationX = locationX;
            LocationZ = locationZ;
            RotationEulerAngleY = rotationEulerAngleY;
            onVesselTransformUpdated?.Invoke(VesselID, LocationX, LocationZ, rotationEulerAngleY);
        }
        public bool ContainsDecoration(int decorationID)
        {
            return decorationDictionary.ContainsKey(decorationID);
        }
        public bool FindDecoration(int decorationID, out Decoration decoration)
        {
            if (ContainsDecoration(decorationID))
            {
                decoration = decorationDictionary[decorationID];
                return true;
            }
            else
            {
                decoration = null;
                return false;
            }
        }
        public void AddDecoration(Decoration decoration)
        {
            if (!ContainsDecoration(decoration.DecorationID))
            {
                decorationDictionary.Add(decoration.DecorationID, decoration);
                onDecorationChange?.Invoke(DataChangeType.Add, VesselID, decoration);
                decoration.OnDecorationUpdate += UpdateDecoration;
            }
            else
            {
                decorationDictionary[decoration.DecorationID].UpdateDecoration(decoration.PositionX, decoration.PositionY, decoration.PositionZ, decoration.RotationEulerAngleX, decoration.RotationEulerAngleY, decoration.RotationEulerAngleZ);
            }
        }
        public bool RemoveDecoration(int decorationID)
        {
            if (ContainsDecoration(decorationID))
            {
                Decoration decoration = decorationDictionary[decorationID];
                decorationDictionary.Remove(decorationID);
                onDecorationChange?.Invoke(DataChangeType.Remove, VesselID, decoration);
                return true;
            }
            else
            {
                return false;
            }
        }
        private void UpdateDecoration(Decoration decoration)
        {
            onDecorationChange?.Invoke(DataChangeType.Update, VesselID, decoration);
        }
    }
}
