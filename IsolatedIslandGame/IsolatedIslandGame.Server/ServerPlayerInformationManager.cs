using IsolatedIslandGame.Database;
using IsolatedIslandGame.Library;

namespace IsolatedIslandGame.Server
{
    public class ServerPlayerInformationManager : PlayerInformationManager
    {
        public override bool ContainsPlayerInformation(int playerID)
        {
            return true;
        }
        public override bool FindPlayerInformation(int playerID, out PlayerInformation playerInformation)
        {
            if(!base.FindPlayerInformation(playerID, out playerInformation))
            {
                if (DatabaseService.RepositoryList.PlayerRepository.ReadPlayerInformation(playerID, out playerInformation))
                {
                    AddPlayerInformation(playerInformation);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
    }
}
