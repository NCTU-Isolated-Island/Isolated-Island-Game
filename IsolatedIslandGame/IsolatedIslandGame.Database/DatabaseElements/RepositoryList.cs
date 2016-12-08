using IsolatedIslandGame.Database.DatabaseElements.Repositories;

namespace IsolatedIslandGame.Database.DatabaseElements
{
    public abstract class RepositoryList
    {
        public abstract PlayerRepository PlayerRepository { get; }
        public abstract ItemRepository ItemRepository { get; }
        public abstract InventoryRepository InventoryRepository { get; }
        public abstract InventoryItemInfoRepository InventoryItemInfoRepository { get; }
        public abstract VesselRepository VesselRepository { get; }
        public abstract DecorationRepository DecorationRepository { get; }
    }
}
