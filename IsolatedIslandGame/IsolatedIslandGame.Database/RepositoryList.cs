using IsolatedIslandGame.Database.Repositories;

namespace IsolatedIslandGame.Database
{
    public abstract class RepositoryList
    {
        #region player data
        public abstract PlayerRepository PlayerRepository { get; }
        public abstract InventoryRepository InventoryRepository { get; }
        public abstract InventoryItemInfoRepository InventoryItemInfoRepository { get; }
        public abstract VesselRepository VesselRepository { get; }
        public abstract DecorationRepository DecorationRepository { get; }
        public abstract PlayerKnownBlueprintRepository PlayerKnownBlueprintRepository { get; }
        public abstract FriendRepository FriendRepository { get; }
        public abstract PlayerConversationRepository PlayerConversationRepository { get; }
        public abstract QuestRecordRepository QuestRecordRepository { get; }
        #endregion

        #region setting data
        public abstract ItemRepository ItemRepository { get; }
        public abstract BlueprintRepository BlueprintRepository { get; }
        public abstract QuestRepository QuestRepository { get; }
        public abstract LandmarkRepository LandmarkRepository { get; }
        #endregion

        #region text data
        public abstract PlayerMessageRepository PlayerMessageRepository { get; }
        public abstract WorldChannelMessageRepository WorldChannelMessageRepository { get; }
        #endregion

        #region archive data
        public abstract TransactionRepository TransactionRepository { get; }
        public abstract TransactionItemInfoRepository TransactionItemInfoRepository { get; }
        public abstract IslandMaterialRepository IslandMaterialRepository { get; }
        #endregion

        #region system data
        public abstract ItemEntityRepository ItemEntityRepository { get; }
        public abstract ItemEntityGeneratorRepository ItemEntityGeneratorRepository { get; }
        #endregion
    }
}
