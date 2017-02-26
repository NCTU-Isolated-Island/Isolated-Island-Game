using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IsolatedIslandGame.Library
{
    public class Scheduler
    {
        public static Scheduler Instance { get; private set; }
        public static void Inital(TimeSpan tickSpan)
        {
            Instance = new Scheduler(tickSpan);
        }

        public TimeSpan TickSpan { get; private set; }
        private SortedDictionary<DateTime, Action> tasks = new SortedDictionary<DateTime, Action>();
        private Timer timer;

        private Scheduler(TimeSpan tickSpan)
        {
            TickSpan = tickSpan;
            timer = new Timer(new TimerCallback(Handler));
            timer.Change(TimeSpan.Zero, tickSpan);
        }

        private void Handler(object timerState)
        {
            lock(tasks)
            {
                while (tasks.Any() && DateTime.Now >= tasks.First().Key)
                {
                    var firstTask = tasks.First();
                    firstTask.Value();
                    tasks.Remove(firstTask.Key);
                }
            }
        }

        public void AddTask(DateTime timestamp, Action task)
        {
            lock (tasks)
            {
                while(tasks.ContainsKey(timestamp))
                {
                    timestamp += TimeSpan.MinValue;
                }
                tasks.Add(timestamp, task);
            }
        }
    }
}
