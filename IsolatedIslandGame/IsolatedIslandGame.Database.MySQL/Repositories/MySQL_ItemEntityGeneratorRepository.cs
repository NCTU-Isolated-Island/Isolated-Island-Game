using IsolatedIslandGame.Database.Repositories;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.MySQL.Repositories
{
    class MySQL_ItemEntityGeneratorRepository : ItemEntityGeneratorRepository
    {
        public override List<ItemEntityGenerator> ListAllItemEntityGenerators()
        {
            List<ItemEntityGenerator> itemEntityGenerators = new List<ItemEntityGenerator>();
            string sqlString = @"SELECT  
                ItemEntityGeneratorID, PositionX, PositionZ, GeneratingRadius, GeneratingPeriod, GeneratingProbability
                from ItemEntityGeneratorCollection ;";
            lock (DatabaseService.ConnectionList.SystemDataConnection)
            {
                using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SystemDataConnection.Connection as MySqlConnection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int itemEntityGeneratorID = reader.GetInt32(0);
                            float positionX = reader.GetFloat(1);
                            float positionZ = reader.GetFloat(2);
                            float generatingRadius = reader.GetFloat(3);
                            TimeSpan generatingPeriod = reader.GetTimeSpan(4);
                            float generatingProbability = reader.GetFloat(5);

                            itemEntityGenerators.Add(new ItemEntityGenerator(itemEntityGeneratorID, positionX, positionZ, generatingRadius, generatingPeriod, generatingProbability, DatabaseService.RepositoryList.ItemEntityRepository.Create));
                        }
                    }
                }
            }
            itemEntityGenerators.ForEach(x => x.LoadItemEntityGeneratingFactors(ListItemEntityGeneratingFactorsOfGenerator(x.ItemEntityGeneratorID)));
            return itemEntityGenerators;
        }

        public override List<ItemEntityGeneratingFactor> ListItemEntityGeneratingFactorsOfGenerator(int itemEntityGeneratorID)
        {
            List<ItemEntityGeneratingFactor> factors = new List<ItemEntityGeneratingFactor>();
            string sqlString = @"SELECT  
                ItemEntityGeneratingFactorID, GeneratingItemID, GeneratingWeight
                from ItemEntityGeneratingFactorCollection 
                where ItemEntityGeneratorID = @itemEntityGeneratorID;";
            lock (DatabaseService.ConnectionList.SystemDataConnection)
            {
                using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SystemDataConnection.Connection as MySqlConnection))
                {
                    command.Parameters.AddWithValue("itemEntityGeneratorID", itemEntityGeneratorID);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int itemEntityGeneratingFactorID = reader.GetInt32(0);
                            int generatingItemID = reader.GetInt32(1);
                            int generatingWeight = reader.GetInt32(2);

                            factors.Add(new ItemEntityGeneratingFactor(itemEntityGeneratingFactorID, generatingItemID, generatingWeight));
                        }
                    }
                }
            }
            return factors;
        }
    }
}
