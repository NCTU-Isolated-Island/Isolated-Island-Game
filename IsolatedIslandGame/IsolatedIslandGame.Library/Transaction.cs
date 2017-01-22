﻿using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using System.Collections.Generic;
using System.Linq;

namespace IsolatedIslandGame.Library
{
    public class Transaction
    {
        public int TransactionID { get; private set; }
        public int RequesterPlayerID { get; private set; }
        public int AccepterPlayerID { get; private set; }
        public bool IsRequesterConfirmed { get; private set; } = false;
        public bool IsAccepterConfirmed { get; private set; } = false;
        private TransactionItemInfo[] requesterTransactionItemInfos = new TransactionItemInfo[6];
        private TransactionItemInfo[] accepterTransactionItemInfos = new TransactionItemInfo[6];
        public IEnumerable<TransactionItemInfo> RequesterTransactionItemInfos { get { return requesterTransactionItemInfos.ToArray(); } }
        public IEnumerable<TransactionItemInfo> AccepterTransactionItemInfos { get { return accepterTransactionItemInfos.ToArray(); } }

        public delegate void TransactionConfirmedEventHandler(int transactionID, int playerID);
        private event TransactionConfirmedEventHandler onTransactionConfirmed;
        public event TransactionConfirmedEventHandler OnTransactionConfirmed { add { onTransactionConfirmed += value; } remove { onTransactionConfirmed -= value; } }

        public delegate void TransactionItemChangeEventHandler(int transactionID, int playerID, DataChangeType changeType, TransactionItemInfo info);
        private event TransactionItemChangeEventHandler onTransactionItemChange;
        public event TransactionItemChangeEventHandler OnTransactionItemChange { add { onTransactionItemChange += value; } remove { onTransactionItemChange -= value; } }

        public delegate void TransactionEndEventHandler(int transactionID, bool isSuccessful);
        private event TransactionEndEventHandler onTransactionEnd;
        public event TransactionEndEventHandler OnTransactionEnd { add { onTransactionEnd += value; } remove { onTransactionEnd -= value; } }

        public Transaction(int transactionID, int requesterPlayerID, int accepterPlayerID)
        {
            TransactionID = transactionID;
            RequesterPlayerID = requesterPlayerID;
            AccepterPlayerID = accepterPlayerID;
        }
        public bool Confirm(int playerID)
        {
            if(RequesterPlayerID == playerID)
            {
                IsRequesterConfirmed = true;
                onTransactionConfirmed?.Invoke(TransactionID, playerID);
                return true;
            }
            else if(AccepterPlayerID == playerID)
            {
                IsAccepterConfirmed = true;
                onTransactionConfirmed?.Invoke(TransactionID, playerID);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool ChangeTransactionItem(int playerID, DataChangeType changeType, TransactionItemInfo info)
        {
            if (info.PositionIndex < 0 || info.PositionIndex > 5)
                return false;
            if (RequesterPlayerID == playerID && !IsRequesterConfirmed)
            {
                switch(changeType)
                {
                    case DataChangeType.Add:
                    case DataChangeType.Update:
                        requesterTransactionItemInfos[info.PositionIndex] = info;
                        break;
                    case DataChangeType.Remove:
                        requesterTransactionItemInfos[info.PositionIndex] = null;
                        break;
                }
                onTransactionItemChange?.Invoke(TransactionID, playerID, changeType, info);
                return true;
            }
            else if (AccepterPlayerID == playerID && !IsAccepterConfirmed)
            {
                switch (changeType)
                {
                    case DataChangeType.Add:
                    case DataChangeType.Update:
                        accepterTransactionItemInfos[info.PositionIndex] = info;
                        break;
                    case DataChangeType.Remove:
                        accepterTransactionItemInfos[info.PositionIndex] = null;
                        break;
                }
                onTransactionItemChange?.Invoke(TransactionID, playerID, changeType, info);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void EndTransaction(bool isEndSuccessful)
        {
            if(isEndSuccessful && IsRequesterConfirmed && IsAccepterConfirmed)
            {
                onTransactionEnd?.Invoke(TransactionID, true);
            }
            else
            {
                onTransactionEnd?.Invoke(TransactionID, false);
            }
        }
    }
}