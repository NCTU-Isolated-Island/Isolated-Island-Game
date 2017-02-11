using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Managers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Managers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Managers;
using IsolatedIslandGame.Library.Quests;
using IsolatedIslandGame.Library.TextData;
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
        private DateTime nextDrawMaterialTime;
        public DateTime NextDrawMaterialTime
        {
            get
            {
                return nextDrawMaterialTime;
            }
            set
            {
                nextDrawMaterialTime = value;
                onNextDrawMaterialTimeUpdated?.Invoke(nextDrawMaterialTime);
            }
        }

        private Dictionary<int, Blueprint> knownBlueprintDictionary = new Dictionary<int, Blueprint>();
        public IEnumerable<Blueprint> KnownBlueprints { get { return knownBlueprintDictionary.Values.ToArray(); } }

        private HashSet<int> knownPlayerIDSet = new HashSet<int>();

        private Dictionary<int, FriendInformation> friendInformationDictionary = new Dictionary<int, FriendInformation>();
        public IEnumerable<FriendInformation> FriendInformations { get { return friendInformationDictionary.Values.ToArray(); } }

        private Dictionary<int, Transaction> transactionDictionary = new Dictionary<int, Transaction>();
        public IEnumerable<Transaction> Transactions { get { return transactionDictionary.Values.ToArray(); } }

        private Dictionary<int, QuestRecord> questRecordDictionary = new Dictionary<int, QuestRecord>();
        public IEnumerable<QuestRecord> QuestRecords { get { return questRecordDictionary.Values.ToArray(); } }

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

        private event Action<PlayerInformation> onGetPlayerInformation;
        public event Action<PlayerInformation> OnGetPlayerInformation { add { onGetPlayerInformation += value; } remove { onGetPlayerInformation -= value; } }

        public delegate void FriendInformationChangeEventHandler(DataChangeType changeType, FriendInformation information);
        private event FriendInformationChangeEventHandler onFriendInformationChange;
        public event FriendInformationChangeEventHandler OnFriendInformationChange { add { onFriendInformationChange += value; } remove { onFriendInformationChange -= value; } }

        private event Action<PlayerConversation> onGetPlayerConversation;
        public event Action<PlayerConversation> OnGetPlayerConversation { add { onGetPlayerConversation += value; } remove { onGetPlayerConversation -= value; } }

        private event Action<int> onTransactionRequest;
        public event Action<int> OnTransactionRequest { add { onTransactionRequest += value; } remove { onTransactionRequest -= value; } }

        private event Action<Transaction> onTransactionStart;
        public event Action<Transaction> OnTransactionStart { add { onTransactionStart += value; } remove { onTransactionStart -= value; } }

        private event Action<QuestRecord> onQuestRecordUpdated;
        public event Action<QuestRecord> OnQuestRecordUpdated { add { onQuestRecordUpdated += value; } remove { onQuestRecordUpdated -= value; } }

        private event Action<string> onScanQR_Code;
        public event Action<string> OnScanQR_Code { add { onScanQR_Code += value; } remove { onScanQR_Code -= value; } }

        private event Action<DateTime> onNextDrawMaterialTimeUpdated;
        public event Action<DateTime> OnNextDrawMaterialTimeUpdated { add { onNextDrawMaterialTimeUpdated += value; } remove { onNextDrawMaterialTimeUpdated -= value; } }
        #endregion

        public Player(int playerID, ulong facebookID, string nickname, string signature, GroupType groupType, IPAddress lastConnectedIPAddress, DateTime nextDrawMaterialTime)
        {
            PlayerID = playerID;
            FacebookID = facebookID;
            Nickname = nickname;
            Signature = signature;
            GroupType = groupType;
            LastConnectedIPAddress = lastConnectedIPAddress;
            NextDrawMaterialTime = nextDrawMaterialTime;

            EventManager = new PlayerEventManager(this);
            OperationManager = new PlayerOperationManager(this);
            ResponseManager = new PlayerResponseManager(this);
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

        public void SyncPlayerInformation(int playerID)
        {
            if(!knownPlayerIDSet.Contains(playerID))
            {
                PlayerInformation playerInformation;
                if(PlayerInformationManager.Instance.FindPlayerInformation(playerID, out playerInformation))
                {
                    knownPlayerIDSet.Add(playerID);
                    onGetPlayerInformation?.Invoke(playerInformation);
                }
            }
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
        public bool FindFriend(int friendPlayerID, out FriendInformation information)
        {
            if(ContainsFriend(friendPlayerID))
            {
                information = friendInformationDictionary[friendPlayerID];
                return true;
            }
            else
            {
                information = new FriendInformation();
                return false;
            }
        }
        public void AddFriend(FriendInformation information)
        {
            if(ContainsFriend(information.friendPlayerID))
            {
                friendInformationDictionary[information.friendPlayerID] = information;
                onFriendInformationChange?.Invoke(DataChangeType.Update, information);
            }
            else
            {
                friendInformationDictionary.Add(information.friendPlayerID, information);
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
        public void GetPlayerConversation(PlayerConversation conversation)
        {
            SyncPlayerInformation(conversation.message.senderPlayerID);
            SyncPlayerInformation(conversation.receiverPlayerID);
            onGetPlayerConversation?.Invoke(conversation);
        }

        public void TransactionRequest(int requesterPlayerID)
        {
            SyncPlayerInformation(requesterPlayerID);
            onTransactionRequest?.Invoke(requesterPlayerID);
        }
        public bool ContainsTransaction(int transactionID)
        {
            return transactionDictionary.ContainsKey(transactionID);
        }
        public bool FindTransaction(int transactionID, out Transaction transaction)
        {
            if(ContainsTransaction(transactionID))
            {
                transaction = transactionDictionary[transactionID];
                return true;
            }
            else
            {
                transaction = null;
                return false;
            }
        }
        public void AddTransaction(Transaction transaction)
        {
            if(!ContainsTransaction(transaction.TransactionID))
            {
                transactionDictionary.Add(transaction.TransactionID, transaction);
                onTransactionStart?.Invoke(transaction);
            }
        }
        public void RemoveTransaction(int transactionID)
        {
            if (ContainsTransaction(transactionID))
            {
                transactionDictionary.Remove(transactionID);
            }
        }

        public bool ContainsQuestRecord(int questRecordID)
        {
            return questRecordDictionary.ContainsKey(questRecordID);
        }
        public bool FindQuestRecord(int questRecordID, out QuestRecord questRecord)
        {
            if (ContainsQuestRecord(questRecordID))
            {
                questRecord = questRecordDictionary[questRecordID];
                return true;
            }
            else
            {
                questRecord = null;
                return false;
            }
        }
        public void AddQuestRecord(QuestRecord questRecord)
        {
            if (ContainsTransaction(questRecord.QuestRecordID))
            {
                questRecordDictionary[questRecord.QuestRecordID] = questRecord;
                onQuestRecordUpdated?.Invoke(questRecord);
            }
            else
            {
                questRecordDictionary.Add(questRecord.QuestRecordID, questRecord);
                onQuestRecordUpdated?.Invoke(questRecord);
            }
        }
        public void ScanQR_Code(string qrCodeString)
        {
            onScanQR_Code?.Invoke(qrCodeString);
        }
    }
}
