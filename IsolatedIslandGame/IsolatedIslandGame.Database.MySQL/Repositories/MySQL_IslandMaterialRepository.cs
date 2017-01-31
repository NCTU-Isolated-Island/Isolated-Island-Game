using IsolatedIslandGame.Database.Repositories;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Protocol;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.MySQL.Repositories
{
    class MySQL_IslandMaterialRepository : IslandMaterialRepository
    {
        public override int ReadTotalScore(GroupType groupType)
        {
            string sqlString = @"SELECT SUM(Score) FROM
                (SELECT Score, GroupType FROM IsolatedIsland_ArchiveData.IslandMaterialCollection, IsolatedIsland_SettingData.MaterialCollection, IsolatedIsland_PlayerData.PlayerCollection 
                WHERE SenderPlayerID = PlayerID AND MaterialItemID = ItemID) as ScoreTable
                WHERE GroupType = @groupType GROUP BY GroupType;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.ArchiveDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("groupType", (byte)groupType);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int totalScore = reader.GetInt32(0);
                        return totalScore;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }
        public override List<Island.PlayerMaterialInfo> ListTodayMaterialRanking()
        {
            List<Island.PlayerMaterialInfo> infos = new List<Island.PlayerMaterialInfo>();
            string sqlString = @"SELECT SenderPlayerID, MaterialItemID 
                FROM IslandMaterialCollection 
                WHERE DATE(DATE_SUB(SendTime, INTERVAL 6 HOUR)) = DATE(DATE_SUB(CURRENT_TIMESTAMP, INTERVAL 6 HOUR));";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.ArchiveDataConnection.Connection as MySqlConnection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int senderPlayerID = reader.GetInt32(0);
                        int materialItemID = reader.GetInt32(1);

                        infos.Add(new Island.PlayerMaterialInfo { playerID = senderPlayerID, materialItemID = materialItemID });
                    }
                }
            }
            return infos;
        }
        public override List<Island.PlayerScoreInfo> ListPlayerScoreRanking()
        {
            List<Island.PlayerScoreInfo> infos = new List<Island.PlayerScoreInfo>();
            string sqlString = @"SELECT SenderPlayerID, SUM(Score) 
                FROM IsolatedIsland_ArchiveData.IslandMaterialCollection, IsolatedIsland_SettingData.MaterialCollection, IsolatedIsland_PlayerData.PlayerCollection 
                WHERE SenderPlayerID = PlayerID AND MaterialItemID = ItemID GROUP BY SenderPlayerID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.ArchiveDataConnection.Connection as MySqlConnection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int senderPlayerID = reader.GetInt32(0);
                        int totalScore = reader.GetInt32(1);

                        infos.Add(new Island.PlayerScoreInfo { playerID = senderPlayerID, score = totalScore });
                    }
                }
            }
            return infos;
        }

        public override void Save(Island.PlayerMaterialInfo info)
        {
            string sqlString = @"INSERT INTO IslandMaterialCollection 
                (SenderPlayerID,MaterialItemID) VALUES (@senderPlayerID,@materialItemID) ;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.ArchiveDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("senderPlayerID", info.playerID);
                command.Parameters.AddWithValue("materialItemID", info.materialItemID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (command.ExecuteNonQuery() <= 0)
                    {
                        LogService.Error($"MySQL_IslandMaterialRepository Save Error SenderPlayerID: {info.playerID}, MaterialItemID: {info.materialItemID}");
                    }
                }
            }
        }
    }
}
