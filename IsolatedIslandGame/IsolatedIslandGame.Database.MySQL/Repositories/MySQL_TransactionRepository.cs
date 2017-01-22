using IsolatedIslandGame.Database.Repositories;
using IsolatedIslandGame.Library;
using MySql.Data.MySqlClient;

namespace IsolatedIslandGame.Database.MySQL.Repositories
{
    class MySQL_TransactionRepository : TransactionRepository
    {
        public override bool Create(int requesterPlayerID, int accepterPlayerID, out Transaction transaction)
        {
            string sqlString = @"INSERT INTO TransactionCollection 
                (RequesterPlayerID,AccepterPlayerID) VALUES (@requesterPlayerID,@accepterPlayerID) ;
                SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.ArchiveDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("requesterPlayerID", requesterPlayerID);
                command.Parameters.AddWithValue("accepterPlayerID", accepterPlayerID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int transactionID = reader.GetInt32(0);
                        transaction = new Transaction(transactionID, requesterPlayerID, accepterPlayerID);
                        return true;
                    }
                    else
                    {
                        transaction = null;
                        return false;
                    }
                }
            }
        }

        public override void Save(Transaction transaction)
        {
            foreach(var info in transaction.RequesterTransactionItemInfos)
            {
                DatabaseService.RepositoryList.TransactionItemInfoRepository.Save(transaction.TransactionID, transaction.RequesterPlayerID, info);
            }
            foreach (var info in transaction.AccepterTransactionItemInfos)
            {
                DatabaseService.RepositoryList.TransactionItemInfoRepository.Save(transaction.TransactionID, transaction.AccepterPlayerID, info);
            }
        }
    }
}
