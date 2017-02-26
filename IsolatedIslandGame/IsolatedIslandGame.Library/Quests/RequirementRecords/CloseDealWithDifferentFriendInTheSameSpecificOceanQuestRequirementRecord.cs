﻿using IsolatedIslandGame.Library.Quests.Requirements;
using IsolatedIslandGame.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class CloseDealWithDifferentFriendInTheSameSpecificOceanQuestRequirementRecord : QuestRequirementRecord
    {
        private HashSet<int> friendPlayerID_Set = new HashSet<int>();
        public IEnumerable<int> OnlineFriendPlayerIDs { get { return friendPlayerID_Set.ToArray(); } }
        public override bool IsSufficient
        {
            get
            {
                return friendPlayerID_Set.Count >= (Requirement as CloseDealWithDifferentFriendInTheSameSpecificOceanQuestRequirement).RequiredFriendNumber;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                return $"人數： {friendPlayerID_Set.Count}/{(Requirement as CloseDealWithDifferentFriendInTheSameSpecificOceanQuestRequirement).RequiredFriendNumber}";
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        public CloseDealWithDifferentFriendInTheSameSpecificOceanQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement, HashSet<int> friendPlayerID_Set) : base(questRequirementRecordID, requirement)
        {
            this.friendPlayerID_Set = friendPlayerID_Set;
        }
        internal override void RegisterObserverEvents(Player player)
        {
            player.OnTransactionStart += (transaction) =>
            {
                transaction.OnTransactionEnd += (transactionID, isSuccessful) =>
                {
                    if(!IsSufficient && isSuccessful)
                    {
                        int theOtherPlayerID = (transaction.RequesterPlayerID == player.PlayerID) ? transaction.AccepterPlayerID : transaction.RequesterPlayerID;
                        FriendInformation info;
                        Vessel friendVessel;
                        OceanType specificOceanType = (Requirement as CloseDealWithDifferentFriendInTheSameSpecificOceanQuestRequirement).SpecificOceanType;
                        if (player.FindFriend(theOtherPlayerID, out info) &&
                            info.isConfirmed &&
                            VesselManager.Instance.FindVesselByOwnerPlayerID(theOtherPlayerID, out friendVessel) &&
                            player.Vessel.LocatedOceanType == specificOceanType &&
                            friendVessel.LocatedOceanType == specificOceanType)
                        {
                            lock (friendPlayerID_Set)
                            {
                                if (!friendPlayerID_Set.Contains(theOtherPlayerID))
                                {
                                    if (friendPlayerID_Set.Add(theOtherPlayerID) && (QuestRecordFactory.Instance == null || QuestRecordFactory.Instance.AddPlayerIDToCloseDealWithDifferentFriendInTheSameOceanQuestRequirementRecord(QuestRequirementRecordID, theOtherPlayerID)))
                                    {
                                        onRequirementStatusChange?.Invoke(this);
                                    }
                                }
                            }
                        }
                    }
                };
            };
        }
    }
}
