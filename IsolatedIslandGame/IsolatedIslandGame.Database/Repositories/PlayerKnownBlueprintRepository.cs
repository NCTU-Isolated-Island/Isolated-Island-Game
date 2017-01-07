using IsolatedIslandGame.Library;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.Repositories
{
    public abstract class PlayerKnownBlueprintRepository
    {
        public abstract void AddRelation(int playerID, int blueprintID);
        public abstract List<Blueprint> ListOfPlayer(int playerID);
    }
}
