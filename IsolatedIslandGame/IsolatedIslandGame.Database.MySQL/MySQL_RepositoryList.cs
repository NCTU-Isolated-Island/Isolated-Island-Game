using IsolatedIslandGame.Database.MySQL.Repositories;
using IsolatedIslandGame.Database.Repositories;

namespace IsolatedIslandGame.Database.MySQL
{
    class MySQL_RepositoryList : RepositoryList
    {
        #region player data
        private MySQL_PlayerRepository playerRepository = new MySQL_PlayerRepository();
        public override PlayerRepository PlayerRepository { get { return playerRepository; } }

        private MySQL_InventoryRepository inventoryRepository = new MySQL_InventoryRepository();
        public override InventoryRepository InventoryRepository { get { return inventoryRepository; } }

        private MySQL_InventoryItemInfoRepository inventoryItemInfoRepository = new MySQL_InventoryItemInfoRepository();
        public override InventoryItemInfoRepository InventoryItemInfoRepository { get { return inventoryItemInfoRepository; } }

        private MySQL_VesselRepository vesselRepository = new MySQL_VesselRepository();
        public override VesselRepository VesselRepository { get { return vesselRepository; } }

        private MySQL_DecorationRepository decorationRepository = new MySQL_DecorationRepository();
        public override DecorationRepository DecorationRepository { get { return decorationRepository; } }

        private MySQL_PlayerKnownBlueprintRepository playerKnownBlueprintRepository = new MySQL_PlayerKnownBlueprintRepository();
        public override PlayerKnownBlueprintRepository PlayerKnownBlueprintRepository { get { return playerKnownBlueprintRepository; } }

        private MySQL_FriendRepository friendRepository = new MySQL_FriendRepository();
        public override FriendRepository FriendRepository { get { return friendRepository; } }

        private MySQL_PlayerConversationRepository playerConversationRepository = new MySQL_PlayerConversationRepository();
        public override PlayerConversationRepository PlayerConversationRepository { get { return playerConversationRepository; } }

        private MySQL_QuestRecordRepository questRecordRepository = new MySQL_QuestRecordRepository();
        public override QuestRecordRepository QuestRecordRepository { get { return questRecordRepository; } }
        #endregion

        #region setting data
        private MySQL_ItemRepository itemRepository = new MySQL_ItemRepository();
        public override ItemRepository ItemRepository { get { return itemRepository; } }

        private MySQL_BlueprintRepository blueprintRepository = new MySQL_BlueprintRepository();
        public override BlueprintRepository BlueprintRepository { get { return blueprintRepository; } }

        private MySQL_QuestRepository questRepository = new MySQL_QuestRepository();
        public override QuestRepository QuestRepository { get { return questRepository; } }

        private MySQL_LandmarkRepository landmarkRepository = new MySQL_LandmarkRepository();
        public override LandmarkRepository LandmarkRepository { get { return landmarkRepository; } }
        #endregion

        #region text data
        private MySQL_PlayerMessageRepository playerMessageRepository = new MySQL_PlayerMessageRepository();
        public override PlayerMessageRepository PlayerMessageRepository { get { return playerMessageRepository; } }

        private MySQL_WorldChannelMessageRepository worldChannelMessageRepository = new MySQL_WorldChannelMessageRepository();
        public override WorldChannelMessageRepository WorldChannelMessageRepository { get { return worldChannelMessageRepository; } }
        #endregion

        #region archive data
        private MySQL_TransactionRepository transactionRepository = new MySQL_TransactionRepository();
        public override TransactionRepository TransactionRepository { get { return transactionRepository; } }

        private MySQL_TransactionItemInfoRepository transactionItemInfoRepository = new MySQL_TransactionItemInfoRepository();
        public override TransactionItemInfoRepository TransactionItemInfoRepository { get { return transactionItemInfoRepository; } }

        private MySQL_IslandMaterialRepository islandMaterialRepository = new MySQL_IslandMaterialRepository();
        public override IslandMaterialRepository IslandMaterialRepository { get { return islandMaterialRepository; } }
        #endregion

        #region system data
        private MySQL_ItemEntityRepository itemEntityRepository = new MySQL_ItemEntityRepository();
        public override ItemEntityRepository ItemEntityRepository { get { return itemEntityRepository; } }
        #endregion
    }
}
