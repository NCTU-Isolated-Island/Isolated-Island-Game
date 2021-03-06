﻿using IsolatedIslandGame.Library.Quests;
using System.Collections.Generic;
using System.Linq;

namespace IsolatedIslandGame.Library
{
    public class QuestManager
    {
        public static QuestManager Instance { get; private set; }
        public static void Initial(QuestManager manager)
        {
            Instance = manager;
        }

        protected Dictionary<int, Quest> questDictionary = new Dictionary<int, Quest>();
        protected Dictionary<int, QuestRequirement> questRequirementDictionary = new Dictionary<int, QuestRequirement>();

        public IEnumerable<Quest> Quests { get { return questDictionary.Values.ToArray(); } }

        public QuestManager() { }
        public bool ContainsQuest(int questID)
        {
            return questDictionary.ContainsKey(questID);
        }
        public bool FindQuest(int questID, out Quest quest)
        {
            if (ContainsQuest(questID))
            {
                quest = questDictionary[questID];
                return true;
            }
            else
            {
                quest = null;
                return false;
            }
        }
        public void AddQuest(Quest quest)
        {
            if (!ContainsQuest(quest.QuestID))
            {
                questDictionary.Add(quest.QuestID, quest);
                foreach(var requirement in quest.Requirements)
                {
                    AddQuestRequirement(requirement);
                }
            }
        }
        public bool ContainsQuestRequirement(int questRequirementID)
        {
            return questRequirementDictionary.ContainsKey(questRequirementID);
        }
        public bool FindQuestRequirement(int questRequirementID, out QuestRequirement questRequirement)
        {
            if (ContainsQuestRequirement(questRequirementID))
            {
                questRequirement = questRequirementDictionary[questRequirementID];
                return true;
            }
            else
            {
                questRequirement = null;
                return false;
            }
        }
        public void AddQuestRequirement(QuestRequirement questRequirement)
        {
            if (!ContainsQuestRequirement(questRequirement.QuestRequirementID))
            {
                questRequirementDictionary.Add(questRequirement.QuestRequirementID, questRequirement);
            }
        }
    }
}
