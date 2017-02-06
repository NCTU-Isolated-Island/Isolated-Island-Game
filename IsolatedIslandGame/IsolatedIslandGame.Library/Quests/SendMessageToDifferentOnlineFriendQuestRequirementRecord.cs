using System;
using System.Collections.Generic;
using System.Linq;

namespace IsolatedIslandGame.Library.Quests
{
    public class SendMessageToDifferentOnlineFriendQuestRequirementRecord : QuestRequirementRecord
    {
        private HashSet<int> onlineFriendPlayerID_Set = new HashSet<int>();
        public IEnumerable<int> OnlineFriendPlayerIDs { get { return onlineFriendPlayerID_Set.ToArray(); } }

        public override bool IsSufficient
        {
            get
            {
                return onlineFriendPlayerID_Set.Count >= (Requirement as SendMessageToDifferentOnlineFriendQuestRequirement).RequiredOnlinedFriendNumber;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                return $"人數： {onlineFriendPlayerID_Set.Count}/{(Requirement as SendMessageToDifferentOnlineFriendQuestRequirement).RequiredOnlinedFriendNumber}";
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        public SendMessageToDifferentOnlineFriendQuestRequirementRecord(int questRequirementRecordID, Player player, QuestRequirement requirement, HashSet<int> onlineFriendPlayerIDSet) : base(questRequirementRecordID, player, requirement)
        {
            this.onlineFriendPlayerID_Set = onlineFriendPlayerIDSet;
        }
        internal override void RegisterObserverEvents()
        {
            player.OnGetPlayerConversation += (conversation) =>
            {
                if (!IsSufficient && conversation.message.senderPlayerID == player.PlayerID && player.ContainsFriend(conversation.receiverPlayerID) && VesselManager.Instance.ContainsVesselWithOwnerPlayerID(conversation.receiverPlayerID))
                {
                    FriendInformation info;
                    if (player.FindFriend(conversation.receiverPlayerID, out info) && info.isConfirmed)
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
                }
            };
        }
    }
}
