using IsolatedIslandGame.Protocol;
using System.Collections.Generic;
using System.Linq;

namespace IsolatedIslandGame.Library.Landmark
{
    public class Landmark
    {
        public int LandmarkID { get; private set; }
        public string LandmarkName { get; private set; }

        private Dictionary<int, LandmarkRoom> roomDictionary = new Dictionary<int, LandmarkRoom>();
        public IEnumerable<LandmarkRoom> Rooms { get { return roomDictionary.Values.ToArray(); } }

        public delegate void LandmarkRoomChangeEventHandler(DataChangeType changeType, LandmarkRoom room);
        private event LandmarkRoomChangeEventHandler onLandmarkRoomChange;
        public event LandmarkRoomChangeEventHandler OnLandmarkRoomChange { add { onLandmarkRoomChange += value; } remove { onLandmarkRoomChange -= value; } }

        public bool ContainsRoom(int roomID)
        {
            return roomDictionary.ContainsKey(roomID);
        }
        public bool FindRoom(int roomID, out LandmarkRoom room)
        {
            return roomDictionary.TryGetValue(roomID, out room);
        }
        public void LoadRoom(LandmarkRoom room)
        {
            if(ContainsRoom(room.LandmarkRoomID))
            {
                roomDictionary[room.LandmarkRoomID] = room;
                onLandmarkRoomChange?.Invoke(DataChangeType.Update, room);
            }
            else
            {
                roomDictionary.Add(room.LandmarkRoomID, room);
                onLandmarkRoomChange?.Invoke(DataChangeType.Add, room);
            }
        }
        public void RemoveRoom(int roomID)
        {
            if(ContainsRoom(roomID))
            {
                LandmarkRoom room = roomDictionary[roomID];
                roomDictionary.Remove(roomID);
                onLandmarkRoomChange?.Invoke(DataChangeType.Remove, room);
            }
        }
    }
}
