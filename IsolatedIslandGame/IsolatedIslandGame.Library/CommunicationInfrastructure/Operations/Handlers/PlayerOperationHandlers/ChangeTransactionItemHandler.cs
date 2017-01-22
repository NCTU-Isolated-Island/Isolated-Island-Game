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
                            if (subject.User.CommunicationInterface.ChangeTransactionItem(subject.PlayerID, transactionID, changeType, new TransactionItemInfo(item, itemCount, positionIndex)))
                            {
                                SendResponse(operationCode, new Dictionary<byte, object>());
                                LogService.InfoFormat($"Player: {subject.IdentityInformation}, ChangeTransactionItem, TransactionID: {transactionID}, DataChangeType: {changeType}, ItemID: {itemID}, ItemCount: {itemCount}, PositionIndex: {positionIndex}");
                                return true;
                            }
                            else
                            {
                                errorCode = ErrorCode.Fail;
                                debugMessage = "ChangeTransactionItem Fail";
                                SendError(operationCode, errorCode, debugMessage);
                                LogService.ErrorFormat($"Player: {subject.IdentityInformation}, ChangeTransactionItem Fail, TransactionID: {transactionID}, ChangeTransactionItem, TransactionID: {transactionID}, DataChangeType: {changeType}, ItemID: {itemID}, ItemCount: {itemCount}, PositionIndex: {positionIndex}");
                                return false;
                            }
                        }
                        else
                        {
                            errorCode = ErrorCode.NotEnough;
                            debugMessage = "ChangeTransactionItem ItemCount NotEnough";
                            SendError(operationCode, errorCode, debugMessage);
                            LogService.ErrorFormat($"Player: {subject.IdentityInformation}, ChangeTransactionItem ItemCount NotEnough, TransactionID: {transactionID}, ChangeTransactionItem, TransactionID: {transactionID}, DataChangeType: {changeType}, ItemID: {itemID}, ItemCount: {itemCount}, PositionIndex: {positionIndex}");
                            return false;
                        }
                    }
                }
                else
                {
                    errorCode = ErrorCode.NotExist;
                    debugMessage = "ChangeTransactionItem Item NotExist";
                    SendError(operationCode, errorCode, debugMessage);
                    LogService.ErrorFormat($"Player: {subject.IdentityInformation}, ChangeTransactionItem Item NotExist, TransactionID: {transactionID}, ChangeTransactionItem, TransactionID: {transactionID}, DataChangeType: {changeType}, ItemID: {itemID}, ItemCount: {itemCount}, PositionIndex: {positionIndex}");
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
