using IsolatedIslandGame.Database.Repositories;
using IsolatedIslandGame.Library;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.MySQL.Repositories
{
    class MySQL_BlueprintRepository : BlueprintRepository
    {
        public override Blueprint Create(Blueprint.ElementInfo[] requirements, Blueprint.ElementInfo[] products)
        {
            int blueprintID = 0;
            string sqlString = @"INSERT INTO BlueprintCollection 
                (BlueprintID) VALUES (NULL) ;
                SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        blueprintID = reader.GetInt32(0);
                    }
                }
            }
            for(int i = 0; i < requirements.Length; i++)
            {
                DatabaseService.RepositoryList.BlueprintRequirementRepository.Create(blueprintID, requirements[i]);
            }
            for (int i = 0; i < products.Length; i++)
            {
                DatabaseService.RepositoryList.BlueprintRequirementRepository.Create(blueprintID, products[i]);
            }
            return new Blueprint(blueprintID, requirements, products);
        }
        public override void Delete(int blueprintID)
        {
            string sqlString = @"DELETE FROM BlueprintCollection 
                WHERE BlueprintID = @blueprintID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@blueprintID", blueprintID);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.ErrorFormat("MySQL_BlueprintRepository Delete Blueprint Error BlueprintID: {0}", blueprintID);
                }
            }
        }
        public override List<Blueprint> ListAll()
        {
            List<int> blueprintIDs = new List<int>();
            string sqlString = @"SELECT BlueprintID from BlueprintCollection ;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int blueprintID = reader.GetInt32(0);
                        blueprintIDs.Add(blueprintID);
                    }
                }
            }
            List<Blueprint> blueprints = new List<Blueprint>();
            foreach (int blueprintID in blueprintIDs)
            {
                Blueprint.ElementInfo[] requirements = DatabaseService.RepositoryList.BlueprintRequirementRepository.ListOfBlueprint(blueprintID).ToArray();
                Blueprint.ElementInfo[] products = DatabaseService.RepositoryList.BlueprintProductRepository.ListOfBlueprint(blueprintID).ToArray();
                blueprints.Add(new Blueprint(blueprintID, requirements, products));
            }
            return blueprints;
        }
    }
}
