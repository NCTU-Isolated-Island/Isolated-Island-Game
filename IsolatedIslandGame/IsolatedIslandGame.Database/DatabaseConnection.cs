using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace IsolatedIslandGame.Database
{
    public abstract class DatabaseConnection : IDisposable
    {
        protected DbConnection connection;
        public DbConnection Connection
        {
            get
            {
                connection?.Close();
                connection?.Open();
                return connection;
            }
        }
        protected List<DatabaseConnection> childConnections;
        public bool Connected
        {
            get
            {
                if (connection == null)
                    return childConnections.All(x => x.Connected);
                else
                    return Connection.State == ConnectionState.Open && childConnections.All(x => x.Connected);
            }
        }
        protected abstract string DatabaseName { get; }

        protected DatabaseConnection()
        {
            childConnections = new List<DatabaseConnection>();
        }
        public abstract bool Connect(string hostName, string userName, string password, string database);
        public void Dispose()
        {
            connection?.Dispose();
            childConnections.ForEach(x => x.Dispose());
        }
    }
}
