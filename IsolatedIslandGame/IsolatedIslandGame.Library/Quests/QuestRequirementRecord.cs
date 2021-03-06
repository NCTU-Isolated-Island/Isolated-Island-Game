﻿using System;

namespace IsolatedIslandGame.Library.Quests
{
    public abstract class QuestRequirementRecord
    {
        public int QuestRequirementRecordID { get; private set; }
        public QuestRequirement Requirement { get; private set; }
        public abstract string ProgressStatus { get; }
        public abstract bool IsSufficient { get; }
        public abstract event Action<QuestRequirementRecord> OnRequirementStatusChange;

        protected QuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement)
        {
            QuestRequirementRecordID = questRequirementRecordID;
            Requirement = requirement;
        }
        internal abstract void RegisterObserverEvents(Player player);
    }
}
