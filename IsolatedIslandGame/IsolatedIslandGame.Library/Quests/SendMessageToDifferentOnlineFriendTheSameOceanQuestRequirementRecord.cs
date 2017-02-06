using System;
using System.Collections.Generic;
using System.Linq;
using MsgPack.Serialization;

namespace IsolatedIslandGame.Library.Quests
{
    public class SendMessageToDifferentOnlineFriendTheSameOceanQuestRequirementRecord : QuestRequirementRecord
    {
        [MessagePackMember(3)]
        private HashSet<int> onlineFriendPlayerID_Set = new HashSet<int>();
        public IEnumerable<int> OnlineFriendPlayerIDs { get { return onlineFriendPlayerID_Set.ToArray(); } }
        public override bool IsSufficient
        {
            get
            {
                return onlineFriendPlayerID_Set.Count >= (Requirement as SendMessageToDifferentOnlineFriendInTheSameOceanQuestRequirement).RequiredFriendNumber;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                return $"人數： {onlineFriendPlayerID_Set.Count}/{(Requirement as SendMessageToDifferentOnlineFriendInTheSameOceanQuestRequirement).RequiredFriendNumber}";
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        [MessagePackDeserializationConstructor]
        public SendMessageToDifferentOnlineFriendTheSameOceanQuestRequirementRecord() { }
        public SendMessageToDifferentOnlineFriendTheSameOceanQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement, HashSet<int> onlineFriendPlayerID_Set) : base(questRequirementRecordID, requirement)
        {
            this.onlineFriendPlayerID_Set = onlineFriendPlayerID_Set;
        }
        internal override void RegisterObserverEvents(Player player)
        {
            player.OnGetPlayerConversation += (conversation) =>
            {
                FriendInformation info;
                Vessel friendVessel;
                if (!IsSufficient && 
                    conversation.message.senderPlayerID == player.PlayerID && 
                    player.FindFriend(conversation.receiverPlayerID, out info) && 
                    info.isConfirmed && 
                    VesselManager.Instance.FindVesselByOwnerPlayerID(conversation.receiverPlayerID, out friendVessel) && 
                    player.Vessel.LocatedOceanType == friendVessel.LocatedOceanType)
                {
                    lock (onlineFriendPlayerID_Set)
                    {
                        if (!onlineFriendPlayerID_Set.Contains(conversation.receiverPlayerID))
                        {
                            if (onlineFriendPlayerID_Set.Add(conversation.receiverPlayerID) && (QuestRecordFactory.Instance == null || QuestRecordFactory.Instance.AddPlayerIDToSendMessageToDifferentOnlineFriendQuestRequirementRecord(QuestRequirementRecordID, conversation.receiverPlayerID)))
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
