using IsolatedIslandGame.Database.DatabaseElements;
using IsolatedIslandGame.Database.MySQL.DatabaseElements;

namespace IsolatedIslandGame.Database.MySQL
{
    public class MySQLDatabaseService : DatabaseService
    {
        private MySQLConnectionList internalConnectionList = new MySQLConnectionList();
        protected override ConnectionList InternalConnectionList { get { return internalConnectionList; } }

        private MySQLRepositoryList internalRepositoryList = new MySQLRepositoryList();
        protected override RepositoryList InternalRepositoryList { get { return internalRepositoryList} }
    }
}
