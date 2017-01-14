using IsolatedIslandGame.Database.Repositories;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Protocol;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.MySQL.Repositories
{
    class MySQL_FriendRepository : FriendRepository
    {
        public override void AddFriend(int inviterPlayerID, int accepterPlayerID)
        {
            string sqlString = @"INSERT INTO FriendCollection 
                (InviterPlayerID,AccepterPlayerID,IsConfirmed) VALUES (@inviterPlayerID,@accepterPlayerID,@isConfirmed);";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@inviterPlayerID", inviterPlayerID);
                command.Parameters.AddWithValue("@accepterPlayerID", accepterPlayerID);
                command.Parameters.AddWithValue("@isConfirmed", false);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.Error($"MySQL_FriendRepository AddFriend Error InviterPlayerID: {inviterPlayerID}, AccepterPlayerID: {accepterPlayerID}");
                }
            }
        }

        public override void ConfirmFriend(int inviterPlayerID, int accepterPlayerID)
        {
            string sqlString = @"UPDATE FriendCollection SET 
                IsConfirmed = @isConfirmed
                WHERE InviterPlayerID = @inviterPlayerID AND AccepterPlayerID = @accepterPlayerID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@isConfirmed", true);
                command.Parameters.AddWithValue("@inviterPlayerID", inviterPlayerID);
                command.Parameters.AddWithValue("@accepterPlayerID", accepterPlayerID);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.Error($"MySQL_FriendRepository ConfirmFriend Error InviterPlayerID: {inviterPlayerID}, AccepterPlayerID: {accepterPlayerID}");
                }
            }
        }

        public override void DeleteFriend(int inviterPlayerID, int accepterPlayerID)
        {
            string sqlString = @"DELETE FROM FriendCollection 
                WHERE (InviterPlayerID = @inviterPlayerID AND AccepterPlayerID = @accepterPlayerID) OR (InviterPlayerID = @accepterPlayerID AND AccepterPlayerID = @inviterPlayerID);";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@inviterPlayerID", inviterPlayerID);
                command.Parameters.AddWithValue("@accepterPlayerID", accepterPlayerID);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.Error($"MySQL_FriendRepository DeleteFriend Error InviterPlayerID: {inviterPlayerID}, AccepterPlayerID: {accepterPlayerID}");
                }
            }
        }

        public override List<FriendInformation> ListOfFriendInformations(int playerID)
        {
            List<FriendInformation> friendInformations = new List<FriendInformation>();
            string sqlString = @"SELECT AccepterPlayerID, IsConfirmed, Signature, Nickname, GroupType, VesselID 
                from FriendCollection, PlayerCollection, Vesselcollection
                WHERE InviterPlayerID = @playerID AND AccepterPlayerID = PlayerID AND AccepterPlayerID = OwnerPlayerID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@playerID", playerID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int friendPlayerID = reader.GetInt32(0);
                        bool isConfirmed = reader.GetBoolean(1);
                        string signature = reader.GetString(2);
                        string nickname = reader.GetString(3);
                        GroupType groupType = (GroupType)reader.GetByte(4);
                        int vesselID = reader.GetInt32(5);
                        
                        friendInformations.Add(new FriendInformation
                        {
                            playerInformation = new PlayerInformation
                            {
                                playerID = friendPlayerID,
                                nickname = nickname,
                                signature = signature,
                                groupType = groupType,
                                vesselID = vesselID
                            },
                            isInviter = true,
                            isConfirmed = isConfirmed
                        });
                    }
                }
            }
            sqlString = @"SELECT InviterPlayerID, IsConfirmed, Signature, Nickname, GroupType, VesselID 
                from FriendCollection, PlayerCollection, Vesselcollection
                WHERE AccepterPlayerID = @playerID AND InviterPlayerID = PlayerID AND InviterPlayerID = OwnerPlayerID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@playerID", playerID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int friendPlayerID = reader.GetInt32(0);
                        bool isConfirmed = reader.GetBoolean(1);
                        string signature = reader.GetString(2);
                        string nickname = reader.GetString(3);
                        GroupType groupType = (GroupType)reader.GetByte(4);
                        int vesselID = reader.GetInt32(5);

                        friendInformations.Add(new FriendInformation
                        {
                            playerInformation = new PlayerInformation
                            {
                                playerID = friendPlayerID,
                                nickname = nickname,
                                signature = signature,
                                groupType = groupType,
                                vesselID = vesselID
                            },
                            isInviter = false,
                            isConfirmed = isConfirmed
                        });
                    }
                }
            }
            return friendInformations;
        }
    }
}
