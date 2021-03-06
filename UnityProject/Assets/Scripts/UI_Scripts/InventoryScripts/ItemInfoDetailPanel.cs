﻿using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoDetailPanel : MonoBehaviour
{
    public InventoryItemInfo ItemInfo { get; private set; }
    private Image itemImage;
	private Image itemGroupIcon;
    private Text itemNameText;
    private Text itemCountText;
    private Text itemDescriptionText;
    private Button synthesizeButton;
    private Button decorateVesselButton;
    private Button dropButton;
    private Button closeButton;

    private void Awake()
    {
        itemImage = transform.Find("ItemImage").GetComponent<Image>();
        itemNameText = transform.Find("ItemNameText").GetComponent<Text>();
        itemCountText = transform.Find("ItemCountText").GetComponent<Text>();
        itemDescriptionText = transform.Find("ItemDescriptionText").GetComponent<Text>();

		itemGroupIcon = transform.Find ("ItemGroupIcon").GetComponent<Image> ();

        //synthesizeButton = transform.Find("SynthesizeButton").GetComponent<Button>();
        //decorateVesselButton = transform.Find("DecorateVesselButton").GetComponent<Button>();
        //dropButton = transform.Find("DropButton").GetComponent<Button>();
        closeButton = transform.Find("CloseButton").GetComponent<Button>();

        closeButton.onClick.AddListener(ClosePanel);
    }
    public void ClosePanel()
    {
        Destroy(gameObject);
    }
    public void Initial(InventoryItemInfo itemInfo)
    {
        ItemInfo = itemInfo;
        itemImage.sprite = Resources.Load<Sprite>("2D/" + ItemInfo.Item.ItemID);
        itemNameText.text = ItemInfo.Item.ItemName;
        itemCountText.text = string.Format("x{0}", ItemInfo.Count);
        itemDescriptionText.text = ItemInfo.Item.Description;

        if(ItemInfo.Item is IsolatedIslandGame.Library.Items.Material)
        {
            itemDescriptionText.text += string.Format("\n素材分數： {0}", (ItemInfo.Item as IsolatedIslandGame.Library.Items.Material).Score);
			switch ((ItemInfo.Item as IsolatedIslandGame.Library.Items.Material).GroupType) {
			case GroupType.Animal:
				itemGroupIcon.sprite = Resources.Load<Sprite> ("GroupIcon/animal");
				break;
			case GroupType.Businessman:
				itemGroupIcon.sprite = Resources.Load<Sprite> ("GroupIcon/businessman");
				break;
			case GroupType.Farmer:
				itemGroupIcon.sprite = Resources.Load<Sprite> ("GroupIcon/farmer");
				break;
			default:
				Destroy (itemGroupIcon.gameObject);
				break;
			}

			itemDescriptionText.text += string.Format("\n素材等級： {0}", (ItemInfo.Item as IsolatedIslandGame.Library.Items.Material).Level);
        }
    }
}
