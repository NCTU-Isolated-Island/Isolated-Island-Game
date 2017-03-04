using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Library.Items;

public class TransactionSlotBehavior : MonoBehaviour {

    public Image itemImage;
    public Item item;
    public int amount;
    public Text amountText;

    public int posIndex;

    public void ToInventoryPageFromTransactionSlot()
    {
        TransactionManager.Instance.puttingIndex = posIndex;
        //TransactionManager.Instance.TransactionUIPage.SetActive(false);
        UIManager.Instance.SwapPage(UIManager.UIPageType.Inventory);
    }

    public void SetSlotItem(Item item)
    {
        this.item = item;
        //TransactionManager.Instance.TransactionUIPage.SetActive(true);
    }

    public void SetSlotAmount(int amount)
    {
        this.amount = amount;
        TransactionItemInfo info = new TransactionItemInfo(this.item , this.amount , this.posIndex);
        TransactionManager.Instance.UploadPutInItem(info);
    }

}
