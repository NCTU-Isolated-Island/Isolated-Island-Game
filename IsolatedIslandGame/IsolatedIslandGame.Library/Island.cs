using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using System;
using System.Linq;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library
{
    public class Island
    {
        public struct PlayerMaterialInfo : IComparable<PlayerMaterialInfo>
        {
            public int playerID;
            public int materialItemID;

            public int CompareTo(PlayerMaterialInfo other)
            {
                Material material, otherMaterial;
                if(ItemManager.Instance.SpecializeItemToMaterial(materialItemID, out material) && ItemManager.Instance.SpecializeItemToMaterial(other.materialItemID, out otherMaterial))
                {
                    return material.Score.CompareTo(otherMaterial.Score);
                }
                else
                {
                    return 0;
                }
            }
        }
        public struct PlayerScoreInfo : IComparable<PlayerScoreInfo>
        {
            public int playerID;
            public int score;

            public int CompareTo(PlayerScoreInfo other)
            {
                return score.CompareTo(other.score);
            }
        }

        public static Island Instance { get; private set; }
        public static void Initial()
        {
            Instance = new Island();
        }

        private Dictionary<GroupType, int> totalScoreTable = new Dictionary<GroupType, int>();
        private Dictionary<int, PlayerMaterialInfo> todayMaterialRanking = new Dictionary<int, PlayerMaterialInfo>();
        private Dictionary<int, PlayerScoreInfo> playerScoreRanking = new Dictionary<int, PlayerScoreInfo>();

        public IEnumerable<PlayerMaterialInfo> TodayMaterialRanking { get { return todayMaterialRanking.Values.ToArray(); } }
        public IEnumerable<PlayerScoreInfo> PlayerScoreRanking { get { return playerScoreRanking.Values.ToArray(); } }

        public delegate void TotalScoreUpdatedEventHandler(GroupType groupType, int score);
        private event TotalScoreUpdatedEventHandler onTotalScoreUpdated;
        public event TotalScoreUpdatedEventHandler OnTotalScoreUpdated { add { onTotalScoreUpdated += value; } remove { onTotalScoreUpdated -= value; } }

        private event Action<PlayerMaterialInfo> onTodayMaterialRankingUpdated;
        public event Action<PlayerMaterialInfo> OnTodayMaterialRankingUpdated { add { onTodayMaterialRankingUpdated += value; } remove { onTodayMaterialRankingUpdated -= value; } }

        private event Action<PlayerScoreInfo> onPlayerScoreRankingUpdated;
        public event Action<PlayerScoreInfo> OnPlayerScoreRankingUpdated { add { onPlayerScoreRankingUpdated += value; } remove { onPlayerScoreRankingUpdated -= value; } }

        public delegate void SendMaterialEventHandler(Player player, Material material);
        private event SendMaterialEventHandler onSendMaterial;
        public event SendMaterialEventHandler OnSendMaterial { add { onSendMaterial += value; } remove { onSendMaterial -= value; } }

        private Island() { }
        public int GetTotalScore(GroupType groupType)
        {
            if (totalScoreTable.ContainsKey(groupType))
            {
                return totalScoreTable[groupType];
            }
            else
            {
                return 0;
            }
        }
        public bool SendMaterial(Player player, Material material)
        {
            if(todayMaterialRanking.ContainsKey(player.PlayerID))
            {
                return false;
            }
            else
            {
                if(totalScoreTable.ContainsKey(player.GroupType))
                {
                    UpdateTotalScore(player.GroupType, totalScoreTable[player.GroupType] + material.Score);
                    UpdateTodayMaterialRanking(new PlayerMaterialInfo { playerID = player.PlayerID, materialItemID = material.ItemID });
                    if (playerScoreRanking.ContainsKey(player.PlayerID))
                    {
                        UpdatePlayerScoreRanking(new PlayerScoreInfo { playerID = player.PlayerID, score = playerScoreRanking[player.PlayerID].score + material.Score });
                    }
                    else
                    {
                        UpdatePlayerScoreRanking(new PlayerScoreInfo { playerID = player.PlayerID, score = material.Score });
                    }
                    onSendMaterial?.Invoke(player, material);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public void UpdateTotalScore(GroupType groupType, int score)
        {
            if (totalScoreTable.ContainsKey(groupType))
            {
                totalScoreTable[groupType] = score;
                onTotalScoreUpdated?.Invoke(groupType, score);
            }
            else
            {
                totalScoreTable.Add(groupType, score);
                onTotalScoreUpdated?.Invoke(groupType, score);
            }
        }
        public void UpdateTodayMaterialRanking(PlayerMaterialInfo info)
        {
            if(todayMaterialRanking.ContainsKey(info.playerID))
            {
                todayMaterialRanking[info.playerID] = info;
                onTodayMaterialRankingUpdated?.Invoke(info);
            }
            else
            {
                todayMaterialRanking.Add(info.playerID, info);
                onTodayMaterialRankingUpdated?.Invoke(info);
            }
        }
        public void UpdatePlayerScoreRanking(PlayerScoreInfo info)
        {
            if(playerScoreRanking.ContainsKey(info.playerID))
            {
                playerScoreRanking[info.playerID] = info;
                onPlayerScoreRankingUpdated?.Invoke(info);
            }
            else
            {
                playerScoreRanking.Add(info.playerID, info);
                onPlayerScoreRankingUpdated?.Invoke(info);
            }
        }
        public void ResetTodayMaterialRanking()
        {
            todayMaterialRanking.Clear();
        }
    }
}
