using IsolatedIslandGame.Database.Repositories;
using IsolatedIslandGame.Library.TextData;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.MySQL.Repositories
{
    class MySQL_WorldChannelMessageRepository : WorldChannelMessageRepository
    {
        public override bool Create(int playerMessageID, out WorldChannelMessage worldMessage)
        {
            string sqlString = @"INSERT INTO WorldChannelMessageCollection 
                (PlayerMessageID) VALUES (@playerMessageID) ;
                SELECT LAST_INSERT_ID();";
            int worldMessageID;
            lock (DatabaseService.ConnectionList.TextDataConnection)
            {
                using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.TextDataConnection.Connection as MySqlConnection))
                {
                    command.Parameters.AddWithValue("playerMessageID", playerMessageID);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            worldMessageID = reader.GetInt32(0);
                        }
                        else
                        {
                            worldMessage = null;
                            return false;
                        }
                    }
                }
            }
            PlayerMessage message;
            if (DatabaseService.RepositoryList.PlayerMessageRepository.Read(playerMessageID, out message))
            {
                worldMessage = new WorldChannelMessage(worldMessageID, message);
                return true;
            }
            else
            {
                worldMessage = null;
                return false;
            }
        }

        public override List<WorldChannelMessage> ListLatestN_Message(int n)
        {
            List<WorldChannelMessage> worldMessages = new List<WorldChannelMessage>();
            string sqlString = @"SELECT  
                WorldChannelMessageID, PlayerMessageCollection.PlayerMessageID, SenderPlayerID, SendTime, Content
                from WorldChannelMessageCollection, PlayerMessageCollection 
                WHERE WorldChannelMessageCollection.PlayerMessageID = PlayerMessageCollection.PlayerMessageID
                LIMIT @n;";
            lock (DatabaseService.ConnectionList.PlayerDataConnection)
                using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.TextDataConnection.Connection as MySqlConnection))
                {
                    command.Parameters.AddWithValue("n", n);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int worldChannelMessageID = reader.GetInt32(0);
                            int playerMessageID = reader.GetInt32(1);
                            int senderPlayerID = reader.GetInt32(2);
                            DateTime sendTime = reader.GetDateTime(3);
                            string content = reader.GetString(4);

                            worldMessages.Add(new WorldChannelMessage(worldChannelMessageID, new PlayerMessage
                            {
                                playerMessageID = playerMessageID,
                                senderPlayerID = senderPlayerID,
                                sendTime = sendTime,
                                content = content
                            }));
                        }
                    }
                }
            return worldMessages;
        }
    }
}
