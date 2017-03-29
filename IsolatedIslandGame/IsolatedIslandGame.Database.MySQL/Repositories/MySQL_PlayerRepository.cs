using IsolatedIslandGame.Database.Repositories;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Protocol;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Net;

namespace IsolatedIslandGame.Database.MySQL.Repositories
{
    class MySQL_PlayerRepository : PlayerRepository
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
                    (FacebookID, RegisterDate) VALUES (@facebookID, @registerDate) ;
                    SELECT LAST_INSERT_ID();";
                lock(DatabaseService.ConnectionList.PlayerDataConnection)
                using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
                {
                    command.Parameters.AddWithValue("facebookID", facebookID);
                    command.Parameters.AddWithValue("registerDate", DateTime.Now);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            playerID = reader.GetInt32(0);
                        }
                        else
                        {
                            LogService.ErrorFormat("MySQLPlayerRepository Register Player no affected row from FacebookID: {0}", facebookID);
                            return false;
                        }
                    }
                }
            }
            Inventory inventory;
            return DatabaseService.RepositoryList.InventoryRepository.Create(playerID, Inventory.DefaultCapacity, out inventory);
        }

        public override bool Contains(ulong facebookID, out int playerID)
        {
            lock(DatabaseService.ConnectionList.PlayerDataConnection)
            using (MySqlCommand command = new MySqlCommand("SELECT PlayerID FROM PlayerCollection WHERE FacebookID = @facebookID;", DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("facebookID", facebookID);
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

        public override bool Read(int playerID, out Player player, out bool isTodayFirstLogin)
        {
            int cumulativeLoginCount;
            UpdateLastLoginTime(playerID, DateTime.Now, out isTodayFirstLogin, out cumulativeLoginCount);
            string sqlString = @"SELECT  
                FacebookID, Nickname, Signature, GroupType, LastConnectedIPAddress, NextDrawMaterialTime
                from PlayerCollection WHERE PlayerID = @playerID;";
            lock(DatabaseService.ConnectionList.PlayerDataConnection)
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("playerID", playerID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ulong facebookID = reader.GetUInt64(0);
                        string nickname = reader.IsDBNull(1) ? "" : reader.GetString(1);
                        string signature = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        GroupType groupType = (GroupType)reader.GetByte(3);
                        IPAddress lastConnectedIPAddress = reader.IsDBNull(4) ? IPAddress.None : IPAddress.Parse(reader.GetString(4));
                        DateTime nextDrawMaterialTime = reader.GetDateTime(5);

                        player = new Player(playerID, facebookID, nickname, signature, groupType, lastConnectedIPAddress, nextDrawMaterialTime, cumulativeLoginCount);
                        return true;
                    }
                    else
                    {
                        player = null;
                        return false;
                    }
                }
            }
        }

        public override void Update(Player player)
        {
            string sqlString = @"UPDATE PlayerCollection SET 
                Nickname = @nickname,
                Signature = @signature,
                GroupType = @groupType,
                LastConnectedIPAddress = @lastConnectedIPAddress,
                NextDrawMaterialTime = @nextDrawMaterialTime
                WHERE PlayerID = @playerID;";
            lock(DatabaseService.ConnectionList.PlayerDataConnection)
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("nickname", player.Nickname ?? "");
                command.Parameters.AddWithValue("signature", player.Signature ?? "");
                command.Parameters.AddWithValue("groupType", (byte)player.GroupType);
                command.Parameters.AddWithValue("lastConnectedIPAddress", player.LastConnectedIPAddress.ToString());
                command.Parameters.AddWithValue("nextDrawMaterialTime", player.NextDrawMaterialTime);
                command.Parameters.AddWithValue("playerID", player.PlayerID);
                
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.ErrorFormat("MySQLPlayerRepository Save Player no affected row from PlayerID:{0}, IPAddress:{1}", player.PlayerID, player.LastConnectedIPAddress);
                }
            }
            if(player.Inventory != null)
            {
                DatabaseService.RepositoryList.InventoryRepository.Update(player.Inventory);
            }
            if (player.Vessel != null)
            {
                DatabaseService.RepositoryList.VesselRepository.Update(player.Vessel);
            }
            var knownBlueprints = DatabaseService.RepositoryList.PlayerKnownBlueprintRepository.ListOfPlayer(player.PlayerID);
            foreach (var blueprint in knownBlueprints)
            {
                player.GetBlueprint(blueprint);
            }
        }

        public override bool ReadPlayerInformation(int playerID, out PlayerInformation playerInformation)
        {
            string sqlString = @"SELECT  
                Nickname, Signature, GroupType, VesselID
                from PlayerCollection, VesselCollection WHERE PlayerID = @playerID AND OwnerPlayerID = PlayerID;";
            lock(DatabaseService.ConnectionList.PlayerDataConnection)
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("playerID", playerID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string nickname = reader.IsDBNull(0) ? null : reader.GetString(0);
                        string signature = reader.IsDBNull(1) ? null : reader.GetString(1);
                        GroupType groupType = (GroupType)reader.GetByte(2);
                        int vesselID = reader.GetInt32(3);

                        playerInformation = new PlayerInformation
                        {
                            playerID = playerID,
                            nickname = nickname,
                            signature = signature,
                            groupType = groupType,
                            vesselID = vesselID
                        };
                        return true;
                    }
                    else
                    {
                        playerInformation = new PlayerInformation();
                        return false;
                    }
                }
            }
        }

        public override void GlobalUpdateNextDrawMaterialTime(DateTime nextDrawMaterialTime)
        {
            string sqlString = @"UPDATE PlayerCollection 
                SET NextDrawMaterialTime = @nextDrawMaterialTime
                WHERE true;";
            lock(DatabaseService.ConnectionList.PlayerDataConnection)
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("nextDrawMaterialTime", nextDrawMaterialTime);
                LogService.Info($"GlobalUpdateNextDrawMaterialTime to {command.ExecuteNonQuery()} players");
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.ErrorFormat("MySQLPlayerRepository GlobalUpdateNextDrawMaterialTime no affected row");
                }
            }
        }

        public override void UpdateLastLoginTime(int playerID, DateTime loginTime, out bool isTodayFirstLogin, out int cumulativeLoginCount)
        {
            DateTime lastLoginTime;
            int originCumulativeLoginCount;
            string sqlString = @"SELECT  
                LastLoginTime, CumulativeLoginCount
                from PlayerCollection WHERE PlayerID = @playerID;";
            lock(DatabaseService.ConnectionList.PlayerDataConnection)
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("playerID", playerID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        lastLoginTime = reader.GetDateTime(0);
                        originCumulativeLoginCount = reader.GetInt32(1);
                    }
                    else
                    {
                        isTodayFirstLogin = false;
                        cumulativeLoginCount = 0;
                        return;
                    }
                }
            }
            if(lastLoginTime.Day != loginTime.Day)
            {
                isTodayFirstLogin = true;
                cumulativeLoginCount = originCumulativeLoginCount + 1;
            }
            else
            {
                isTodayFirstLogin = false;
                cumulativeLoginCount = originCumulativeLoginCount;
            }

            sqlString = @"UPDATE PlayerCollection 
                SET LastLoginTime = @lastLoginTime, CumulativeLoginCount = @cumulativeLoginCount
                WHERE PlayerID = @playerID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("lastLoginTime", loginTime);
                command.Parameters.AddWithValue("cumulativeLoginCount", cumulativeLoginCount);
                command.Parameters.AddWithValue("playerID", playerID);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.ErrorFormat("MySQLPlayerRepository UpdateLastLoginTime no affected row, PlayerID: {0}", playerID);
                }
            }
        }

        public override List<int> ListAllPlayerID()
        {
            List<int> playerIDs = new List<int>();
            string sqlString = @"SELECT PlayerID from PlayerCollection ;";
            lock(DatabaseService.ConnectionList.PlayerDataConnection)
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int playerID = reader.GetInt32(0);
                        playerIDs.Add(playerID);
                    }
                }
            }
            return playerIDs;
        }
    }
}
