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
        private Dictionary<int, MutiplayerSynthesizeParticipationInfo> mutiplayerSynthesizeParticipationInfoDictionary = new Dictionary<int, MutiplayerSynthesizeParticipationInfo>();
        public IEnumerable<MutiplayerSynthesizeParticipationInfo> MutiplayerSynthesizeParticipationInfos { get { return mutiplayerSynthesizeParticipationInfoDictionary.Values.ToArray(); } }

        public delegate void MutiplayerSynthesizeParticipationInfoChangeEventHandler(DataChangeType changeType, MutiplayerSynthesizeParticipationInfo info);
        private event MutiplayerSynthesizeParticipationInfoChangeEventHandler onMutiplayerSynthesizeParticipationInfoChange;
        public event MutiplayerSynthesizeParticipationInfoChangeEventHandler OnMutiplayerSynthesizeParticipationInfoChange { add { onMutiplayerSynthesizeParticipationInfoChange += value; } remove { onMutiplayerSynthesizeParticipationInfoChange -= value; } }

        public bool ContainsMutiplayerSynthesizeParticipant(int playerID)
        {
            return mutiplayerSynthesizeParticipationInfoDictionary.ContainsKey(playerID);
        }
        public bool FindMutiplayerSynthesizeParticipationInfo(int playerID, out MutiplayerSynthesizeParticipationInfo info)
        {
            return mutiplayerSynthesizeParticipationInfoDictionary.TryGetValue(playerID, out info);
        }
        public void LoadMutiplayerSynthesizeParticipationInfo(MutiplayerSynthesizeParticipationInfo info)
        {
            if (ContainsMutiplayerSynthesizeParticipant(info.playerID))
            {
                mutiplayerSynthesizeParticipationInfoDictionary[info.playerID] = info;
                onMutiplayerSynthesizeParticipationInfoChange?.Invoke(DataChangeType.Update, info);
            }
            else
            {
                mutiplayerSynthesizeParticipationInfoDictionary.Add(info.playerID, info);
                onMutiplayerSynthesizeParticipationInfoChange?.Invoke(DataChangeType.Add, info);
            }
        }
        public void RemoveMutiplayerSynthesizeParticipant(int playerID)
        {
            if (ContainsMutiplayerSynthesizeParticipant(playerID))
            {
                MutiplayerSynthesizeParticipationInfo info = mutiplayerSynthesizeParticipationInfoDictionary[playerID];
                mutiplayerSynthesizeParticipationInfoDictionary.Remove(playerID);
                onMutiplayerSynthesizeParticipationInfoChange?.Invoke(DataChangeType.Remove, info);
            }
        }
    }
}
