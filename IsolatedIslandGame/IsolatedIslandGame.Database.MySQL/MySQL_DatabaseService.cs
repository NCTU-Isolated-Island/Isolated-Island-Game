using IsolatedIslandGame.Database.DatabaseElements;
using IsolatedIslandGame.Database.MySQL.DatabaseElements;

namespace IsolatedIslandGame.Database.MySQL
{
    public class MySQL_DatabaseService : DatabaseService
    {
        private MySQL_ConnectionList internalConnectionList = new MySQL_ConnectionList();
        protected override ConnectionList InternalConnectionList { get { return internalConnectionList; } }

        private MySQL_RepositoryList internalRepositoryList = new MySQL_RepositoryList();
        protected override RepositoryList InternalRepositoryList { get { return internalRepositoryList; } }
    }
}
