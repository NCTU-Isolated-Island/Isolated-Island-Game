using IsolatedIslandGame.Database.Repositories;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.TextData;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.MySQL.Repositories
{
    class MySQL_PlayerConversationRepository : PlayerConversationRepository
    {
        public override bool Create(int receiverPlayerID, int playerMessageID, bool hasRead, out PlayerConversation conversation)
        {
            string sqlString = @"INSERT INTO PlayerConversationCollection 
                (ReceiverPlayerID,MessageID,HasRead) VALUES (@receiverPlayerID,@playerMessageID,@hasRead) ;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("receiverPlayerID", receiverPlayerID);
                command.Parameters.AddWithValue("playerMessageID", playerMessageID);
                command.Parameters.AddWithValue("hasRead", hasRead);

                PlayerMessage message;
                if (command.ExecuteNonQuery() > 0 && DatabaseService.RepositoryList.PlayerMessageRepository.Read(playerMessageID, out message))
                {
                    conversation = new PlayerConversation
                    {
                        message = message,
                        receiverPlayerID = receiverPlayerID,
                        hasRead = hasRead
                    };
                    return true;
                }
                else
                {
                    LogService.Error($"MySQL_PlayerConversationRepository Create Error ReceiverPlayerID: {receiverPlayerID}, PlayerMessageID: {playerMessageID}");
                    conversation = new PlayerConversation();
                    return false;
                }
            }
        }
        public override bool Read(int receiverPlayerID, int playerMessageID, out PlayerConversation conversation)
        {
            string sqlString = @"SELECT  
                MessageID, HasRead, SenderPlayerID, SendTime, Content
                from IsolatedIsland_PlayerData.PlayerConversationCollection, IsolatedIsland_TextData.PlayerMessageCollection 
                WHERE ReceiverPlayerID = @receiverPlayerID AND MessageID = @playerMessageID AND MessageID = PlayerMessageID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("receiverPlayerID", receiverPlayerID);
                command.Parameters.AddWithValue("playerMessageID", playerMessageID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int messageID = reader.GetInt32(0);
                        bool hasRead = reader.GetBoolean(1);
                        int senderPlayerID = reader.GetInt32(2);
                        DateTime sendTime = reader.GetDateTime(3);
                        string content = reader.GetString(4);

                        conversation = new PlayerConversation
                        {
                            message = new PlayerMessage
                            {
                                playerMessageID = messageID,
                                senderPlayerID = senderPlayerID,
                                sendTime = sendTime,
                                content = content
                            },
                            receiverPlayerID = receiverPlayerID,
                            hasRead = hasRead
                        };
                        return true;
                    }
                    else
                    {
                        conversation = new PlayerConversation();
                        return false;
                    }
                }
            }
        }
        public override List<PlayerConversation> ListOfReceiver(int receiverPlayerID)
        {
            List<PlayerConversation> conversations = new List<PlayerConversation>();
            string sqlString = @"SELECT  
                MessageID, HasRead, SenderPlayerID, SendTime, Content
                from IsolatedIsland_PlayerData.PlayerConversationCollection, IsolatedIsland_TextData.PlayerMessageCollection 
                WHERE ReceiverPlayerID = @receiverPlayerID AND MessageID = PlayerMessageID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("receiverPlayerID", receiverPlayerID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int messageID = reader.GetInt32(0);
                        bool hasRead = reader.GetBoolean(1);
                        int senderPlayerID = reader.GetInt32(2);
                        DateTime sendTime = reader.GetDateTime(3);
                        string content = reader.GetString(4);

                        conversations.Add(new PlayerConversation
                        {
                            message = new PlayerMessage
                            {
                                playerMessageID = messageID,
                                senderPlayerID = senderPlayerID,
                                sendTime = sendTime,
                                content = content
                            },
                            receiverPlayerID = receiverPlayerID,
                            hasRead = hasRead
                        });
                    }
                }
            }
            return conversations;
        }

        public override bool SetPlayerMessageRead(int playerID, int playerMessageID)
        {
            string sqlString = @"UPDATE PlayerConversationCollection SET HasRead  = true
                WHERE ReceiverPlayerID = @playerID AND MessageID = @playerMessageID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("playerID", playerID);
                command.Parameters.AddWithValue("playerMessageID", playerMessageID);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.Error($"MySQL_PlayerConversationRepository SetPlayerMessageRead Error PlayerID: {playerID}, PlayerMessageID: {playerMessageID}");
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
