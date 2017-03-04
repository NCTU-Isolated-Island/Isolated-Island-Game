﻿using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using System;
using System.Linq;

namespace IsolatedIslandGame.Library.Quests.Rewards
{
    public class GiveSpecificNumberSpecificLevelSpecificGroupRandomMaterialQuestReward : QuestReward
    {
        public int MaterialCount { get; private set; }
        public int MaterialLevel { get; private set; }
        public GroupType GroupType { get; private set; }

        public override QuestRewardType QuestRewardType { get { return QuestRewardType.GiveSpecificNumberSpecificLevelSpecificGroupRandomMaterial; } }
        public override string Description
        {
            get
            {
                return $"{GroupNameMapping.GetGroupName(GroupType)}陣營{MaterialLevel}階的隨機素材 x{MaterialCount}";
            }
        }

        public GiveSpecificNumberSpecificLevelSpecificGroupRandomMaterialQuestReward(int questRewardID, int materialCount, int materialLevel, GroupType groupType) : base(questRewardID)
        {
            MaterialCount = materialCount;
            MaterialLevel = materialLevel;
            GroupType = groupType;
        }

        public override void GiveReward(Player player)
        {
            var materials = ItemManager.Instance.Items.OfType<Material>().Where(x => x.Level == MaterialLevel && x.GroupType == GroupType).ToArray();
            Random randomGenerator = new Random();
            for (int i = 0; i < MaterialCount; i++)
            {
                player.Inventory.AddItem(materials[randomGenerator.Next(0, materials.Length)], 1);
            }
        }

        public override bool GiveRewardCheck(Player player)
        {
            return true;
        }
    }
}