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
        public string OwnerName { get; private set; }
        public float LocationX { get; private set; }
        public float LocationZ { get; private set; }
        public Quaternion Roration { get; private set; }

        private Dictionary<int, Decoration> decorationDictionary;
        public int DecorationCount { get { return decorationDictionary.Count; } }
        public IEnumerable<Decoration> Decorations { get { return decorationDictionary.Values; } }

        public delegate void DecorationChangeEventHandler(int vesselID, Decoration decoration, DataChangeType changeType);
        private event DecorationChangeEventHandler onDecorationChange;
        public event DecorationChangeEventHandler OnDecorationChange { add { onDecorationChange += value; } remove { onDecorationChange -= value; } }

        public Vessel(int vesselID, int ownerPlayerID, string ownerName, float locationX, float locationZ, Quaternion roration)
        {
            VesselID = vesselID;
            OwnerPlayerID = ownerPlayerID;
            OwnerName = ownerName;
            LocationX = locationX;
            LocationZ = locationZ;
            Roration = roration;

            decorationDictionary = new Dictionary<int, Decoration>();
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
