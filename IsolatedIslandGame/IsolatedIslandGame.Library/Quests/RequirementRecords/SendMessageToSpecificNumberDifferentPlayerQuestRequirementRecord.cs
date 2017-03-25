using IsolatedIslandGame.Library.Quests.Requirements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class SendMessageToSpecificNumberDifferentPlayerQuestRequirementRecord : QuestRequirementRecord
    {
        private HashSet<int> playerID_Set = new HashSet<int>();
        public IEnumerable<int> PlayerIDs { get { return playerID_Set.ToArray(); } }
        public override bool IsSufficient
        {
            get
            {
                return playerID_Set.Count >= (Requirement as SendMessageToSpecificNumberDifferentPlayerQuestRequirement).SpecificNumber;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                return $"人數： {playerID_Set.Count}/{(Requirement as SendMessageToSpecificNumberDifferentPlayerQuestRequirement).SpecificNumber}";
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        public SendMessageToSpecificNumberDifferentPlayerQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement, HashSet<int> playerID_Set) : base(questRequirementRecordID, requirement)
        {
            this.playerID_Set = playerID_Set;
        }
        internal override void RegisterObserverEvents(Player player)
        {
            player.OnGetPlayerConversation += (conversation) =>
            {
                if (!IsSufficient && conversation.message.senderPlayerID == player.PlayerID )
                {
                    lock (playerID_Set)
                    {
                        if (!playerID_Set.Contains(conversation.receiverPlayerID))
                        {
                            if (playerID_Set.Add(conversation.receiverPlayerID) && (QuestRecordFactory.Instance == null || QuestRecordFactory.Instance.AddPlayerIDToSendMessageToSpecificNumberDifferentPlayerQuestRequirementRecord(this, conversation.receiverPlayerID)))
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
