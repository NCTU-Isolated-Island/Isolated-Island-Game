using IsolatedIslandGame.Database.DatabaseElements;
using IsolatedIslandGame.Database.DatabaseElements.Repositories;
using IsolatedIslandGame.Database.MySQL.DatabaseElements.Repositories;

namespace IsolatedIslandGame.Database.MySQL.DatabaseElements
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
    }
}
