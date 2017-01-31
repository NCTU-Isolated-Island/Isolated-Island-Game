using IsolatedIslandGame.Database;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Protocol;
using System;

namespace IsolatedIslandGame.Server
{
    public class IslandManager
    {
        public static IslandManager Instance { get; private set; }
        public static void Initial()
        {
            Instance = new IslandManager();
            Island.Initial();

            for(GroupType groupType = GroupType.Animal; groupType <= GroupType.Farmer; groupType++)
            {
                int totalScore = DatabaseService.RepositoryList.IslandMaterialRepository.ReadTotalScore(groupType);
                Island.Instance.UpdateTotalScore(groupType, totalScore);
            }
            foreach(var info in DatabaseService.RepositoryList.IslandMaterialRepository.ListPlayerScoreRanking())
            {
                Island.Instance.UpdatePlayerScoreRanking(info);
            }
            foreach (var info in DatabaseService.RepositoryList.IslandMaterialRepository.ListTodayMaterialRanking())
            {
                Island.Instance.UpdateTodayMaterialRanking(info);
            }
            Scheduler.Instance.AddTask(DateTime.Today + TimeSpan.FromHours(6), ResetTodayMaterialRanking);
        }
        private static void ResetTodayMaterialRanking()
        {
            Island.Instance.ResetTodayMaterialRanking();
            foreach (var info in DatabaseService.RepositoryList.IslandMaterialRepository.ListTodayMaterialRanking())
            {
                Island.Instance.UpdateTodayMaterialRanking(info);
            }
            LogService.Info($"Island ResetTodayMaterialRanking");
            Scheduler.Instance.AddTask(DateTime.Today + TimeSpan.FromHours(6), ResetTodayMaterialRanking);
        }

        private IslandManager() { }
    }
}
