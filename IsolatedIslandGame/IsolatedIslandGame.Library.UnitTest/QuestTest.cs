using IsolatedIslandGame.Library.Quests;
using IsolatedIslandGame.Library.CommunicationInfrastructure;
using IsolatedIslandGame.Protocol;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IsolatedIslandGame.Library.UnitTest
{
    [TestClass]
    public class QuestTest
    {
        [TestMethod]
        public void SendMessageToDifferentOnlineFriendQuestTest1()
        {
            ItemManager.Initial(new TestItemManager());
            ItemManager.Instance.AddItem(new Item(1, "TestItem 1", "xxx"));

            QuestRequirement requirement = new SendMessageToDifferentOnlineFriendInTheSameOceanQuestRequirement(1, 2);
            Quest quest = new Quest(1, QuestType.SendMessage, "Test", new List<QuestRequirement>
            {
                requirement
            },
            new List<QuestReward>
            {
                new GiveItemQuestReward(1, 1, 1)
            },
            "TestSendMessageToDifferentOnlineFriendQuest");

            Player player1 = new Player(1, 0, "TestPlayer 1", "xx", GroupType.No, null);
            player1.BindInventory(new Inventory(1, 40));
            player1.AddFriend(new FriendInformation { friendPlayerID = 2, isConfirmed = true, isInviter = true });
            player1.AddFriend(new FriendInformation { friendPlayerID = 3, isConfirmed = true, isInviter = true });

            PlayerInformationManager.Initial(new PlayerInformationManager());
            PlayerInformationManager.Instance.AddPlayerInformation(new PlayerInformation
            {
                playerID = 1,
                groupType = GroupType.No,
                nickname = "",
                signature = "",
                vesselID = 1
            });
            PlayerInformationManager.Instance.AddPlayerInformation(new PlayerInformation
            {
                playerID = 2,
                groupType = GroupType.No,
                nickname = "",
                signature = "",
                vesselID = 2
            });
            PlayerInformationManager.Instance.AddPlayerInformation(new PlayerInformation
            {
                playerID = 3,
                groupType = GroupType.No,
                nickname = "",
                signature = "",
                vesselID = 3
            });

            VesselManager.Initial(new TestVesselManager());
            player1.BindVessel(new Vessel(1, 1, 0, 0, 0, OceanType.Unknown));
            VesselManager.Instance.AddVessel(player1.Vessel);
            VesselManager.Instance.AddVessel(new Vessel(2, 2, 0, 0, 0, OceanType.Unknown));
            VesselManager.Instance.AddVessel(new Vessel(3, 3, 0, 0, 0, OceanType.Unknown));

            QuestRecord record = new QuestRecord(1, player1.PlayerID, quest, new List<QuestRequirementRecord>()
            {
                new SendMessageToDifferentOnlineFriendTheSameOceanQuestRequirementRecord(1, requirement, new HashSet<int>())
            });
            record.RegisterObserverEvents(player1);
            record.OnQuestRecordStatusChange += (recordState) =>
            {
                if(recordState.IsFinished)
                {
                    foreach(var questReward in recordState.Quest.Rewards)
                    {
                        if(questReward.GiveRewardCheck(player1))
                        {
                            questReward.GiveReward(player1);
                        }
                        else
                        {
                            Assert.Fail();
                        }
                    }
                }
            };

            Assert.IsFalse(record.IsFinished);
            Assert.IsTrue(VesselManager.Instance.ContainsVesselWithOwnerPlayerID(2));
            Assert.IsTrue(VesselManager.Instance.ContainsVesselWithOwnerPlayerID(3));
            player1.GetPlayerConversation(new TextData.PlayerConversation
            {
                message = new TextData.PlayerMessage
                {
                    content = "",
                    playerMessageID = 1,
                    senderPlayerID = 1,
                    sendTime = DateTime.Now
                },
                hasRead = false,
                receiverPlayerID = 2
            });
            Assert.IsFalse(record.IsFinished);
            Assert.AreEqual($"人數： 1/2", record.RequirementRecords.First().ProgressStatus);
            player1.GetPlayerConversation(new TextData.PlayerConversation
            {
                message = new TextData.PlayerMessage
                {
                    content = "",
                    playerMessageID = 1,
                    senderPlayerID = 1,
                    sendTime = DateTime.Now
                },
                hasRead = false,
                receiverPlayerID = 2
            });
            Assert.IsFalse(record.IsFinished);
            Assert.AreEqual($"人數： 1/2", record.RequirementRecords.First().ProgressStatus);
            player1.GetPlayerConversation(new TextData.PlayerConversation
            {
                message = new TextData.PlayerMessage
                {
                    content = "",
                    playerMessageID = 1,
                    senderPlayerID = 1,
                    sendTime = DateTime.Now
                },
                hasRead = false,
                receiverPlayerID = 3
            });
            Assert.IsTrue(record.IsFinished);
            Assert.AreEqual($"人數： 2/2", record.RequirementRecords.First().ProgressStatus);
        }

        [TestMethod]
        public void QuestSerializationTest1()
        {
            ItemManager.Initial(new TestItemManager());
            ItemManager.Instance.AddItem(new Item(1, "TestItem 1", "xxx"));

            QuestRequirement requirement = new SendMessageToDifferentOnlineFriendInTheSameOceanQuestRequirement(1, 2);
            Quest quest = new Quest(1, QuestType.SendMessage, "Test", new List<QuestRequirement>
            {
                requirement
            },
            new List<QuestReward>
            {
                new GiveItemQuestReward(1, 1, 1)
            },
            "TestSendMessageToDifferentOnlineFriendQuest");

            byte[] serializedData = SerializationHelper.TypeSerialize(quest);
            Assert.IsNotNull(serializedData);
            Quest deserializadQuest = SerializationHelper.TypeDeserialize<Quest>(serializedData);
            Assert.IsNotNull(deserializadQuest);
            List<QuestRequirement> originRequirements = quest.Requirements.ToList(), deserializedRequirements = deserializadQuest.Requirements.ToList();
            Assert.AreEqual(originRequirements.Count, deserializedRequirements.Count);
            for(int i = 0; i < originRequirements.Count; i++)
            {
                Assert.AreEqual(originRequirements[i].QuestRequirementID, deserializedRequirements[i].QuestRequirementID);
                Assert.AreEqual(originRequirements[i].Description, deserializedRequirements[i].Description);
            }
            List<QuestReward> originRewards = quest.Rewards.ToList(), deserializedRewards = deserializadQuest.Rewards.ToList();
            Assert.AreEqual(originRewards.Count, deserializedRewards.Count);
            for (int i = 0; i < originRewards.Count; i++)
            {
                Assert.AreEqual(originRewards[i].QuestRewardID, deserializedRewards[i].QuestRewardID);
                Assert.AreEqual(originRewards[i].Description, deserializedRewards[i].Description);
            }
        }
        [TestMethod]
        public void QuestRecordSerializationTest1()
        {
            ItemManager.Initial(new TestItemManager());
            ItemManager.Instance.AddItem(new Item(1, "TestItem 1", "xxx"));

            QuestRequirement requirement = new SendMessageToDifferentOnlineFriendInTheSameOceanQuestRequirement(1, 2);
            Quest quest = new Quest(1, QuestType.SendMessage, "Test", new List<QuestRequirement>
            {
                requirement
            },
            new List<QuestReward>
            {
                new GiveItemQuestReward(1, 1, 1)
            },
            "TestSendMessageToDifferentOnlineFriendQuest");
            Player player1 = new Player(1, 0, "TestPlayer 1", "xx", GroupType.No, null);
            QuestRecord record = new QuestRecord(1, player1.PlayerID, quest, new List<QuestRequirementRecord>()
            {
                new SendMessageToDifferentOnlineFriendTheSameOceanQuestRequirementRecord(1, requirement, new HashSet<int> { 2, 3 })
            });
            record.RegisterObserverEvents(player1);

            byte[] serializedData = SerializationHelper.TypeSerialize(record);
            Assert.IsNotNull(serializedData);
            QuestRecord deserializadQuestRecord = SerializationHelper.TypeDeserialize<QuestRecord>(serializedData);
            Assert.IsNotNull(deserializadQuestRecord);
            List<QuestRequirementRecord> originRequirementRecords = record.RequirementRecords.ToList(), deserializedRequirementRecords = deserializadQuestRecord.RequirementRecords.ToList();
            Assert.AreEqual(originRequirementRecords.Count, deserializedRequirementRecords.Count);
            for (int i = 0; i < originRequirementRecords.Count; i++)
            {
                Assert.AreEqual(originRequirementRecords[i].QuestRequirementRecordID, deserializedRequirementRecords[i].QuestRequirementRecordID);
                Assert.AreEqual(originRequirementRecords[i].ProgressStatus, deserializedRequirementRecords[i].ProgressStatus);
            }
        }
    }
}
