using System;
using System.Collections.Generic;
using System.Linq;

namespace IsolatedIslandGame.Library.Quests
{
    public class SendMessageToDifferentOnlineFriendQuestRequirementRecord : QuestRequirementRecord
    {
        private HashSet<int> onlineFriendPlayerIDSet = new HashSet<int>();
        public IEnumerable<int> OnlineFriendPlayerIDs { get { return onlineFriendPlayerIDSet.ToArray(); } }

        public override bool IsSufficient
        {
            get
            {
                return onlineFriendPlayerIDSet.Count >= (Requirement as SendMessageToDifferentOnlineFriendQuestRequirement).RequiredOnlinedFriendNumber;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                return $"人數： {onlineFriendPlayerIDSet.Count}/{(Requirement as SendMessageToDifferentOnlineFriendQuestRequirement).RequiredOnlinedFriendNumber}";
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        public SendMessageToDifferentOnlineFriendQuestRequirementRecord(Player player, QuestRequirement requirement, HashSet<int> onlineFriendPlayerIDSet) : base(player, requirement)
        {
            this.onlineFriendPlayerIDSet = onlineFriendPlayerIDSet;

            player.OnGetPlayerConversation += (conversation) =>
            {
                if(!IsSufficient && conversation.message.senderPlayerID == player.PlayerID && player.ContainsFriend(conversation.receiverPlayerID) && VesselManager.Instance.ContainsVesselWithOwnerPlayerID(conversation.receiverPlayerID))
                {
                    FriendInformation info;
                    if(player.FindFriend(conversation.receiverPlayerID, out info) && info.isConfirmed)
                    {
                        if (onlineFriendPlayerIDSet.Add(conversation.receiverPlayerID))
                        {
                            onRequirementStatusChange?.Invoke(this);
                        }
                    }
                }
            };
        }
    }
}
