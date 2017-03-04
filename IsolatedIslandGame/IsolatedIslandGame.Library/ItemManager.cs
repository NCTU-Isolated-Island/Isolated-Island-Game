﻿using System.Collections.Generic;
using System;
using IsolatedIslandGame.Library.Items;

namespace IsolatedIslandGame.Library
{
    public abstract class ItemManager
    {
        public static ItemManager Instance { get; private set; }
        public static void Initial(ItemManager manager)
        {
            Instance = manager;
        }

        protected Dictionary<int, Item> itemDictionary;
        public IEnumerable<Item> Items { get { return itemDictionary.Values; } }
        public int ItemCount { get { return itemDictionary.Count; } }
        public TimeSpan NextDrawMaterialTimeSpan { get; protected set; }
        public DateTime NextDrawMaterialTime { get; protected set; }

        public abstract event Action<Item> OnItemUpdate;

        protected ItemManager()
        {
            itemDictionary = new Dictionary<int, Item>();
        }
        public bool ContainsItem(int itemID)
        {
            return itemDictionary.ContainsKey(itemID);
        }
        public abstract bool FindItem(int itemID, out Item item);
        public abstract void AddItem(Item item);
        public abstract bool SpecializeItemToMaterial(int itemID, out Material material);
        public bool RemoveItem(int itemID)
        {
            return itemDictionary.Remove(itemID);
        }
    }
}
