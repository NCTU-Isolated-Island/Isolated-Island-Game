using IsolatedIslandGame.Database;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Quests;
using IsolatedIslandGame.Protocol;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Server
{
    public class QuestFactory : QuestManager
    {
        private List<Quest> questsWhenRegistered = new List<Quest>();
        public IEnumerable<Quest> QuestsWhenRegistered { get { return questsWhenRegistered.ToArray(); } }

        private List<Quest> questsWhenChosedGroup = new List<Quest>();
        public IEnumerable<Quest> QuestsWhenChosedGroup { get { return questsWhenChosedGroup.ToArray(); } }

        private List<Quest> questsWhenTodayFirstLogin = new List<Quest>();
        public IEnumerable<Quest> QuestsWhenTodayFirstLogin { get { return questsWhenTodayFirstLogin.ToArray(); } }

        private List<Quest> questsWhenEveryHourPassed = new List<Quest>();
        public IEnumerable<Quest> QuestsWhenEveryHourPassed { get { return questsWhenEveryHourPassed.ToArray(); } }

        private List<Quest> questsWhenEveryDayPassed = new List<Quest>();
        public IEnumerable<Quest> QuestsWhenEveryDayPassed { get { return questsWhenEveryDayPassed.ToArray(); } }

        private Dictionary<OceanType, List<Quest>> questsWhenEnteredSpecificOcean = new Dictionary<OceanType, List<Quest>>
        {
            { OceanType.Unknown, new List<Quest>() },
            { OceanType.Type1, new List<Quest>() },
            { OceanType.Type2, new List<Quest>() },
            { OceanType.Type3, new List<Quest>() },
            { OceanType.Type4, new List<Quest>() },
            { OceanType.Type5, new List<Quest>() },
            { OceanType.Type6, new List<Quest>() },
            { OceanType.Type7, new List<Quest>() }
        };

        private Dictionary<OceanType, Dictionary<int, HashSet<int>>> hasGivenEnteredSpecificOceanQuestRecords = new Dictionary<OceanType, Dictionary<int, HashSet<int>>>
        {
            { OceanType.Unknown, new Dictionary<int, HashSet<int>>() },
            { OceanType.Type1, new Dictionary<int, HashSet<int>>() },
            { OceanType.Type2, new Dictionary<int, HashSet<int>>() },
            { OceanType.Type3, new Dictionary<int, HashSet<int>>() },
            { OceanType.Type4, new Dictionary<int, HashSet<int>>() },
            { OceanType.Type5, new Dictionary<int, HashSet<int>>() },
            { OceanType.Type6, new Dictionary<int, HashSet<int>>() },
            { OceanType.Type7, new Dictionary<int, HashSet<int>>() }
        };

        public QuestFactory()
        {
            var quests = DatabaseService.RepositoryList.QuestRepository.ListAll();
            foreach (var quest in quests)
            {
                AddQuest(quest);
            }
            foreach(var questID in DatabaseService.RepositoryList.QuestRepository.ListQuestIDsWhenRegistered())
            {
                Quest quest;
                if(FindQuest(questID, out quest))
                {
                    questsWhenRegistered.Add(quest);
                }
            }
            foreach (var questID in DatabaseService.RepositoryList.QuestRepository.ListQuestIDsWhenChosedGroup())
            {
                Quest quest;
                if (FindQuest(questID, out quest))
                {
                    questsWhenChosedGroup.Add(quest);
                }
            }
            foreach (var questID in DatabaseService.RepositoryList.QuestRepository.ListQuestIDsWhenTodayFirstLogin())
            {
                Quest quest;
                if (FindQuest(questID, out quest))
                {
                    questsWhenTodayFirstLogin.Add(quest);
                }
            }
            foreach (var questID in DatabaseService.RepositoryList.QuestRepository.ListQuestIDsWhenEveryHourPassed())
            {
                Quest quest;
                if (FindQuest(questID, out quest))
                {
                    questsWhenEveryHourPassed.Add(quest);
                }
            }
            foreach (var questID in DatabaseService.RepositoryList.QuestRepository.ListQuestIDsWhenEveryDayPassed())
            {
                Quest quest;
                if (FindQuest(questID, out quest))
                {
                    questsWhenEveryDayPassed.Add(quest);
                }
            }
            foreach (var ocen_questID_Pair in DatabaseService.RepositoryList.QuestRepository.ListQuestIDsWhenEnteredSpecificOcean())
            {
                foreach (var questID in ocen_questID_Pair.Value)
                {
                    Quest quest;
                    if (FindQuest(questID, out quest))
                    {
                        questsWhenEnteredSpecificOcean[ocen_questID_Pair.Key].Add(quest);
                    }
                }
            }

            AssignEveryHourPassedQuests();
            AssignEveryDayPassedQuests();
            (VesselManager.Instance as ServerVesselManager).OnVesselOceanChanged += DetectVesselOceanChange;
        }
        public bool QuestsWhenEnteredSpecificOcean(OceanType oceanType, out IEnumerable<Quest> quests)
        {
            if(questsWhenEnteredSpecificOcean.ContainsKey(oceanType))
            {
                quests = questsWhenEnteredSpecificOcean[oceanType].ToArray();
                return true;
            }
            else
            {
                quests = null;
                return false;
            }
        }

        private void AssignEveryHourPassedQuests()
        {
            foreach(int playerID in DatabaseService.RepositoryList.PlayerRepository.ListAllPlayerID())
            {
                foreach(Quest quest in QuestsWhenEveryHourPassed)
                {
                    if (!DatabaseService.RepositoryList.QuestRecordRepository.IsPlayerHasAnyStillNotGottenRewardQuest(playerID, quest.QuestID))
                    {
                        QuestRecord record;
                        Player player;
                        if(quest.CreateRecord(playerID, out record) && PlayerFactory.Instance.FindPlayer(playerID, out player))
                        {
                            record.RegisterObserverEvents(player);
                            player.AddQuestRecord(record);
                            record.OnQuestRecordStatusChange += player.EventManager.SyncDataResolver.SyncQuestRecordUpdated;
                        }
                    }
                }
            }
            DateTime nextHour = DateTime.Today;
            while (nextHour < DateTime.Now)
            {
                nextHour += TimeSpan.FromHours(1);
            }

            Scheduler.Instance.AddTask(nextHour, AssignEveryHourPassedQuests);
        }
        private void AssignEveryDayPassedQuests()
        {
            foreach (int playerID in DatabaseService.RepositoryList.PlayerRepository.ListAllPlayerID())
            {
                foreach (Quest quest in QuestsWhenEveryDayPassed)
                {
                    if (!DatabaseService.RepositoryList.QuestRecordRepository.IsPlayerHasAnyStillNotGottenRewardQuest(playerID, quest.QuestID))
                    {
                        QuestRecord record;
                        Player player;
                        if (quest.CreateRecord(playerID, out record) && PlayerFactory.Instance.FindPlayer(playerID, out player))
                        {
                            record.RegisterObserverEvents(player);
                            player.AddQuestRecord(record);
                            record.OnQuestRecordStatusChange += player.EventManager.SyncDataResolver.SyncQuestRecordUpdated;
                        }
                    }
                }
            }
            DateTime nextDay = DateTime.Today;
            while (nextDay < DateTime.Now)
            {
                nextDay += TimeSpan.FromDays(1);
            }

            Scheduler.Instance.AddTask(nextDay, AssignEveryDayPassedQuests);

            foreach (var setPair in hasGivenEnteredSpecificOceanQuestRecords)
            {
                setPair.Value.Clear();
            }
        }

        private void DetectVesselOceanChange(Vessel vessel)
        {
            Player player;
            if(PlayerFactory.Instance.FindPlayer(vessel.OwnerPlayerID, out player))
            {
                GiveEnteredSpecificOceanQuest(player, vessel.LocatedOceanType);
            }
        }
        private void GiveEnteredSpecificOceanQuest(Player player, OceanType oceanType)
        {
            var playerID_QuestSet = hasGivenEnteredSpecificOceanQuestRecords[oceanType];
            IEnumerable<Quest> quests;
            if(QuestsWhenEnteredSpecificOcean(oceanType, out quests))
            {
                if (!playerID_QuestSet.ContainsKey(player.PlayerID))
                {
                    playerID_QuestSet.Add(player.PlayerID, new HashSet<int>());
                }
                foreach (Quest quest in quests)
                {
                    if (!playerID_QuestSet[player.PlayerID].Contains(quest.QuestID) && !DatabaseService.RepositoryList.QuestRecordRepository.IsPlayerHasAnyStillNotGottenRewardQuest(player.PlayerID, quest.QuestID))
                    {
                        QuestRecord record;
                        if (quest.CreateRecord(player.PlayerID, out record))
                        {
                            record.RegisterObserverEvents(player);
                            player.AddQuestRecord(record);
                            record.OnQuestRecordStatusChange += player.EventManager.SyncDataResolver.SyncQuestRecordUpdated;
                            playerID_QuestSet[player.PlayerID].Add(quest.QuestID);
                        }
                    }
                }
            }
        }
    }
}
