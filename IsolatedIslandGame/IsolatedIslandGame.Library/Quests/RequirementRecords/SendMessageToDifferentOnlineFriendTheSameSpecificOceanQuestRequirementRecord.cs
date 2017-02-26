using IsolatedIslandGame.Library.Quests.Requirements;
using IsolatedIslandGame.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class SendMessageToDifferentOnlineFriendTheSameSpecificOceanQuestRequirementRecord : QuestRequirementRecord
    {
        private HashSet<int> friendPlayerID_Set = new HashSet<int>();
        public IEnumerable<int> FriendPlayerIDs { get { return friendPlayerID_Set.ToArray(); } }
        public override bool IsSufficient
        {
            get
            {
                return friendPlayerID_Set.Count >= (Requirement as SendMessageToDifferentOnlineFriendInTheSameSpecificOceanQuestRequirement).RequiredFriendNumber;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                return $"人數： {friendPlayerID_Set.Count}/{(Requirement as SendMessageToDifferentOnlineFriendInTheSameSpecificOceanQuestRequirement).RequiredFriendNumber}";
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        public SendMessageToDifferentOnlineFriendTheSameSpecificOceanQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement, HashSet<int> friendPlayerID_Set) : base(questRequirementRecordID, requirement)
        {
            this.friendPlayerID_Set = friendPlayerID_Set;
        }
        internal override void RegisterObserverEvents(Player player)
        {
            player.OnGetPlayerConversation += (conversation) =>
            {
                FriendInformation info;
                Vessel friendVessel;
                OceanType specificOceanType = (Requirement as SendMessageToDifferentOnlineFriendInTheSameSpecificOceanQuestRequirement).SpecificOceanType;
                if (!IsSufficient && 
                    conversation.message.senderPlayerID == player.PlayerID && 
                    player.FindFriend(conversation.receiverPlayerID, out info) && 
                    info.isConfirmed && 
                    VesselManager.Instance.FindVesselByOwnerPlayerID(conversation.receiverPlayerID, out friendVessel) && 
                    player.Vessel.LocatedOceanType == specificOceanType &&
                    friendVessel.LocatedOceanType == specificOceanType)
                {
                    lock (friendPlayerID_Set)
                    {
                        if (!friendPlayerID_Set.Contains(conversation.receiverPlayerID))
                        {
                            if (friendPlayerID_Set.Add(conversation.receiverPlayerID) && (QuestRecordFactory.Instance == null || QuestRecordFactory.Instance.AddPlayerIDToSendMessageToDifferentOnlineFriendInTheSameOceanQuestRequirementRecord(QuestRequirementRecordID, conversation.receiverPlayerID)))
                            {
                                onRequirementStatusChange?.Invoke(this);
                            }
                        }
                    }
                }
            };
        }
    }
}
