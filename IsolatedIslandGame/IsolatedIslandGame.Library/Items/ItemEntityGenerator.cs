using System;
using System.Collections.Generic;
using System.Linq;

namespace IsolatedIslandGame.Library.Items
{
    public class ItemEntityGenerator
    {
        public int ItemEntityGeneratorID { get; private set; }
        public float PositionX { get; private set; }
        public float PositionZ { get; private set; }
        public float GeneratingRadius { get; private set; }
        public TimeSpan GeneratingPeriod { get; private set; }
        public float GeneratingProbability { get; private set; }
        public delegate bool ItemEntityCreateFynction(int itemID, float positionX, float positionZ, out ItemEntity itemEntity);
        private ItemEntityCreateFynction itemEntityCreateFynction;
        private List<ItemEntityGeneratingFactor> itemEntityGeneratingFactors = new List<ItemEntityGeneratingFactor>();

        public ItemEntityGenerator(int itemEntityGeneratorID, float positionX, float positionZ, float generatingRadius, TimeSpan generatingPeriod, float generatingProbability, ItemEntityCreateFynction itemEntityCreateFynction)
        {
            ItemEntityGeneratorID = itemEntityGeneratorID;
            PositionX = positionX;
            PositionZ = positionZ;
            GeneratingRadius = generatingRadius;
            GeneratingPeriod = generatingPeriod;
            GeneratingProbability = generatingProbability;
            this.itemEntityCreateFynction = itemEntityCreateFynction;
        }

        public void LoadItemEntityGeneratingFactors(List<ItemEntityGeneratingFactor> itemEntityGeneratingFactors)
        {
            foreach (var factor in itemEntityGeneratingFactors)
            {
                this.itemEntityGeneratingFactors.Add(factor);
            }
        }

        public void GenerateItemEntityRoutine()
        {
            Random randomGenerator = new Random(Guid.NewGuid().GetHashCode());
            if(randomGenerator.NextDouble() < GeneratingProbability)
            {
                GenerateItemEntity();
            }
            Scheduler.Instance.AddTask(DateTime.Now + GeneratingPeriod, GenerateItemEntityRoutine);
        }

        private void GenerateItemEntity()
        {
            int totalWeights = itemEntityGeneratingFactors.Sum(x => x.GeneratingWeight);
            Random randomGenerator = new Random(Guid.NewGuid().GetHashCode());
            int randomWeight = randomGenerator.Next(0, totalWeights);
            foreach(var factor in itemEntityGeneratingFactors)
            {
                if(randomWeight < factor.GeneratingWeight)
                {
                    float radius = (float)randomGenerator.NextDouble() * GeneratingRadius;
                    float radians = (float)(randomGenerator.NextDouble() * Math.PI * 2);
                    float generatingPositionX = (float)(PositionX + radius * Math.Cos(radians));
                    float generatingPositionY = (float)(PositionZ + radius * Math.Sin(radians));
                    ItemEntity itemEntity;
                    if(itemEntityCreateFynction(factor.GeneratingItemID, generatingPositionX, generatingPositionY, out itemEntity))
                    {
                        ItemEntityManager.Instance.AddItemEntity(itemEntity);
                    }
                    break;
                }
                else
                {
                    randomWeight -= factor.GeneratingWeight;
                }
            }
        }
    }
}
