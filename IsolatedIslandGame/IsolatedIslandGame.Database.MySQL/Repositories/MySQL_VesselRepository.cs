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
                (OwnerPlayerID) VALUES (@ownerPlayerID,@name) ;
                SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("ownerPlayerID", player.PlayerID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int vesselID = reader.GetInt32(0);
                        vessel = new Vessel(vesselID, new PlayerInformation
                        {
                            playerID = player.PlayerID,
                            nickname = player.Nickname,
                            signature = player.Signature,
                            groupType = player.GroupType,
                            vesselID = vesselID
                        }, 0, 0, 0);
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
                OwnerPlayerID, Nickname, Signature, GroupType, LocationX, LocationZ, EulerAngleY
                from VesselCollection, PlayerCollection WHERE VesselID = @vesselID AND OwnerPlayerID = PlayerID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@vesselID", vesselID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int ownerPlayerID = reader.GetInt32(0);
                        string nickname = reader.GetString(1);
                        string signature = reader.GetString(2);
                        GroupType groupType = (GroupType)reader.GetByte(3);
                        float locationX = reader.GetFloat(4);
                        float locationZ = reader.GetFloat(5);
                        float eulerAngleY = reader.GetFloat(6);

                        vessel = new Vessel(
                            vesselID: vesselID,
                            playerInformation: new PlayerInformation
                            {
                                playerID = ownerPlayerID,
                                nickname = nickname,
                                signature = signature,
                                groupType = groupType,
                                vesselID = vesselID
                            },
                            locationX: locationX,
                            locationZ: locationZ,
                            rotationEulerAngleY: eulerAngleY);
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
                VesselID, Nickname, Signature, GroupType, LocationX, LocationZ, EulerAngleY
                from VesselCollection, PlayerCollection WHERE OwnerPlayerID = @ownerPlayerID AND OwnerPlayerID = PlayerID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@ownerPlayerID", ownerPlayerID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int vesselID = reader.GetInt32(0);
                        string nickname = reader.GetString(1);
                        string signature = reader.GetString(2);
                        GroupType groupType = (GroupType)reader.GetByte(3);
                        float locationX = reader.GetFloat(4);
                        float locationZ = reader.GetFloat(5);
                        float eulerAngleY = reader.GetFloat(6);

                        vessel = new Vessel(
                            vesselID: vesselID,
                            playerInformation: new PlayerInformation
                            {
                                playerID = ownerPlayerID,
                                nickname = nickname,
                                signature = signature,
                                groupType = groupType,
                                vesselID = vesselID
                            },
                            locationX: locationX,
                            locationZ: locationZ,
                            rotationEulerAngleY: eulerAngleY);
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
                LocationX = @locationX, LocationZ = @locationZ, EulerAngleY = @eulerAngleY
                WHERE VesselID = @vesselID;";
            using (MySqlCommand command = new MySqlCommand(sqlString, DatabaseService.ConnectionList.PlayerDataConnection.Connection as MySqlConnection))
            {
                command.Parameters.AddWithValue("@locationX", vessel.LocationX);
                command.Parameters.AddWithValue("@locationZ", vessel.LocationZ);
                command.Parameters.AddWithValue("@eulerAngleY", vessel.RotationEulerAngleY);
                command.Parameters.AddWithValue("@vesselID", vessel.VesselID);
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
                command.Parameters.AddWithValue("@vesselID", vesselID);
                if (command.ExecuteNonQuery() <= 0)
                {
                    LogService.ErrorFormat("MySQL_VesselRepository Delete Vessel Error VesselID: {0}", vesselID);
                }
            }
        }
    }
}
