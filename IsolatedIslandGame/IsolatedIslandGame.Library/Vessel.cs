using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library
{
    public class Vessel
    {
        public int VesselID { get; private set; }
        public int OwnerPlayerID { get; private set; }
        public float LocationX { get; private set; }
        public float LocationZ { get; private set; }
        public float RotationEulerAngleY { get; private set; }
        private OceanType locatedOceanType;
        public OceanType LocatedOceanType
        {
            get { return locatedOceanType; }
            private set
            {
                OceanType originOcean = locatedOceanType;
                locatedOceanType = value;
                if (originOcean != locatedOceanType)
                {
                    onOceanTypeChanged?.Invoke(this);
                }
            }
        }

        private Dictionary<int, Decoration> decorationDictionary;
        public int DecorationCount { get { return decorationDictionary.Count; } }
        public IEnumerable<Decoration> Decorations { get { return decorationDictionary.Values; } }

        public delegate void DecorationChangeEventHandler(DataChangeType changeType, int vesselID, Decoration decoration);
        private event DecorationChangeEventHandler onDecorationChange;
        public event DecorationChangeEventHandler OnDecorationChange { add { onDecorationChange += value; } remove { onDecorationChange -= value; } }

        public delegate void VesselTransformUpdatedEventHandler(int vesselID, float locationX, float locationY,float rotationEulerAngleY, OceanType locatedOceanType);
        private event VesselTransformUpdatedEventHandler onVesselTransformUpdated;
        public event VesselTransformUpdatedEventHandler OnVesselTransformUpdated { add { onVesselTransformUpdated += value; } remove { onVesselTransformUpdated -= value; } }

        private event Action<Vessel> onVesselFullDataUpdated;
        public event Action<Vessel> OnVesselFullDataUpdated { add { onVesselFullDataUpdated += value; } remove { onVesselFullDataUpdated -= value; } }

        private event Action<Vessel> onOceanTypeChanged;
        public event Action<Vessel> OnOceanTypeChanged { add { onOceanTypeChanged += value; } remove { onOceanTypeChanged -= value; } }

        public Vessel(int vesselID, int ownerPlayerID, float locationX, float locationZ, float rotationEulerAngleY, OceanType locatedOceanType)
        {
            VesselID = vesselID;
            OwnerPlayerID = ownerPlayerID;
            LocationX = locationX;
            LocationZ = locationZ;
            RotationEulerAngleY = rotationEulerAngleY;
            LocatedOceanType = locatedOceanType;

            decorationDictionary = new Dictionary<int, Decoration>();
        }
        public void UpdateFullData(Vessel vessel)
        {
            VesselID = vessel.VesselID;
            OwnerPlayerID = vessel.OwnerPlayerID;
            LocationX = vessel.LocationX;
            LocationZ = vessel.LocationZ;
            RotationEulerAngleY = vessel.RotationEulerAngleY;
            LocatedOceanType = vessel.LocatedOceanType;

            onVesselFullDataUpdated?.Invoke(this);
        }
        public void UpdateTransform(float locationX, float locationZ, float rotationEulerAngleY, OceanType locatedOceanType)
        {
            LocationX = locationX;
            LocationZ = locationZ;
            RotationEulerAngleY = rotationEulerAngleY;
            LocatedOceanType = locatedOceanType;

            onVesselTransformUpdated?.Invoke(VesselID, LocationX, LocationZ, RotationEulerAngleY, LocatedOceanType);
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
