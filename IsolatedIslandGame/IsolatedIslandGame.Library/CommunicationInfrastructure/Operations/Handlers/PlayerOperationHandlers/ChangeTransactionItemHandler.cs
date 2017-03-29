using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.Player;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers
{
    class ChangeTransactionItemHandler : PlayerOperationHandler
    {
        public ChangeTransactionItemHandler(Player subject) : base(subject, 5)
        {
        }

        internal override bool Handle(PlayerOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                string debugMessage;
                ErrorCode errorCode;
                int transactionID = (int)parameters[(byte)ChangeTransactionItemParameterCode.TransactionID];
                DataChangeType changeType = (DataChangeType)parameters[(byte)ChangeTransactionItemParameterCode.DataChangeType];
                int itemID = (int)parameters[(byte)ChangeTransactionItemParameterCode.ItemID];
                int itemCount = (int)parameters[(byte)ChangeTransactionItemParameterCode.ItemCount];
                int positionIndex = (int)parameters[(byte)ChangeTransactionItemParameterCode.PositionIndex];

                Item item;
                if(ItemManager.Instance.FindItem(itemID, out item))
                {
                    lock(subject.Inventory)
                    {
                        if (subject.Inventory.ItemCount(itemID) >= itemCount)
                        {
                            if (SystemManager.Instance.OperationInterface.ChangeTransactionItem(subject.PlayerID, transactionID, changeType, new TransactionItemInfo(item, itemCount, positionIndex)))
                            {
                                LogService.InfoFormat($"Player: {subject.IdentityInformation}, ChangeTransactionItem, TransactionID: {transactionID}, DataChangeType: {changeType}, ItemID: {itemID}, ItemCount: {itemCount}, PositionIndex: {positionIndex}");
                                return true;
                            }
                            else
                            {
                                LogService.ErrorFormat($"Player: {subject.IdentityInformation}, ChangeTransactionItem Fail, TransactionID: {transactionID}, ChangeTransactionItem, TransactionID: {transactionID}, DataChangeType: {changeType}, ItemID: {itemID}, ItemCount: {itemCount}, PositionIndex: {positionIndex}");
                                subject.User.EventManager.UserInform("失敗", "更改交易的物品失敗。");
                                return false;
                            }
                        }
                        else
                        {
                            LogService.ErrorFormat($"Player: {subject.IdentityInformation}, ChangeTransactionItem ItemCount NotEnough, TransactionID: {transactionID}, ChangeTransactionItem, TransactionID: {transactionID}, DataChangeType: {changeType}, ItemID: {itemID}, ItemCount: {itemCount}, PositionIndex: {positionIndex}");
                            subject.User.EventManager.UserInform("錯誤", "此交易的物品的數量不足。");
                            return false;
                        }
                    }
                }
                else
                {
                    LogService.ErrorFormat($"Player: {subject.IdentityInformation}, ChangeTransactionItem Item NotExist, TransactionID: {transactionID}, ChangeTransactionItem, TransactionID: {transactionID}, DataChangeType: {changeType}, ItemID: {itemID}, ItemCount: {itemCount}, PositionIndex: {positionIndex}");
                    subject.User.EventManager.UserInform("錯誤", "此交易的物品並不存在。");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
