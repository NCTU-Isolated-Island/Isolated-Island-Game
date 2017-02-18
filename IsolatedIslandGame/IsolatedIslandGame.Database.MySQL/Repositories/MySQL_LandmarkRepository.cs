using IsolatedIslandGame.Database.Repositories;
using IsolatedIslandGame.Library.Landmarks;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.MySQL.Repositories
{
    class MySQL_LandmarkRepository : LandmarkRepository
    {
        public override List<Landmark> ListAll()
        {
            List<Landmark> landmarks = new List<Landmark>();
            string sqlString = @"SELECT LandmarkID, LandmarkName, Description from LandmarkCollection ;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.SettingDataConnection.Connection as MySqlConnection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int landmarkID = reader.GetInt32(0);
                        string landmarkName = reader.GetString(1);
                        string description = reader.GetString(2);

                        landmarks.Add(new Landmark(landmarkID, landmarkName, description));
                    }
                }
            }
            return landmarks;
        }
    }
}
