using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IsolatedIslandGame.Library;

public class CombineSlotBehavior : MonoBehaviour {
    
    public Item item;
    [SerializeField]
    private Image image;

    public int index;

    public void ToInventoryPageFromCombineSlot()
    {
        CombineUIManager.Instance.puttingSlotIndex = index;
        UIManager.Instance.SwapPage(UIManager.UIPageType.Inventory);
    }

    public void SetSlotInfo(Item item)
    {
        image = transform.GetChild(0).GetComponent<Image>();

        this.item = item;
        if (item != null)
        {
            image.sprite = Resources.Load<Sprite>("2D/" + item.ItemID);
        }
        else
            image.sprite = null;
    }
	
}
