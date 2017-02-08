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

    private void Awake()
    {
        itemImage = transform.Find("ItemImage").GetComponent<Image>();
        itemNameText = transform.Find("ItemNameText").GetComponent<Text>();
        itemCountText = transform.Find("ItemCountText").GetComponent<Text>();
    }
    public void Initial(InventoryItemInfo itemInfo)
    {
        ItemInfo = itemInfo;
        itemImage.sprite = Resources.Load<Sprite>("2D/" + ItemInfo.Item.ItemID);
        itemNameText.text = ItemInfo.Item.ItemName;
        itemCountText.text = ItemInfo.Count.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        InventoryPanel.Instance.ShowItemInfoDetail(ItemInfo);
    }
}
