using IsolatedIslandGame.Database;
using IsolatedIslandGame.Library;
using System.Collections.Generic;

namespace IsolatedIslandGame.Server
{
    public class TransactionManager
    {
        public static TransactionManager Instance { get; private set; }

        public static void InitialManager()
        {
            Instance = new TransactionManager();
        }

        private Dictionary<int, Transaction> transactionDictionary = new Dictionary<int, Transaction>();

        public bool CreateTransaction(int requesterPlayerID, int accepterPlayerID, out Transaction transaction)
        {
            Player requester, accepter;
            if (PlayerFactory.Instance.FindPlayer(requesterPlayerID, out requester) && PlayerFactory.Instance.FindPlayer(accepterPlayerID, out accepter))
            {
                if (DatabaseService.RepositoryList.TransactionRepository.Create(requesterPlayerID, accepterPlayerID, out transaction))
                {
                    transactionDictionary.Add(transaction.TransactionID, transaction);
                    AssemblyTransaction(transaction, requester, accepter);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                transaction = null;
                return false;
            }
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
        public void EndTransaction(int transactionID, bool isEndSuccessful)
        {
            Transaction transaction;
            if(FindTransaction(transactionID, out transaction))
            {
                Player requester, accepter;
                if (PlayerFactory.Instance.FindPlayer(transaction.RequesterPlayerID, out requester) && PlayerFactory.Instance.FindPlayer(transaction.AccepterPlayerID, out accepter))
                {
                    if (isEndSuccessful)
                    {
                        #region change inventory
                        lock (requester.Inventory)
                        {
                            lock (accepter.Inventory)
                            {
                                bool inventoryCheck = true;
                                foreach (var info in transaction.RequesterTransactionItemInfos)
                                {
                                    if (!requester.Inventory.RemoveItemCheck(info.Item.ItemID, info.Count))
                                    {
                                        inventoryCheck = false;
                                        break;
                                    }
                                    if (accepter.Inventory.AddItemCheck(info.Item.ItemID, info.Count))
                                    {
                                        inventoryCheck = false;
                                        break;
                                    }
                                }
                                if (inventoryCheck)
                                {
                                    foreach (var info in transaction.AccepterTransactionItemInfos)
                                    {
                                        if (accepter.Inventory.RemoveItemCheck(info.Item.ItemID, info.Count))
                                        {
                                            inventoryCheck = false;
                                            break;
                                        }
                                        if (requester.Inventory.AddItemCheck(info.Item.ItemID, info.Count))
                                        {
                                            inventoryCheck = false;
                                            break;
                                        }
                                    }
                                }
                                if (inventoryCheck)
                                {
                                    foreach (var info in transaction.RequesterTransactionItemInfos)
                                    {
                                        requester.Inventory.RemoveItem(info.Item.ItemID, info.Count);
                                        accepter.Inventory.AddItem(info.Item, info.Count);
                                    }
                                    foreach (var info in transaction.AccepterTransactionItemInfos)
                                    {
                                        accepter.Inventory.RemoveItem(info.Item.ItemID, info.Count);
                                        requester.Inventory.AddItem(info.Item, info.Count);
                                    }
                                    transaction.EndTransaction(true);
                                    DatabaseService.RepositoryList.TransactionRepository.Save(transaction);
                                }
                                else
                                {
                                    transaction.EndTransaction(false);
                                }
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        transaction.EndTransaction(false);
                    }
                    DisassemblyTransaction(transaction, requester, accepter);
                }
                else
                {
                    transaction.EndTransaction(false);
                }
                transactionDictionary.Remove(transactionID);
            }
        }
        private void CheckTransactionEnd(int transactionID, int playerID)
        {
            Transaction transaction;
            if (FindTransaction(transactionID, out transaction))
            {
                if (transaction.IsRequesterConfirmed && transaction.IsAccepterConfirmed)
                {
                    EndTransaction(transactionID, true);
                }
            }
        }
        private void AssemblyTransaction(Transaction transaction, Player requester, Player accepter)
        {
            transaction.OnTransactionConfirmed += requester.EventManager.SyncDataResolver.SyncTransactionConfirm;
            transaction.OnTransactionConfirmed += accepter.EventManager.SyncDataResolver.SyncTransactionConfirm;

            transaction.OnTransactionItemChange += requester.EventManager.SyncDataResolver.SyncTransactionItemChange;
            transaction.OnTransactionItemChange += accepter.EventManager.SyncDataResolver.SyncTransactionItemChange;

            transaction.OnTransactionEnd += requester.EventManager.EndTransaction;
            transaction.OnTransactionEnd += accepter.EventManager.EndTransaction;

            transaction.OnTransactionConfirmed += CheckTransactionEnd;

            requester.AddTransaction(transaction);
            accepter.AddTransaction(transaction);
        }
        private void DisassemblyTransaction(Transaction transaction, Player requester, Player accepter)
        {
            transaction.OnTransactionConfirmed -= requester.EventManager.SyncDataResolver.SyncTransactionConfirm;
            transaction.OnTransactionConfirmed -= accepter.EventManager.SyncDataResolver.SyncTransactionConfirm;

            transaction.OnTransactionItemChange -= requester.EventManager.SyncDataResolver.SyncTransactionItemChange;
            transaction.OnTransactionItemChange -= accepter.EventManager.SyncDataResolver.SyncTransactionItemChange;

            transaction.OnTransactionEnd -= requester.EventManager.EndTransaction;
            transaction.OnTransactionEnd -= accepter.EventManager.EndTransaction;

            transaction.OnTransactionConfirmed -= CheckTransactionEnd;

            requester.RemoveTransaction(transaction.TransactionID);
            accepter.RemoveTransaction(transaction.TransactionID);
        }
    }
}
