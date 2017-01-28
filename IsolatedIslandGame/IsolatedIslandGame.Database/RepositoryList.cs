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
        #endregion

        #region setting data
        public abstract ItemRepository ItemRepository { get; }
        public abstract BlueprintRepository BlueprintRepository { get; }
        public abstract BlueprintRequirementRepository BlueprintRequirementRepository { get; }
        public abstract BlueprintProductRepository BlueprintProductRepository { get; }
        #endregion

        #region text data
        public abstract PlayerMessageRepository PlayerMessageRepository { get; }
        #endregion

        #region archive data
        public abstract TransactionRepository TransactionRepository { get; }
        public abstract TransactionItemInfoRepository TransactionItemInfoRepository { get; }
        public abstract IslandMaterialRepository IslandMaterialRepository { get; }
        #endregion
    }
}
