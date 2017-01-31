using IsolatedIslandGame.Database.Repositories;
using IsolatedIslandGame.Library;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.MySQL.Repositories
{
    class MySQL_FriendRepository : FriendRepository
    {
        public override bool AddFriend(int inviterPlayerID, int accepterPlayerID, out FriendInformation friendInformation)
        {
            string sqlString = @"INSERT INTO FriendCollection 
                (InviterPlayerID,AccepterPlayerID,IsConfirmed) VALUES (@inviterPlayerID,@accepterPlayerID,@isConfirmed);";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("inviterPlayerID", inviterPlayerID);
                command.Parameters.AddWithValue("accepterPlayerID", accepterPlayerID);
                command.Parameters.AddWithValue("isConfirmed", false);
                if (command.ExecuteNonQuery() > 0)
                {
                    friendInformation = new FriendInformation
                    {
                        friendPlayerID = accepterPlayerID,
                        isInviter = false,
                        isConfirmed = false
                    };
                    return true;
                }
                else
                {
                    LogService.Error($"MySQL_FriendRepository AddFriend Error InviterPlayerID: {inviterPlayerID}, AccepterPlayerID: {accepterPlayerID}");
                    friendInformation = new FriendInformation();
                    return false;
                }
            }
        }

        public override bool ConfirmFriend(int inviterPlayerID, int accepterPlayerID, out FriendInformation friendInformation)
        {
            string sqlString = @"UPDATE FriendCollection SET 
                IsConfirmed = @isConfirmed
                WHERE InviterPlayerID = @inviterPlayerID AND AccepterPlayerID = @accepterPlayerID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("isConfirmed", true);
                command.Parameters.AddWithValue("inviterPlayerID", inviterPlayerID);
                command.Parameters.AddWithValue("accepterPlayerID", accepterPlayerID);
                if (command.ExecuteNonQuery() > 0)
                {
                    friendInformation = new FriendInformation
                    {
                        friendPlayerID = inviterPlayerID,
                        isInviter = true,
                        isConfirmed = true
                    };
                    return true;
                }
                else
                {
                    LogService.Error($"MySQL_FriendRepository ConfirmFriend Error InviterPlayerID: {inviterPlayerID}, AccepterPlayerID: {accepterPlayerID}");
                    friendInformation = new FriendInformation();
                    return false;
                }
            }
        }

        public override void DeleteFriend(int selfPlayerID, int targetPlayerID)
        {
            string sqlString = @"DELETE FROM FriendCollection 
                WHERE (InviterPlayerID = @selfPlayerID AND AccepterPlayerID = @targetPlayerID) OR (InviterPlayerID = @targetPlayerID AND AccepterPlayerID = @selfPlayerID);";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("selfPlayerID", selfPlayerID);
                command.Parameters.AddWithValue("targetPlayerID", targetPlayerID);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.Error($"MySQL_FriendRepository DeleteFriend Error SelfPlayerID: {selfPlayerID}, TargetPlayerID: {targetPlayerID}");
                }
            }
        }

        public override List<FriendInformation> ListOfFriendInformations(int playerID)
        {
            List<FriendInformation> friendInformations = new List<FriendInformation>();
            string sqlString = @"SELECT AccepterPlayerID, IsConfirmed from FriendCollection
                WHERE InviterPlayerID = @playerID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("playerID", playerID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int friendPlayerID = reader.GetInt32(0);
                        bool isConfirmed = reader.GetBoolean(1);
                        
                        friendInformations.Add(new FriendInformation
                        {
                            friendPlayerID = friendPlayerID,
                            isInviter = false,
                            isConfirmed = isConfirmed
                        });
                    }
                }
            }
            sqlString = @"SELECT InviterPlayerID, IsConfirmed from FriendCollection
                WHERE AccepterPlayerID = @playerID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("playerID", playerID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int friendPlayerID = reader.GetInt32(0);
                        bool isConfirmed = reader.GetBoolean(1);

                        friendInformations.Add(new FriendInformation
                        {
                            friendPlayerID = friendPlayerID,
                            isInviter = true,
                            isConfirmed = isConfirmed
                        });
                    }
                }
            }
            return friendInformations;
        }
    }
}
