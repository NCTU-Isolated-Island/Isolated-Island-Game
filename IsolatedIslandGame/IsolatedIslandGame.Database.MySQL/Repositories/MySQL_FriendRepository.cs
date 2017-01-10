using IsolatedIslandGame.Database.Repositories;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Protocol;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.MySQL.Repositories
{
    class MySQL_FriendRepository : FriendRepository
    {
        public override void AddFriend(int senderPlayerID, int receiverPlayerID)
        {
            string sqlString = @"INSERT INTO FriendCollection 
                (SenderPlayerID,ReceiverPlayerID,IsConfirmed) VALUES (@senderPlayerID,@receiverPlayerID,@isConfirmed);";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@senderPlayerID", senderPlayerID);
                command.Parameters.AddWithValue("@receiverPlayerID", receiverPlayerID);
                command.Parameters.AddWithValue("@isConfirmed", false);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.Error($"MySQL_FriendRepository AddFriend Error SenderPlayerID: {senderPlayerID}, ReceiverPlayerID: {receiverPlayerID}");
                }
            }
        }

        public override void ConfirmFriend(int senderPlayerID, int receiverPlayerID)
        {
            string sqlString = @"UPDATE FriendCollection SET 
                IsConfirmed = @isConfirmed
                WHERE SenderPlayerID = @senderPlayerID AND ReceiverPlayerID = @receiverPlayerID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@isConfirmed", true);
                command.Parameters.AddWithValue("@senderPlayerID", senderPlayerID);
                command.Parameters.AddWithValue("@receiverPlayerID", receiverPlayerID);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.Error($"MySQL_FriendRepository ConfirmFriend Error SenderPlayerID: {senderPlayerID}, ReceiverPlayerID: {receiverPlayerID}");
                }
            }
        }

        public override void DeleteFriend(int senderPlayerID, int receiverPlayerID)
        {
            string sqlString = @"DELETE FROM FriendCollection 
                WHERE (SenderPlayerID = @senderPlayerID AND ReceiverPlayerID = @receiverPlayerID) OR (SenderPlayerID = @receiverPlayerID AND ReceiverPlayerID = @senderPlayerID);";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@senderPlayerID", senderPlayerID);
                command.Parameters.AddWithValue("@receiverPlayerID", receiverPlayerID);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.Error($"MySQL_FriendRepository DeleteFriend Error SenderPlayerID: {senderPlayerID}, ReceiverPlayerID: {receiverPlayerID}");
                }
            }
        }

        public override List<FriendInformation> ListOfFriendInformations(int playerID)
        {
            List<FriendInformation> friendInformations = new List<FriendInformation>();
            string sqlString = @"SELECT ReceiverPlayerID, IsConfirmed, Signature, Nickname, GroupType, VesselID 
                from FriendCollection, PlayerCollection, Vesselcollection
                WHERE SenderPlayerID = @playerID AND ReceiverPlayerID = PlayerID AND ReceiverPlayerID = OwnerPlayerID;";
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
                            isSender = true,
                            isConfirmed = isConfirmed
                        });
                    }
                }
            }
            sqlString = @"SELECT SenderPlayerID, IsConfirmed, Signature, Nickname, GroupType, VesselID 
                from FriendCollection, PlayerCollection, Vesselcollection
                WHERE ReceiverPlayerID = @playerID AND SenderPlayerID = PlayerID AND SenderPlayerID = OwnerPlayerID;";
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
                            isSender = false,
                            isConfirmed = isConfirmed
                        });
                    }
                }
            }
            return friendInformations;
        }
    }
}
