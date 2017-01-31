using IsolatedIslandGame.Database.Repositories;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Protocol;
using MySql.Data.MySqlClient;

namespace IsolatedIslandGame.Database.MySQL.Repositories
{
    class MySQL_VesselRepository : VesselRepository
    {
        public override bool Create(Player player, out Vessel vessel)
        {
            string sqlString = @"INSERT INTO VesselCollection 
                (OwnerPlayerID) VALUES (@ownerPlayerID) ;
                SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("ownerPlayerID", player.PlayerID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int vesselID = reader.GetInt32(0);
                        vessel = new Vessel(vesselID, player.PlayerID, 0, 0, 0, OceanType.Unknown);
                        return true;
                    }
                    else
                    {
                        vessel = null;
                        return false;
                    }
                }
            }
        }
        public override bool Read(int vesselID, out Vessel vessel)
        {
            vessel = null;
            string sqlString = @"SELECT  
                OwnerPlayerID, LocationX, LocationZ, EulerAngleY, LocatedOceanType
                from VesselCollection WHERE VesselID = @vesselID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("vesselID", vesselID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int ownerPlayerID = reader.GetInt32(0);
                        float locationX = reader.GetFloat(1);
                        float locationZ = reader.GetFloat(2);
                        float eulerAngleY = reader.GetFloat(3);
                        OceanType locatedOceanType = (OceanType)reader.GetByte(4);

                        vessel = new Vessel(
                            vesselID: vesselID,
                            ownerPlayerID: ownerPlayerID,
                            locationX: locationX,
                            locationZ: locationZ,
                            rotationEulerAngleY: eulerAngleY,
                            locatedOceanType: locatedOceanType);
                    }
                }
            }
            if (vessel != null)
            {
                var decorations = DatabaseService.RepositoryList.DecorationRepository.ListOfVessel(vessel.VesselID);
                foreach (var decoration in decorations)
                {
                    vessel.AddDecoration(decoration);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public override bool ReadByOwnerPlayerID(int ownerPlayerID, out Vessel vessel)
        {
            vessel = null;
            string sqlString = @"SELECT  
                VesselID, LocationX, LocationZ, EulerAngleY, LocatedOceanType
                from VesselCollection WHERE OwnerPlayerID = @ownerPlayerID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("ownerPlayerID", ownerPlayerID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int vesselID = reader.GetInt32(0);
                        float locationX = reader.GetFloat(1);
                        float locationZ = reader.GetFloat(2);
                        float eulerAngleY = reader.GetFloat(3);
                        OceanType locatedOceanType = (OceanType)reader.GetByte(4);

                        vessel = new Vessel(
                            vesselID: vesselID,
                            ownerPlayerID: ownerPlayerID,
                            locationX: locationX,
                            locationZ: locationZ,
                            rotationEulerAngleY: eulerAngleY,
                            locatedOceanType: locatedOceanType);
                    }
                }
            }
            if (vessel != null)
            {
                var decorations = DatabaseService.RepositoryList.DecorationRepository.ListOfVessel(vessel.VesselID);
                foreach (var decoration in decorations)
                {
                    vessel.AddDecoration(decoration);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public override void Update(Vessel vessel)
        {
            string sqlString = @"UPDATE VesselCollection SET 
                LocationX = @locationX, LocationZ = @locationZ, EulerAngleY = @eulerAngleY, LocatedOceanType = @locatedOceanType
                WHERE VesselID = @vesselID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("locationX", vessel.LocationX);
                command.Parameters.AddWithValue("locationZ", vessel.LocationZ);
                command.Parameters.AddWithValue("eulerAngleY", vessel.RotationEulerAngleY);
                command.Parameters.AddWithValue("vesselID", vessel.VesselID);
                command.Parameters.AddWithValue("locatedOceanType", vessel.LocatedOceanType);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.ErrorFormat("MySQL_VesselRepository Save Vessel Error VesselID: {0}", vessel.VesselID);
                }
            }
            foreach (var decoration in vessel.Decorations)
            {
                DatabaseService.RepositoryList.DecorationRepository.Update(decoration, vessel.VesselID);
            }
        }
        public override void Delete(int vesselID)
        {
            string sqlString = @"DELETE FROM VesselCollection 
                WHERE VesselID = @vesselID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("vesselID", vesselID);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.ErrorFormat("MySQL_VesselRepository Delete Vessel Error VesselID: {0}", vesselID);
                }
            }
        }
    }
}
