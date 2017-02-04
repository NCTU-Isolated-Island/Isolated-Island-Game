﻿using System;

namespace IsolatedIslandGame.Library.Quests
{
    public class QuestRecord
    {
        public Quest Quest { get; private set; }

        private bool isFinished;
        public bool IsFinished
        {
            get { return isFinished; }
            protected set
            {
                isFinished = value;
                onQuestFinished?.Invoke(this);
            }
        }

        private event Action<QuestRecord> onQuestFinished;
        public event Action<QuestRecord> OnQuestFinished { add { onQuestFinished += value; } remove { onQuestFinished -= value; } }

        protected QuestRecord(Quest quest, bool isFinished)
        {
            Quest = quest;
            IsFinished = isFinished;
        }
    }
}