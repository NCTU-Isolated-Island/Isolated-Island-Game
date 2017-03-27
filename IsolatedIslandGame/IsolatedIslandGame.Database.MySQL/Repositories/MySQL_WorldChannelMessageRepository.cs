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
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.TextDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("playerMessageID", playerMessageID);

                PlayerMessage message;
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read() && DatabaseService.RepositoryList.PlayerMessageRepository.Read(playerMessageID, out message))
                    {
                        int worldMessageID = reader.GetInt32(0);
                        worldMessage = new WorldChannelMessage(worldMessageID, message);
                        return true;
                    }
                    else
                    {
                        worldMessage = null;
                        return false;
                    }
                }
            }
        }

        public override List<WorldChannelMessage> ListLatestN_Message(int n)
        {
            throw new NotImplementedException();
        }
    }
}
