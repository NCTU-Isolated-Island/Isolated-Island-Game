using IsolatedIslandGame.Database.DatabaseElements.Repositories;

namespace IsolatedIslandGame.Database.DatabaseElements
{
    public abstract class RepositoryList
    {
        public abstract PlayerRepository PlayerRepository { get; }
    }
}
