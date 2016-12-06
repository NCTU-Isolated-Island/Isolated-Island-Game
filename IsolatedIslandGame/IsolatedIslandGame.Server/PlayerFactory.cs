using IsolatedIslandGame.Database;
using IsolatedIslandGame.Database.DatabaseFormatData;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Protocol;
using System.Collections.Generic;

namespace IsolatedIslandGame.Server
{
    public class PlayerFactory
    {
        public static PlayerFactory Instance { get; private set; }

        public static void InitialFactory()
        {
            Instance = new PlayerFactory();
        }

        private Dictionary<int, Player> players;

        private PlayerFactory()
        {
            players = new Dictionary<int, Player>();
        }
        public bool PlayerLogin(ServerUser user, ulong facebookID, string accessToken, out string debugMessage, out ErrorCode errorCode)
        {
            if(FacebookService.LoginCheck(facebookID, accessToken))
            {
                debugMessage = null;
                errorCode = ErrorCode.NoError;
                int playerID;
                PlayerData playerData;
                if (!DatabaseService.RepositoryList.PlayerRepository.Contains(facebookID, out playerID))
                {
                    if(!DatabaseService.RepositoryList.PlayerRepository.Register(facebookID))
                    {
                        debugMessage = "register fail";
                        errorCode = ErrorCode.Fail;
                        return false;
                    }
                    if (DatabaseService.RepositoryList.PlayerRepository.Contains(facebookID, out playerID))
                    {
                        playerData = DatabaseService.RepositoryList.PlayerRepository.Read(playerID);
                    }
                    else
                    {
                        debugMessage = string.Format("facebookID: {0} Register Fail Identity: {1}", facebookID, user.IdentityInformation);
                        errorCode = ErrorCode.Fail;
                        return false;
                    }
                }
                else
                {
                    playerData = DatabaseService.RepositoryList.PlayerRepository.Read(playerID);
                }
                Player player = new Player(user, playerData.playerID, playerData.facebookID, playerData.nickname, playerData.signature, playerData.groupType, playerData.lastConnectedIPAddress);
                player.BindInventory(DatabaseService.RepositoryList.InventoryRepository.ReadByPlayerID(player.PlayerID));
                if (PlayerOnline(player))
                {
                    return true;
                }
                else
                {
                    debugMessage = string.Format("PlayerID: {0} already Logined from IP: {1}", player.PlayerID, player.LastConnectedIPAddress?.ToString() ?? "");
                    errorCode = ErrorCode.AlreadyExisted;
                    return false;
                }
            }
            else
            {
                debugMessage = "facebook login fail";
                errorCode = ErrorCode.Fail;
                return false;
            }
        }
        public void PlayerLogout(Player player)
        {
            if (players.ContainsKey(player.PlayerID))
            {
                UserFactory.Instance.UserDisconnect(player.User as ServerUser);
            }
        }

        public bool PlayerOnline(Player player)
        {
            if (players.ContainsKey(player.PlayerID))
            {
                return false;
            }
            else
            {
                players.Add(player.PlayerID, player);
                AssemblyPlayer(player);
                player.User.PlayerOnline(player);
                LogService.InfoFormat("PlayerID: {0} Online from: {1}", player.PlayerID, player.LastConnectedIPAddress);
                return true;
            }
        }
        public void PlayerOffline(Player player)
        {
            if (players.ContainsKey(player.PlayerID))
            {
                DisassemblyPlayer(player);
                DatabaseService.RepositoryList.PlayerRepository.Update(player);
                players.Remove(player.PlayerID);
            }
            LogService.InfoFormat("PlayerID: {0} Offline", player.PlayerID);
            player.User.PlayerOffline();
        }

        private void AssemblyPlayer(Player player)
        {
            player.OnCreateCharacter += DatabaseService.RepositoryList.PlayerRepository.Update;
        }
        private void DisassemblyPlayer(Player player)
        {
            player.OnCreateCharacter -= DatabaseService.RepositoryList.PlayerRepository.Update;
        }
    }
}
