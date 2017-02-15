using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Managers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Managers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Managers;
using IsolatedIslandGame.Protocol;
using System.Collections.Generic;
using System.Linq;

namespace IsolatedIslandGame.Library.Landmarks
{
    public class LandmarkRoom
    {
        public int LandmarkRoomID { get; private set; }
        public string RoomName { get; private set; }
        public int HostPlayerID { get; private set; }
        private Dictionary<int, MutiplayerSynthesizeParticipantInfo> mutiplayerSynthesizeParticipantInfoDictionary = new Dictionary<int, MutiplayerSynthesizeParticipantInfo>();
        public IEnumerable<MutiplayerSynthesizeParticipantInfo> MutiplayerSynthesizeParticipationInfos { get { return mutiplayerSynthesizeParticipantInfoDictionary.Values.ToArray(); } }

        public LandmarkRoomEventManager EventManager { get; private set; }
        public LandmarkRoomOperationManager OperationManager { get; private set; }
        public LandmarkRoomResponseManager ResponseManager { get; private set; }

        public delegate void MutiplayerSynthesizeParticipantInfoChangeEventHandler(DataChangeType changeType, MutiplayerSynthesizeParticipantInfo info);
        private event MutiplayerSynthesizeParticipantInfoChangeEventHandler onMutiplayerSynthesizeParticipantInfoChange;
        public event MutiplayerSynthesizeParticipantInfoChangeEventHandler OnMutiplayerSynthesizeParticipantInfoChange { add { onMutiplayerSynthesizeParticipantInfoChange += value; } remove { onMutiplayerSynthesizeParticipantInfoChange -= value; } }

        public LandmarkRoom(int landmarkRoomID, string roomName, int hostPlayerID)
        {
            LandmarkRoomID = landmarkRoomID;
            RoomName = roomName;
            HostPlayerID = hostPlayerID;

            EventManager = new LandmarkRoomEventManager(this);
            OperationManager = new LandmarkRoomOperationManager(this);
            ResponseManager = new LandmarRoomkResponseManager(this);
        }

        public bool ContainsMutiplayerSynthesizeParticipant(int playerID)
        {
            return mutiplayerSynthesizeParticipantInfoDictionary.ContainsKey(playerID);
        }
        public bool FindMutiplayerSynthesizeParticipantInfo(int playerID, out MutiplayerSynthesizeParticipantInfo info)
        {
            return mutiplayerSynthesizeParticipantInfoDictionary.TryGetValue(playerID, out info);
        }
        public void LoadMutiplayerSynthesizeParticipantInfo(MutiplayerSynthesizeParticipantInfo info)
        {
            if (ContainsMutiplayerSynthesizeParticipant(info.participantPlayerID))
            {
                mutiplayerSynthesizeParticipantInfoDictionary[info.participantPlayerID] = info;
                onMutiplayerSynthesizeParticipantInfoChange?.Invoke(DataChangeType.Update, info);
            }
            else
            {
                mutiplayerSynthesizeParticipantInfoDictionary.Add(info.participantPlayerID, info);
                onMutiplayerSynthesizeParticipantInfoChange?.Invoke(DataChangeType.Add, info);
            }
        }
        public void RemoveMutiplayerSynthesizeParticipant(int playerID)
        {
            if (ContainsMutiplayerSynthesizeParticipant(playerID))
            {
                MutiplayerSynthesizeParticipantInfo info = mutiplayerSynthesizeParticipantInfoDictionary[playerID];
                mutiplayerSynthesizeParticipantInfoDictionary.Remove(playerID);
                onMutiplayerSynthesizeParticipantInfoChange?.Invoke(DataChangeType.Remove, info);
            }
        }
    }
}
