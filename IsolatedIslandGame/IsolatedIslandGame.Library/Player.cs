using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Managers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Managers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Managers;
using IsolatedIslandGame.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace IsolatedIslandGame.Library
{
    public class Player : IIdentityProvidable
    {
        #region properties
        public User User { get; private set; }
        public int PlayerID { get; private set; }
        public ulong FacebookID { get; private set; }
        public string Nickname { get; private set; }
        public string Signature { get; private set; }
        public GroupType GroupType { get; private set; }
        public IPAddress LastConnectedIPAddress { get; set; }
        public string IdentityInformation { get { return string.Format("Player ID: {0}", PlayerID); } }
        public Inventory Inventory { get; private set; }
        public Vessel Vessel { get; private set; }
        private Dictionary<int, Blueprint> knownBlueprintDictionary;
        public IEnumerable<Blueprint> KnownBlueprints { get { return knownBlueprintDictionary.Values; } }
        private Dictionary<int, FriendInformation> friendInformationDictionary;
        public IEnumerable<FriendInformation> FriendInformations { get { return friendInformationDictionary.Values.ToArray(); } }

        public PlayerEventManager EventManager { get; private set; }
        public PlayerOperationManager OperationManager { get; private set; }
        public PlayerResponseManager ResponseManager { get; private set; }
        #endregion

        #region events
        private event Action<Player> onCreateCharacter;
        public event Action<Player> OnCreateCharacter { add { onCreateCharacter += value; } remove { onCreateCharacter -= value; } }

        private event Action<Inventory> onBindInventory;
        public event Action<Inventory> OnBindInventory { add { onBindInventory += value; } remove { onBindInventory -= value; } }

        private event Action<Vessel> onBindVessel;
        public event Action<Vessel> OnBindVessel { add { onBindVessel += value; } remove { onBindVessel -= value; } }

        private event Action<Blueprint> onGetBlueprint;
        public event Action<Blueprint> OnGetBlueprint { add { onGetBlueprint += value; } remove { onGetBlueprint -= value; } }

        public delegate void FriendInformationChangeEventHandler(DataChangeType changeType, FriendInformation information);
        private event FriendInformationChangeEventHandler onFriendInformationChange;
        public event FriendInformationChangeEventHandler OnFriendInformationChange { add { onFriendInformationChange += value; } remove { onFriendInformationChange -= value; } }
        #endregion

        public Player(int playerID, ulong facebookID, string nickname, string signature, GroupType groupType, IPAddress lastConnectedIPAddress)
        {
            PlayerID = playerID;
            FacebookID = facebookID;
            Nickname = nickname;
            Signature = signature;
            GroupType = groupType;
            LastConnectedIPAddress = lastConnectedIPAddress;

            EventManager = new PlayerEventManager(this);
            OperationManager = new PlayerOperationManager(this);
            ResponseManager = new PlayerResponseManager(this);

            knownBlueprintDictionary = new Dictionary<int, Blueprint>();
            friendInformationDictionary = new Dictionary<int, FriendInformation>();
        }
        public void BindUser(User user)
        {
            User = user;
        }
        public void BindInventory(Inventory inventory)
        {
            Inventory = inventory;
            onBindInventory?.Invoke(Inventory);
        }
        public void BindVessel(Vessel vessel)
        {
            if(vessel.OwnerPlayerID == PlayerID)
            {
                Vessel = vessel;
                onBindVessel?.Invoke(Vessel);
            }
        }
        public void CreateCharacter(string nickname, string signature, GroupType groupType)
        {
            Nickname = nickname;
            Signature = signature;
            GroupType = groupType;
            onCreateCharacter?.Invoke(this);
        }

        public bool IsKnownBlueprint(int blueprintID)
        {
            return knownBlueprintDictionary.ContainsKey(blueprintID);
        }
        public void GetBlueprint(Blueprint blueprint)
        {
            if (!IsKnownBlueprint(blueprint.BlueprintID))
            {
                knownBlueprintDictionary.Add(blueprint.BlueprintID, blueprint);
                onGetBlueprint?.Invoke(blueprint);
            }
        }

        public bool ContainsFriend(int friendPlayerID)
        {
            return friendInformationDictionary.ContainsKey(friendPlayerID);
        }
        public void AddFriend(FriendInformation information)
        {
            if(ContainsFriend(information.playerID))
            {
                friendInformationDictionary[information.playerID] = information;
                onFriendInformationChange?.Invoke(DataChangeType.Update, information);
            }
            else
            {
                friendInformationDictionary.Add(information.playerID, information);
                onFriendInformationChange?.Invoke(DataChangeType.Add, information);
            }
        }
        public void RemoveFriend(int friendPlayerID)
        {
            if(ContainsFriend(friendPlayerID))
            {
                FriendInformation information = friendInformationDictionary[friendPlayerID];
                friendInformationDictionary.Remove(friendPlayerID);
                onFriendInformationChange?.Invoke(DataChangeType.Remove, information);
            }
        }
    }
}
