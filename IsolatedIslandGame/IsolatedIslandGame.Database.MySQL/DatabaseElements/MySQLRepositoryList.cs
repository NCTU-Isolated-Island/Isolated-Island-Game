using IsolatedIslandGame.Database.DatabaseElements;
using IsolatedIslandGame.Database.DatabaseElements.Repositories;
using IsolatedIslandGame.Database.MySQL.DatabaseElements.Repositories;

namespace IsolatedIslandGame.Database.MySQL.DatabaseElements
{
    class MySQLRepositoryList : RepositoryList
    {
        private MySQLPlayerRepository playerRepository = new MySQLPlayerRepository();
        public override PlayerRepository PlayerRepository { get { return playerRepository; } }
    }
}
