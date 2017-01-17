﻿using IsolatedIslandGame.Database.Repositories;

namespace IsolatedIslandGame.Database
{
    public abstract class RepositoryList
    {
        public abstract PlayerRepository PlayerRepository { get; }
        public abstract ItemRepository ItemRepository { get; }
        public abstract InventoryRepository InventoryRepository { get; }
        public abstract InventoryItemInfoRepository InventoryItemInfoRepository { get; }
        public abstract VesselRepository VesselRepository { get; }
        public abstract DecorationRepository DecorationRepository { get; }
        public abstract BlueprintRepository BlueprintRepository { get; }
        public abstract BlueprintRequirementRepository BlueprintRequirementRepository { get; }
        public abstract BlueprintProductRepository BlueprintProductRepository { get; }
        public abstract PlayerKnownBlueprintRepository PlayerKnownBlueprintRepository { get; }
        public abstract FriendRepository FriendRepository { get; }
        public abstract PlayerMessageRepository PlayerMessageRepository { get; }
        public abstract PlayerConversationRepository PlayerConversationRepository { get; }
    }
}
