using IsolatedIslandGame.Database;
using IsolatedIslandGame.Database.Repositories;
using IsolatedIslandGame.Database.MySQL.Repositories;
using System;

namespace IsolatedIslandGame.Database.MySQL
{
    class MySQL_RepositoryList : RepositoryList
    {
        private MySQL_PlayerRepository playerRepository = new MySQL_PlayerRepository();
        public override PlayerRepository PlayerRepository { get { return playerRepository; } }

        private MySQL_ItemRepository itemRepository = new MySQL_ItemRepository();
        public override ItemRepository ItemRepository { get { return itemRepository; } }

        private MySQL_InventoryRepository inventoryRepository = new MySQL_InventoryRepository();
        public override InventoryRepository InventoryRepository { get { return inventoryRepository; } }

        private MySQL_InventoryItemInfoRepository inventoryItemInfoRepository = new MySQL_InventoryItemInfoRepository();
        public override InventoryItemInfoRepository InventoryItemInfoRepository { get { return inventoryItemInfoRepository; } }

        private MySQL_VesselRepository vesselRepository = new MySQL_VesselRepository();
        public override VesselRepository VesselRepository { get { return vesselRepository; } }

        private MySQL_DecorationRepository decorationRepository = new MySQL_DecorationRepository();
        public override DecorationRepository DecorationRepository { get { return decorationRepository; } }

        private MySQL_BlueprintRepository blueprintRepository = new MySQL_BlueprintRepository();
        public override BlueprintRepository BlueprintRepository { get { return blueprintRepository; } }

        private MySQL_BlueprintRequirementRepository blueprintRequirementRepository = new MySQL_BlueprintRequirementRepository();
        public override BlueprintRequirementRepository BlueprintRequirementRepository { get { return blueprintRequirementRepository; } }

        private MySQL_BlueprintProductRepository blueprintProductRepository = new MySQL_BlueprintProductRepository();
        public override BlueprintProductRepository BlueprintProductRepository { get { return blueprintProductRepository; } }

        private MySQL_PlayerKnownBlueprintRepository playerKnownBlueprintRepository = new MySQL_PlayerKnownBlueprintRepository();
        public override PlayerKnownBlueprintRepository PlayerKnownBlueprintRepository { get { return playerKnownBlueprintRepository; } }

        private MySQL_FriendRepository friendRepository = new MySQL_FriendRepository();
        public override FriendRepository FriendRepository { get { return friendRepository; } }

        private MySQL_PlayerMessageRepository playerMessageRepository = new MySQL_PlayerMessageRepository();
        public override PlayerMessageRepository PlayerMessageRepository { get { return playerMessageRepository; } }

        private MySQL_PlayerConversationRepository playerConversationRepository = new MySQL_PlayerConversationRepository();
        public override PlayerConversationRepository PlayerConversationRepository { get { return playerConversationRepository; } }
    }
}
