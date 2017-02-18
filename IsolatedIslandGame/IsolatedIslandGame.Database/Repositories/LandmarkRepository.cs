using IsolatedIslandGame.Library.Landmarks;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.Repositories
{
    public abstract class LandmarkRepository
    {
        public abstract List<Landmark> ListAll();
    }
}
