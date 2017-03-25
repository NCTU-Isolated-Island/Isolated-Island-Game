﻿using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using System;
using System.Linq;
using System.Text;

namespace IsolatedIslandGame.Library.Quests.Rewards
{
    public class GiveSpecificNumberSpecificLevelRandomMaterialQuestReward : QuestReward
    {
        public int MaterialCount { get; private set; }
        public int MaterialLevel { get; private set; }

        public override QuestRewardType QuestRewardType { get { return QuestRewardType.GiveSpecificNumberSpecificLevelRandomMaterial; } }
        public override string Description
        {
            get
            {
                return $"給予{MaterialLevel}階的隨機素材 x{MaterialCount}";
            }
        }

        public GiveSpecificNumberSpecificLevelRandomMaterialQuestReward(int questRewardID, int materialCount, int materialLevel) : base(questRewardID)
        {
            MaterialCount = materialCount;
            MaterialLevel = materialLevel;
        }

        public override void GiveReward(Player player)
        {
            var materials = ItemManager.Instance.Items.OfType<Material>().Where(x => x.Level == MaterialLevel).ToArray();
            Random randomGenerator = new Random(Guid.NewGuid().GetHashCode());
            StringBuilder stringBuilder = new StringBuilder("獲得了： ");
            for (int i = 0; i < MaterialCount; i++)
            {
                Material material = materials[randomGenerator.Next(0, materials.Length)];
                player.Inventory.AddItem(material, 1);
                stringBuilder.Append($"{material.ItemName} ");
            }
            player.User.EventManager.UserInform("提示", stringBuilder.ToString());
        }

        public override bool GiveRewardCheck(Player player)
        {
            return true;
        }
    }
}
