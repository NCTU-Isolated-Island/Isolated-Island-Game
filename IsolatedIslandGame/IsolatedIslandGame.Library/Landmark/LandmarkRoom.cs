using IsolatedIslandGame.Protocol;
using System.Collections.Generic;
using System.Linq;

namespace IsolatedIslandGame.Library.Landmark
{
    public class LandmarkRoom
    {
        public int LandmarkRoomID { get; private set; }
        public string RoomName { get; private set; }
        public int HostPlayerID { get; private set; }
        private Dictionary<int, MutiplayerSynthesizeParticipantInfo> mutiplayerSynthesizeParticipantInfoDictionary = new Dictionary<int, MutiplayerSynthesizeParticipantInfo>();
        public IEnumerable<MutiplayerSynthesizeParticipantInfo> MutiplayerSynthesizeParticipationInfos { get { return mutiplayerSynthesizeParticipantInfoDictionary.Values.ToArray(); } }

        public delegate void MutiplayerSynthesizeParticipantInfoChangeEventHandler(DataChangeType changeType, MutiplayerSynthesizeParticipantInfo info);
        private event MutiplayerSynthesizeParticipantInfoChangeEventHandler onMutiplayerSynthesizeParticipantInfoChange;
        public event MutiplayerSynthesizeParticipantInfoChangeEventHandler OnMutiplayerSynthesizeParticipantInfoChange { add { onMutiplayerSynthesizeParticipantInfoChange += value; } remove { onMutiplayerSynthesizeParticipantInfoChange -= value; } }

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
