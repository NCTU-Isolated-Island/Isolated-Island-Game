using IsolatedIslandGame.Database.Repositories;
using IsolatedIslandGame.Library;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.MySQL.Repositories
{
    class MySQL_FriendRepository : FriendRepository
    {
        public override void AddFriend(int selfPlayerID, int friendPlayerID)
        {
            string sqlString = @"INSERT INTO FriendCollection 
                (SelfPlayerID,FriendPlayerID) VALUES (@selfPlayerID,@friendPlayerID);";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@selfPlayerID", selfPlayerID);
                command.Parameters.AddWithValue("@friendPlayerID", friendPlayerID);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.ErrorFormat("MySQL_FriendRepository AddFriend Error SelfPlayerID: {0}, FriendPlayerID: {1}", selfPlayerID, friendPlayerID);
                }
            }
        }

        public override void DeleteFriend(int selfPlayerID, int friendPlayerID)
        {
            string sqlString = @"DELETE FROM FriendCollection 
                WHERE SelfPlayerID = @selfPlayerID AND FriendPlayerID = @friendPlayerID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@selfPlayerID", selfPlayerID);
                command.Parameters.AddWithValue("@friendPlayerID", friendPlayerID);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.ErrorFormat("MySQL_FriendRepository DeleteFriend Error SelfPlayerID: {0}, FriendPlayerID: {1}", selfPlayerID, friendPlayerID);
                }
            }
        }

        public override List<int> ListOfFriendPlayerIDs(int selfPlayerID)
        {
            List<int> friendPlayerIDs = new List<int>();
            string sqlString = @"SELECT FriendPlayerID from FriendCollection 
                WHERE SelfPlayerID = @selfPlayerID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@selfPlayerID", selfPlayerID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int friendPlayerID = reader.GetInt32(0);
                        friendPlayerIDs.Add(friendPlayerID);
                    }
                }
            }
            return friendPlayerIDs;
        }
    }
}
