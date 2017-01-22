using IsolatedIslandGame.Database.Repositories;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;
using MySql.Data.MySqlClient;

namespace IsolatedIslandGame.Database.MySQL.Repositories
{
    class MySQL_TransactionItemInfoRepository : TransactionItemInfoRepository
    {
        public override void Save(int transactionID, int playerID, TransactionItemInfo info)
        {
            string sqlString = @"INSERT INTO TransactionItemInfoCollection 
                (TransactionID,PlayerID,ItemID,ItemCount,PositionIndex) VALUES (@transactionID,@playerID,@itemID,@itemCount,@positionIndex) ;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.ArchiveDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("transactionID", transactionID);
                command.Parameters.AddWithValue("playerID", playerID);
                command.Parameters.AddWithValue("itemID", info.Item.ItemID);
                command.Parameters.AddWithValue("itemCount", info.Count);
                command.Parameters.AddWithValue("positionIndex", info.PositionIndex);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (command.ExecuteNonQuery() <= 0)
                    {
                        LogService.Error($"MySQL_TransactionItemInfoRepository Save TransactionItemInfo Error TransactionID: {transactionID}, PlayerID: {playerID}, ItemID: {info.Item.ItemID}, ItemCount: {info.Count}, PositionIndex: {info.PositionIndex}");
                    }
                }
            }
        }
    }
}
