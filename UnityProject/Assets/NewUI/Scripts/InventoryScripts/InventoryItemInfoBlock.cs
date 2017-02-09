using IsolatedIslandGame.Library.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemInfoBlock : MonoBehaviour, IPointerClickHandler
{
    public InventoryItemInfo ItemInfo { get; private set; }
    private Image itemImage;
    private Text itemNameText;
    private Text itemCountText;
    private InventoryPanel.InventoryUsageType usageType;

    private void Awake()
    {
        itemImage = transform.Find("ItemImage").GetComponent<Image>();
        itemNameText = transform.Find("ItemNameText").GetComponent<Text>();
        itemCountText = transform.Find("ItemCountText").GetComponent<Text>();
    }
    public void Initial(InventoryItemInfo itemInfo , InventoryPanel.InventoryUsageType usageType)
    {
        ItemInfo = itemInfo;
        itemImage.sprite = Resources.Load<Sprite>("2D/" + ItemInfo.Item.ItemID);
        itemNameText.text = ItemInfo.Item.ItemName;
        itemCountText.text = string.Format("x{0}", ItemInfo.Count);
        this.usageType = usageType;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (this.usageType)
        {
            case InventoryPanel.InventoryUsageType.CheckInventoryItemDetail:
                InventoryPanel.Instance.ShowItemInfoDetail(ItemInfo);
                break;
            case InventoryPanel.InventoryUsageType.PutInCombineSlot:
                CombineUIManager.Instance.PutInItemFromInventory(ItemInfo.Item);
                UIManager.Instance.ToPreviousPage();
                break;
            case InventoryPanel.InventoryUsageType.PutInTransactionSlot:
                throw new System.NotImplementedException("InventoryPanel.InventoryUsageType.PutInTransactionSlot");
                UIManager.Instance.ToPreviousPage();
                break;
            case InventoryPanel.InventoryUsageType.PutItemOnVessel:
                throw new System.NotImplementedException("InventoryPanel.InventoryUsageType.PutItemOnVessel");
                UIManager.Instance.ToPreviousPage();
                break;
        }
    }
}
