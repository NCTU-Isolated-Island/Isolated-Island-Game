using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Database;

namespace IsolatedIslandGame.Server
{
    public class LandmarkFactory : LandmarkManager
    {
        public LandmarkFactory()
        {
            OnLandmarkUpdated += AssemblyLandmark;

            DatabaseService.RepositoryList.LandmarkRepository.ListAll().ForEach(x => LoadLandmark(x));
        }

        private void AssemblyLandmark(Landmark landmark)
        {
            landmark.OnLandmarkRoomChange += landmark.EventManager.SyncDataResolver.SyncLandmarkRoomChange;
            landmark.OnLandmarkRoomChange += DynamicAssemblyLandmarkRoom;
        }
        private void DynamicAssemblyLandmarkRoom(DataChangeType changeType, LandmarkRoom room)
        {
            switch(changeType)
            {
                case DataChangeType.Add:
                case DataChangeType.Update:
                    room.OnMutiplayerSynthesizeParticipantInfoChange += room.EventManager.SyncDataResolver.SyncMultiplayerSynthesizeParticipantInfoChange;
                    break;
                case DataChangeType.Remove:
                    room.OnMutiplayerSynthesizeParticipantInfoChange -= room.EventManager.SyncDataResolver.SyncMultiplayerSynthesizeParticipantInfoChange;
                    break;
            }
        }
    }
}
