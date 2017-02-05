using IsolatedIslandGame.Library.Quests;
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
            QuestRequirement requirement = new SendMessageToDifferentOnlineFriendQuestRequirement(1, 2);
            Quest quest = new Quest(1, QuestType.SendMessage, "Test", new List<QuestRequirement>
            {
                requirement
            },
            new List<QuestReward>
            {
                new GiveItemQuestReward(1, new Item(1, "TestItem 1", "TestItem 1"), 1)
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
            VesselManager.Instance.AddVessel(new Vessel(1, 1, 0, 0, 0, OceanType.Unknown));
            VesselManager.Instance.AddVessel(new Vessel(2, 2, 0, 0, 0, OceanType.Unknown));
            VesselManager.Instance.AddVessel(new Vessel(3, 3, 0, 0, 0, OceanType.Unknown));

            QuestRecord record = new QuestRecord(1, player1.PlayerID, quest, new List<QuestRequirementRecord>()
            {
                new SendMessageToDifferentOnlineFriendQuestRequirementRecord(1, player1, requirement, new HashSet<int>())
            });
            record.OnQuestStatusChange += (recordState) =>
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
    }
}
