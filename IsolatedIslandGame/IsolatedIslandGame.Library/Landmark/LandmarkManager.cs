using System;
using System.Collections.Generic;
using System.Linq;

namespace IsolatedIslandGame.Library.Landmarks
{
    public abstract class LandmarkManager
    {
        public static LandmarkManager Instance { get; private set; }
        public static void Initial(LandmarkManager landmarkManager)
        {
            Instance = landmarkManager;
        }

        private Dictionary<int, Landmark> landmarkDictionary = new Dictionary<int, Landmark>();
        public IEnumerable<Landmark> Landmarks { get { return landmarkDictionary.Values.ToArray(); } }

        private event Action<Landmark> onLandmarkUpdated;
        public event Action<Landmark> OnLandmarkUpdated { add { onLandmarkUpdated += value; } remove { onLandmarkUpdated -= value; } }

        protected LandmarkManager() { }
        public bool ContainsLandmark(int landmarkID)
        {
            return landmarkDictionary.ContainsKey(landmarkID);
        }
        public bool FindLandmark(int landmarkID, out Landmark landmark)
        {
            return landmarkDictionary.TryGetValue(landmarkID, out landmark);
        }
        public void LoadLandmark(Landmark landmark)
        {
            if(ContainsLandmark(landmark.LandmarkID))
            {
                landmarkDictionary[landmark.LandmarkID] = landmark;
            }
            else
            {
                landmarkDictionary.Add(landmark.LandmarkID, landmark);
            }
            onLandmarkUpdated?.Invoke(landmark);
        }
    }
}
