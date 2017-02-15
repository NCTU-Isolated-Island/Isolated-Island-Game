using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Managers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Managers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Managers;
using IsolatedIslandGame.Protocol;
using System.Collections.Generic;
using System.Linq;

namespace IsolatedIslandGame.Library.Landmarks
{
    public class Landmark : IIdentityProvidable
    {
        public int LandmarkID { get; private set; }
        public string LandmarkName { get; private set; }

        private Dictionary<int, LandmarkRoom> roomDictionary = new Dictionary<int, LandmarkRoom>();
        public IEnumerable<LandmarkRoom> Rooms { get { return roomDictionary.Values.ToArray(); } }

        public string IdentityInformation
        {
            get
            {
                return $"Landmark ID: {LandmarkID}, Name: {LandmarkName}";
            }
        }

        public LandmarkEventManager EventManager { get; private set; }
        public LandmarkOperationManager OperationManager { get; private set; }
        public LandmarkResponseManager ResponseManager { get; private set; }

        public delegate void LandmarkRoomChangeEventHandler(DataChangeType changeType, LandmarkRoom room);
        private event LandmarkRoomChangeEventHandler onLandmarkRoomChange;
        public event LandmarkRoomChangeEventHandler OnLandmarkRoomChange { add { onLandmarkRoomChange += value; } remove { onLandmarkRoomChange -= value; } }

        public Landmark(int landmarkID, string landmarkName)
        {
            LandmarkID = landmarkID;
            LandmarkName = landmarkName;

            EventManager = new LandmarkEventManager(this);
            OperationManager = new LandmarkOperationManager(this);
            ResponseManager = new LandmarkResponseManager(this);
        }
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
                room.BindLandmark(this);
                onLandmarkRoomChange?.Invoke(DataChangeType.Update, room);
            }
            else
            {
                roomDictionary.Add(room.LandmarkRoomID, room);
                room.BindLandmark(this);
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
