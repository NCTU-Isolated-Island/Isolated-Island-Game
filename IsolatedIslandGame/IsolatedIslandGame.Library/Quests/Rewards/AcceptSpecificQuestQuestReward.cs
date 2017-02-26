using IsolatedIslandGame.Protocol;
using MsgPack.Serialization;

namespace IsolatedIslandGame.Library.Quests.Rewards
{
    public class AcceptSpecificQuestQuestReward : QuestReward
    {
        [MessagePackMember(1)]
        public int AcceptedQuesiID { get; private set; }

        public override QuestRewardType QuestRewardType { get { return QuestRewardType.AcceptSpecificQuest; } }
        public override string Description
        {
            get
            {
                return $"接取新任務";
            }
        }

        [MessagePackDeserializationConstructor]
        public AcceptSpecificQuestQuestReward() { }
        public AcceptSpecificQuestQuestReward(int questRewardID, int acceptedQuesiID) : base(questRewardID)
        {
            AcceptedQuesiID = acceptedQuesiID;
        }

        public override void GiveReward(Player player)
        {
            Quest quest;
            QuestRecord record;
            if(QuestManager.Instance.FindQuest(AcceptedQuesiID, out quest) && quest.CreateRecord(player.PlayerID, out record))
            {
                record.RegisterObserverEvents(player);
                player.AddQuestRecord(record);
                record.OnQuestRecordStatusChange += player.EventManager.SyncDataResolver.SyncQuestRecordUpdated;
            }
        }

        public override bool GiveRewardCheck(Player player)
        {
            return true;
        }
    }
}
