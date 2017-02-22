using IsolatedIslandGame.Database;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Quests;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Server.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace IsolatedIslandGame.Server
{
    public class PlayerFactory
    {
        public static PlayerFactory Instance { get; private set; }

        public static void Initial()
        {
            Instance = new PlayerFactory();
        }

        private Dictionary<int, Player> playerDictionary;
        public IEnumerable<Player> Players { get { return playerDictionary.Values.ToArray(); } }

        private Dictionary<int, Action<Blueprint>> playerGetBlueprintFunctionDictionary;

        private PlayerFactory()
        {
            playerDictionary = new Dictionary<int, Player>();
            playerGetBlueprintFunctionDictionary = new Dictionary<int, Action<Blueprint>>();
        }
        public bool ContainsPlayer(int playerID)
        {
            return playerDictionary.ContainsKey(playerID);
        }
        public bool FindPlayer(int playerID, out Player player)
        {
            if(ContainsPlayer(playerID))
            {
                player = playerDictionary[playerID];
                return true;
            }
            else
            {
                player = null;
                return false;
            }
        }
        public bool PlayerLogin(ServerUser user, ulong facebookID, string accessToken, out string debugMessage, out ErrorCode errorCode)
        {
            if(FacebookService.LoginCheck(facebookID, accessToken))
            {
                debugMessage = null;
                errorCode = ErrorCode.NoError;
                int playerID;
                Player player;
                if (DatabaseService.RepositoryList.PlayerRepository.Contains(facebookID, out playerID))
                {
                    if (!DatabaseService.RepositoryList.PlayerRepository.Read(playerID, out player))
                    {
                        debugMessage = $"Player not in PlayerRepository, PlayerID: {playerID}";
                        errorCode = ErrorCode.Fail;
                        return false;
                    }
                }
                else
                {
                    if (!DatabaseService.RepositoryList.PlayerRepository.Register(facebookID))
                    {
                        debugMessage = "register fail";
                        errorCode = ErrorCode.Fail;
                        return false;
                    }
                    if (DatabaseService.RepositoryList.PlayerRepository.Contains(facebookID, out playerID))
                    {
                        if (!DatabaseService.RepositoryList.PlayerRepository.Read(playerID, out player))
                        {
                            debugMessage = $"Player not in PlayerRepository, PlayerID: {playerID}";
                            errorCode = ErrorCode.Fail;
                            return false;
                        }
                        else
                        {
                            Quest quest;
                            QuestRecord record;
                            if ((QuestManager.Instance as QuestFactory).FindCumulativeLoginQuest(1, out quest))
                            {
                                quest.CreateRecord(player.PlayerID, out record);
                            }
                        }
                    }
                    else
                    {
                        debugMessage = string.Format("facebookID: {0} Register Fail Identity: {1}", facebookID, user.IdentityInformation);
                        errorCode = ErrorCode.Fail;
                        return false;
                    }
                }
                player.BindUser(user);
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
        public bool PlayerLoginWithPlayerID(ServerUser user, int playerID, string password, out string debugMessage, out ErrorCode errorCode)
        {
            if (HashPassword(password) == HashPassword(SystemConfiguration.Instance.AdministratorPassword))
            {
                debugMessage = null;
                errorCode = ErrorCode.NoError;
                Player player;
                if (!DatabaseService.RepositoryList.PlayerRepository.Read(playerID, out player))
                {
                    debugMessage = $"Player not in PlayerRepository, PlayerID: {playerID}";
                    errorCode = ErrorCode.Fail;
                    return false;
                }
                else
                {
                    player.BindUser(user);
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
            }
            else
            {
                debugMessage = "playerID login fail";
                errorCode = ErrorCode.Fail;
                return false;
            }
        }
        public void PlayerLogout(Player player)
        {
            if (ContainsPlayer(player.PlayerID))
            {
                UserFactory.Instance.UserDisconnect(player.User as ServerUser);
            }
        }

        public bool PlayerOnline(Player player)
        {
            Player existedPlayer;
            if (FindPlayer(player.PlayerID, out existedPlayer))
            {
                PlayerOffline(existedPlayer);
            }
            if (ContainsPlayer(player.PlayerID))
            {
                return false;
            }
            else
            {
                playerDictionary.Add(player.PlayerID, player);
                AssemblePlayer(player);
                player.User.PlayerOnline(player);
                LogService.InfoFormat("PlayerID: {0} Online from: {1}", player.PlayerID, player.LastConnectedIPAddress);

                bool isTodayFirstLogin;
                int cumulativeLoginCount;
                DatabaseService.RepositoryList.PlayerRepository.UpdateLastLoginTime(player.PlayerID, DateTime.Now, out isTodayFirstLogin, out cumulativeLoginCount);
                if(isTodayFirstLogin)
                {
                    IssueCumulativeLoginQuest(player, cumulativeLoginCount);
                }
                player.UpdateLoginStatus(isTodayFirstLogin, cumulativeLoginCount);
                return true;
            }
        }
        public void PlayerOffline(Player player)
        {
            if (ContainsPlayer(player.PlayerID))
            {
                DisassemblePlayer(player);
                playerDictionary.Remove(player.PlayerID);
            }
            LogService.InfoFormat("PlayerID: {0} Offline", player.PlayerID);
            player.User.PlayerOffline();
        }

        private void AssemblePlayer(Player player)
        {
            player.OnCreateCharacter += DatabaseService.RepositoryList.PlayerRepository.Update;
            player.OnCreateCharacter += CreateVessel;

            Inventory inventory;
            if(DatabaseService.RepositoryList.InventoryRepository.ReadByPlayerID(player.PlayerID, out inventory))
            {
                player.BindInventory(inventory);
                player.Inventory.OnItemInfoChange += player.EventManager.SyncDataResolver.SyncInventoryItemInfoChange;
            }

            Vessel vessel;
            if(DatabaseService.RepositoryList.VesselRepository.ReadByOwnerPlayerID(player.PlayerID, out vessel))
            {
                VesselManager.Instance.AddVessel(vessel);
                player.BindVessel(vessel);
            }
            
            foreach(var blueprint in DatabaseService.RepositoryList.PlayerKnownBlueprintRepository.ListOfPlayer(player.PlayerID))
            {
                player.GetBlueprint(blueprint);
            }
            Action<Blueprint> playerGetBlueprintFunction = (blueprint) =>
            {
                DatabaseService.RepositoryList.PlayerKnownBlueprintRepository.AddRelation(player.PlayerID, blueprint.BlueprintID);
            };
            playerGetBlueprintFunctionDictionary.Add(player.PlayerID, playerGetBlueprintFunction); ;
            player.OnGetBlueprint += playerGetBlueprintFunction;

            player.OnGetPlayerInformation += player.EventManager.SyncDataResolver.SyncPlayerInformation;

            DatabaseService.RepositoryList.FriendRepository.ListOfFriendInformations(player.PlayerID).ForEach(x => player.AddFriend(x));
            player.OnFriendInformationChange += player.EventManager.SyncDataResolver.SyncFriendInformationChange;

            player.OnGetPlayerConversation += player.EventManager.GetPlayerConversation;

            player.OnTransactionRequest += player.EventManager.TransactionRequest;
            player.OnTransactionStart += player.EventManager.StartTransaction;

            foreach(var questRecord in DatabaseService.RepositoryList.QuestRecordRepository.ListOfPlayer(player.PlayerID))
            {
                questRecord.RegisterObserverEvents(player);
                player.AddQuestRecord(questRecord);
                questRecord.OnQuestRecordStatusChange += player.EventManager.SyncDataResolver.SyncQuestRecordUpdated;
            }
            player.OnQuestRecordUpdated += player.EventManager.SyncDataResolver.SyncQuestRecordUpdated;
        }
        private void DisassemblePlayer(Player player)
        {
            foreach(var transaction in player.Transactions)
            {
                transaction.EndTransaction(false);
            }

            player.OnCreateCharacter -= DatabaseService.RepositoryList.PlayerRepository.Update;
            player.OnCreateCharacter -= CreateVessel;

            if (player.Inventory != null)
            {
                player.Inventory.OnItemInfoChange -= player.EventManager.SyncDataResolver.SyncInventoryItemInfoChange;
            }

            DatabaseService.RepositoryList.PlayerRepository.Update(player);
            if (player.Vessel != null)
            {
                VesselManager.Instance.RemoveVessel(player.Vessel.VesselID);
            }
            if (playerGetBlueprintFunctionDictionary.ContainsKey(player.PlayerID))
            {
                Action<Blueprint> playerGetBlueprintFunction = playerGetBlueprintFunctionDictionary[player.PlayerID];
                player.OnGetBlueprint -= playerGetBlueprintFunction;
                playerGetBlueprintFunctionDictionary.Remove(player.PlayerID);
            }

            player.OnGetPlayerInformation -= player.EventManager.SyncDataResolver.SyncPlayerInformation;

            player.OnFriendInformationChange -= player.EventManager.SyncDataResolver.SyncFriendInformationChange;

            player.OnGetPlayerConversation -= player.EventManager.GetPlayerConversation;

            player.OnTransactionRequest -= player.EventManager.TransactionRequest;
            player.OnTransactionStart -= player.EventManager.StartTransaction;

            foreach (var questRecord in DatabaseService.RepositoryList.QuestRecordRepository.ListOfPlayer(player.PlayerID))
            {
                questRecord.OnQuestRecordStatusChange -= player.EventManager.SyncDataResolver.SyncQuestRecordUpdated;
            }
            player.OnQuestRecordUpdated -= player.EventManager.SyncDataResolver.SyncQuestRecordUpdated;
        }
        private void CreateVessel(Player player)
        {
            if(player.Vessel == null)
            {
                Vessel vessel;
                if(DatabaseService.RepositoryList.VesselRepository.Create(player, out vessel))
                {
                    VesselManager.Instance.AddVessel(vessel);
                    player.BindVessel(vessel);
                }
            }
        }
        public string HashPassword(string password)
        {
            SHA512 sha512 = new SHA512CryptoServiceProvider();
            string passwordHash = Convert.ToBase64String(sha512.ComputeHash(Encoding.Default.GetBytes(password)));
            return passwordHash;
        }

        private void IssueCumulativeLoginQuest(Player player, int cumulativeLoginCount)
        {
            Quest quest;
            QuestRecord record;
            if ((QuestManager.Instance as QuestFactory).FindCumulativeLoginQuest(cumulativeLoginCount, out quest) && quest.CreateRecord(player.PlayerID, out record))
            {
                record.RegisterObserverEvents(player);
                player.AddQuestRecord(record);
                record.OnQuestRecordStatusChange += player.EventManager.SyncDataResolver.SyncQuestRecordUpdated;
            }
        }
    }
}
