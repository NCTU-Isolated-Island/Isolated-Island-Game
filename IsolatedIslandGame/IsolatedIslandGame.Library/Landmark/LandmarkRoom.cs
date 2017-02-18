using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Managers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Managers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Managers;
using IsolatedIslandGame.Protocol;
using System.Collections.Generic;
using System.Linq;
using System;

namespace IsolatedIslandGame.Library.Landmarks
{
    public class LandmarkRoom : IIdentityProvidable
    {
        public int LandmarkRoomID { get; private set; }
        public string RoomName { get; private set; }
        public int HostPlayerID { get; private set; }
        public Landmark Landmark { get; private set; }
        private Dictionary<int, MultiplayerSynthesizeParticipantInfo> mutiplayerSynthesizeParticipantInfoDictionary = new Dictionary<int, MultiplayerSynthesizeParticipantInfo>();
        public IEnumerable<MultiplayerSynthesizeParticipantInfo> MutiplayerSynthesizeParticipationInfos { get { return mutiplayerSynthesizeParticipantInfoDictionary.Values.ToArray(); } }
        public bool CanStartMultiplayerSynthesize { get { return MutiplayerSynthesizeParticipationInfos.All(x => x.isChecked || x.participantPlayerID == HostPlayerID); } }


        public LandmarkRoomEventManager EventManager { get; private set; }
        public LandmarkRoomOperationManager OperationManager { get; private set; }
        public LandmarkRoomResponseManager ResponseManager { get; private set; }

        public string IdentityInformation
        {
            get
            {
                return $"LandmarkRoom ID: {LandmarkRoomID}, Name: {RoomName}, HostPlayerID: {HostPlayerID}, Under Landmark: {Landmark.IdentityInformation}";
            }
        }

        public delegate void MutiplayerSynthesizeParticipantInfoChangeEventHandler(DataChangeType changeType, MultiplayerSynthesizeParticipantInfo info);
        private event MutiplayerSynthesizeParticipantInfoChangeEventHandler onMutiplayerSynthesizeParticipantInfoChange;
        public event MutiplayerSynthesizeParticipantInfoChangeEventHandler OnMutiplayerSynthesizeParticipantInfoChange { add { onMutiplayerSynthesizeParticipantInfoChange += value; } remove { onMutiplayerSynthesizeParticipantInfoChange -= value; } }

        private event Action<LandmarkRoom> onStartMultiplayerSynthesize;
        public event Action<LandmarkRoom> OnStartMultiplayerSynthesize { add { onStartMultiplayerSynthesize += value; } remove { onStartMultiplayerSynthesize -= value; } }

        public LandmarkRoom(int landmarkRoomID, string roomName, int hostPlayerID)
        {
            LandmarkRoomID = landmarkRoomID;
            RoomName = roomName;
            HostPlayerID = hostPlayerID;

            EventManager = new LandmarkRoomEventManager(this);
            OperationManager = new LandmarkRoomOperationManager(this);
            ResponseManager = new LandmarkRoomResponseManager(this);
        }
        public void BindLandmark(Landmark landmark)
        {
            Landmark = landmark;
        }
        public bool ContainsMutiplayerSynthesizeParticipant(int playerID)
        {
            return mutiplayerSynthesizeParticipantInfoDictionary.ContainsKey(playerID);
        }
        public bool FindMutiplayerSynthesizeParticipantInfo(int playerID, out MultiplayerSynthesizeParticipantInfo info)
        {
            return mutiplayerSynthesizeParticipantInfoDictionary.TryGetValue(playerID, out info);
        }
        public void LoadMutiplayerSynthesizeParticipantInfo(MultiplayerSynthesizeParticipantInfo info)
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
                MultiplayerSynthesizeParticipantInfo info = mutiplayerSynthesizeParticipantInfoDictionary[playerID];
                mutiplayerSynthesizeParticipantInfoDictionary.Remove(playerID);
                onMutiplayerSynthesizeParticipantInfoChange?.Invoke(DataChangeType.Remove, info);
            }
        }
        public void StartMultiplayerSynthesize()
        {
            if(CanStartMultiplayerSynthesize)
            {
                onStartMultiplayerSynthesize?.Invoke(this);
            }
        }
    }
}
