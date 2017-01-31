using IsolatedIslandGame.Database;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;
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

        private IslandManager()
        {
            Island.Initial();
            Island.Instance.OnSendMaterial += SaveSendMaterialRecord;
            Island.Instance.OnTotalScoreUpdated += SystemManager.Instance.EventManager.SyncDataResolver.SyncIslandTotalScoreUpdated;
            Island.Instance.OnTodayMaterialRankingUpdated += SyncTodayMaterialRankingUpdated;
            Island.Instance.OnPlayerScoreRankingUpdated += SyncPlayerScoreRankingUpdated;

            for (GroupType groupType = GroupType.Animal; groupType <= GroupType.Farmer; groupType++)
            {
                int totalScore = DatabaseService.RepositoryList.IslandMaterialRepository.ReadTotalScore(groupType);
                Island.Instance.UpdateTotalScore(groupType, totalScore);
            }
            foreach (var info in DatabaseService.RepositoryList.IslandMaterialRepository.ListPlayerScoreRanking())
            {
                Island.Instance.UpdatePlayerScoreRanking(info);
            }
            foreach (var info in DatabaseService.RepositoryList.IslandMaterialRepository.ListTodayMaterialRanking())
            {
                Island.Instance.UpdateTodayMaterialRanking(info);
            }
            Scheduler.Instance.AddTask(DateTime.Today + TimeSpan.FromHours(6), ResetTodayMaterialRanking);
        }
        private void SaveSendMaterialRecord(Player player, Material material)
        {
            DatabaseService.RepositoryList.IslandMaterialRepository.Save(new Island.PlayerMaterialInfo { playerID = player.PlayerID, materialItemID = material.ItemID });
        }
        private void SyncTodayMaterialRankingUpdated(Island.PlayerMaterialInfo info)
        {
            foreach(var player in PlayerFactory.Instance.Players)
            {
                player.SyncPlayerInformation(info.playerID);
            }
            SystemManager.Instance.EventManager.SyncDataResolver.SyncIslandTodayMaterialRankingUpdated(info);
        }
        private void SyncPlayerScoreRankingUpdated(Island.PlayerScoreInfo info)
        {
            foreach (var player in PlayerFactory.Instance.Players)
            {
                player.SyncPlayerInformation(info.playerID);
            }
            SystemManager.Instance.EventManager.SyncDataResolver.SyncIslandPlayerScoreRankingUpdated(info);
        }
    }
}
