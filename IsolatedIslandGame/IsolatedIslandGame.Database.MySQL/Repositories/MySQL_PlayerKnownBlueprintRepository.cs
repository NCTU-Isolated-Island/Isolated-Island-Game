using IsolatedIslandGame.Database.Repositories;
using IsolatedIslandGame.Library;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.MySQL.Repositories
{
    class MySQL_PlayerKnownBlueprintRepository : PlayerKnownBlueprintRepository
    {
        public override void AddRelation(int playerID, int blueprintID)
        {
            string sqlString = @"INSERT INTO PlayerKnownBlueprintCollection 
                (PlayerID,BlueprintID) VALUES (@playerID,@blueprintID);";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@playerID", playerID);
                command.Parameters.AddWithValue("@blueprintID", blueprintID);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.ErrorFormat("MySQL_PlayerKnownBlueprintRepository AddRelation PlayerKnownBlueprint Error PlayerID: {0}, BlueprintID: {1}", playerID, blueprintID);
                }
            }
        }

        public override List<Blueprint> ListOfPlayer(int playerID)
        {
            List<Blueprint> blueprints = new List<Blueprint>();
            string sqlString = @"SELECT  
                BlueprintID
                from PlayerKnownBlueprintCollection 
                WHERE PlayerID = @playerID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@playerID", playerID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int blueprintID = reader.GetInt32(0);
                        if(BlueprintManager.Instance.ContainsBlueprint(blueprintID))
                        {
                            blueprints.Add(BlueprintManager.Instance.FindBlueprint(blueprintID));
                        }
                    }
                }
            }
            return blueprints;
        }
    }
}
