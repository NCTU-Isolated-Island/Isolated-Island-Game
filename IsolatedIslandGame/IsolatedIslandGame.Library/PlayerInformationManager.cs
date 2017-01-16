using System.Collections.Generic;

namespace IsolatedIslandGame.Library
{
    public class PlayerInformationManager
    {
        public static PlayerInformationManager Instance { get; private set; }
        public static void InitialManager(PlayerInformationManager playerInformationManager)
        {
            Instance = playerInformationManager;
        }

        private Dictionary<int, PlayerInformation> playerInformationDictionary;

        public PlayerInformationManager()
        {
            playerInformationDictionary = new Dictionary<int, PlayerInformation>();
        }

        public virtual bool ContainsPlayerInformation(int playerID)
        {
            return playerInformationDictionary.ContainsKey(playerID);
        }
        public virtual bool FindPlayerInformation(int playerID, out PlayerInformation playerInformation)
        {
            if(ContainsPlayerInformation(playerID))
            {
                playerInformation = playerInformationDictionary[playerID];
                return true;
            }
            else
            {
                playerInformation = new PlayerInformation();
                return false;
            }
        }
        public void AddPlayerInformation(PlayerInformation playerInformation)
        {
            if (!ContainsPlayerInformation(playerInformation.playerID))
            {
                playerInformationDictionary.Add(playerInformation.playerID, playerInformation);
            }
        }
    }
}
