using IsolatedIslandGame.Database.DatabaseElements.Repositories;
using IsolatedIslandGame.Database.DatabaseFormatData;
using IsolatedIslandGame.Library;
using MySql.Data.MySqlClient;
using System;
using System.Net;

namespace IsolatedIslandGame.Database.MySQL.DatabaseElements.Repositories
{
    class MySQLPlayerRepository : PlayerRepository
    {
        public override bool Register(ulong facebookID)
        {
            int playerID;
            if (Contains(facebookID, out playerID))
            {
                return false;
            }
            else
            {
                string sqlString = @"INSERT INTO PlayerCollection 
                        (FacebookID, RegisterDate) VALUES (@facebookID, @registerDate) ;";
                using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
                {
                    command.Parameters.AddWithValue("@facebookID", facebookID);
                    command.Parameters.AddWithValue("@registerDate", DateTime.Now);
                    if (command.ExecuteNonQuery() <= 0)
                    {
                        LogService.ErrorFormat("MySQLPlayerRepository Register Player no affected row from FacebookID: {0}", facebookID);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }

        public override bool Contains(ulong facebookID, out int playerID)
        {
            using (MySqlCommand command = new MySqlCommand("SELECT PlayerID FROM PlayerCollection WHERE FacebookID = @facebookID;", DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@facebookID", facebookID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        playerID = reader.GetInt32(0);
                        return true;
                    }
                    else
                    {
                        playerID = -1;
                        return false;
                    }
                }
            }
        }

        public override PlayerData Find(int playerID)
        {
            string sqlString = @"SELECT  
                FacebookID, Nickname, LastConnectedIPAddress
                from PlayerCollection WHERE PlayerID = @playerID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@playerID", playerID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ulong facebookID = reader.GetUInt64(0);
                        string nickname = reader.IsDBNull(1) ? null : reader.GetString(1);
                        IPAddress lastConnectedIPAddress = reader.IsDBNull(2) ? IPAddress.None : IPAddress.Parse(reader.GetString(2));
                        return new PlayerData { playerID = playerID, facebookID = facebookID, nickname = nickname, lastConnectedIPAddress = lastConnectedIPAddress};
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public override void Save(Player player)
        {
            string sqlString = @"UPDATE PlayerCollection SET 
                Nickname = @nickname,
                LastConnectedIPAddress = @lastConnectedIPAddress
                WHERE PlayerID = @playerID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@nickname", player.Nickname ?? "");
                command.Parameters.AddWithValue("@lastConnectedIPAddress", player.LastConnectedIPAddress.ToString());
                command.Parameters.AddWithValue("@playerID", player.PlayerID);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.ErrorFormat("MySQLPlayerRepository Save Player no affected row from PlayerID:{0}, IPAddress:{1}", player.PlayerID, player.LastConnectedIPAddress);
                }
            }
        }
    }
}
