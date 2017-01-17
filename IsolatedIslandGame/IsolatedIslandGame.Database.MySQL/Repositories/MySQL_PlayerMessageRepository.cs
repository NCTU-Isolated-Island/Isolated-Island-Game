using IsolatedIslandGame.Database.Repositories;
using IsolatedIslandGame.Library.TextData;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.MySQL.Repositories
{
    class MySQL_PlayerMessageRepository : PlayerMessageRepository
    {
        public override bool Create(int senderPlayerID, string content, out PlayerMessage message)
        {
            string sqlString = @"INSERT INTO PlayerMessageCollection 
                (SenderPlayerID,SendTime,Content) VALUES (@senderPlayerID,@sendTime,@content) ;
                SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.TextDataConnection.Connection as MySqlConnection))
            {
                DateTime now = DateTime.Now;
                command.Parameters.AddWithValue("senderPlayerID", senderPlayerID);
                command.Parameters.AddWithValue("sendTime", now);
                command.Parameters.AddWithValue("content", content);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int playerMessageID = reader.GetInt32(0);
                        message = new PlayerMessage
                        {
                            playerMessageID = playerMessageID,
                            senderPlayerID = senderPlayerID,
                            sendTime = now,
                            content = content
                        };
                        return true;
                    }
                    else
                    {
                        message = default(PlayerMessage);
                        return false;
                    }
                }
            }
        }

        public override bool Read(int playerMessageID, out PlayerMessage message)
        {
            string sqlString = @"SELECT  
                SenderPlayerID, SendTime, Content
                from PlayerMessageCollection WHERE PlayerMessageID = @playerMessageID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.TextDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@playerMessageID", playerMessageID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int senderPlayerID = reader.GetInt32(0);
                        DateTime sendTime = reader.GetDateTime(1);
                        string content = reader.GetString(2);

                        message = new PlayerMessage
                        {
                            playerMessageID = playerMessageID,
                            senderPlayerID = senderPlayerID,
                            sendTime = sendTime,
                            content = content
                        };
                        return true;
                    }
                    else
                    {
                        message = default(PlayerMessage);
                        return false;
                    }
                }
            }
        }

        public override List<PlayerMessage> ListOfSender(int senderPlayerID)
        {
            List<PlayerMessage> messages = new List<PlayerMessage>();
            string sqlString = @"SELECT  
                PlayerMessageID, SendTime, Content
                from PlayerMessageCollection 
                WHERE SenderPlayerID = @senderPlayerID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.TextDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@senderPlayerID", senderPlayerID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int playerMessageID = reader.GetInt32(0);
                        DateTime sendTime = reader.GetDateTime(1);
                        string content = reader.GetString(2);

                        messages.Add(new PlayerMessage
                        {
                            playerMessageID = playerMessageID,
                            senderPlayerID = senderPlayerID,
                            sendTime = sendTime,
                            content = content
                        });
                    }
                }
            }
            return messages;
        }
    }
}
