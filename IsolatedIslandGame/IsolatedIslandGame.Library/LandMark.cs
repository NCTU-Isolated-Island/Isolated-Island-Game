using System;
using System.Collections.Generic;
using System.Linq;
using IsolatedIslandGame.Library.LandmarkElements;

namespace IsolatedIslandGame.Library
{
    public class Landmark
    {
        public int LandmarkID { get; private set; }
        public string LandmarkName { get; private set; }

        private Dictionary<int, LandmarkRoom> roomDictionary = new Dictionary<int, LandmarkRoom>();
        public IEnumerable<LandmarkRoom> Rooms { get { return roomDictionary.Values.ToArray(); } }
    }
}
