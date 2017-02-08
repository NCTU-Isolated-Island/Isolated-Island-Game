using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{
    public static InventoryPanel Instance { get; private set; }

    private Dictionary<int, InventoryItemInfoBlock> inventoryItemInfoBlockDictionary = new Dictionary<int, InventoryItemInfoBlock>();
    private Inventory inventory;
    private RectTransform inventoryScrollViewContent;
    [SerializeField]
    private InventoryItemInfoBlock inventoryItemInfoBlockPrefab;
    [SerializeField]
    private ItemInfoDetailPanel itemInfoDetailPanelPrefab;

    private void Awake()
    {
        inventoryScrollViewContent = transform.Find("InventoryScrollView/Viewport/Content").GetComponent<RectTransform>();
        Instance = this;
    }
    private void Start()
    {
        UserManager.Instance.User.OnPlayerOnline += RegisterEventsToPlayer;
        transform.gameObject.SetActive(false);
    }

    public void ShowPanel()
    {
        gameObject.SetActive(true);
    }
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
    public void ShowItemInfoDetail(InventoryItemInfo info)
    {
        ItemInfoDetailPanel itemInfoDetailPanel = Instantiate(itemInfoDetailPanelPrefab);
        itemInfoDetailPanel.transform.SetParent(transform);
        itemInfoDetailPanel.GetComponent<RectTransform>().localScale = Vector3.one;
        itemInfoDetailPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -37.5f);
        itemInfoDetailPanel.Initial(info);
    }

    private void RegisterEventsToPlayer(Player player)
    {
        if (player.Inventory == null)
        {
            player.OnBindInventory += (inventory) =>
            {
                this.inventory = inventory;
                inventory.OnItemInfoChange += RenderInventoryItemInfoChange;
                RenderInventory(inventory);
            };
        }
        else
        {
            inventory = player.Inventory;
            inventory.OnItemInfoChange += RenderInventoryItemInfoChange;
            RenderInventory(inventory);
        }
    }

    private void RenderInventory(Inventory inventory)
    {
        foreach (Transform child in inventoryScrollViewContent)
        {
            Destroy(child.gameObject);
        }
        inventoryItemInfoBlockDictionary.Clear();
        inventoryScrollViewContent.sizeDelta = new Vector2(364, 15 + 115 * (Mathf.CeilToInt(inventory.DifferentItemCount / 3f)));
        foreach(var info in inventory.ItemInfos)
        {
            InventoryItemInfoBlock infoBlock = Instantiate(inventoryItemInfoBlockPrefab);
            infoBlock.transform.SetParent(inventoryScrollViewContent);
            infoBlock.Initial(info);
            inventoryItemInfoBlockDictionary.Add(info.InventoryItemInfoID, infoBlock);
        }
    }
    private void RenderInventoryItemInfoChange(DataChangeType changeType, InventoryItemInfo info)
    {
        switch(changeType)
        {
            case DataChangeType.Add:
                RenderInventory(inventory);
                break;
            case DataChangeType.Remove:
                RenderInventory(inventory);
                break;
            case DataChangeType.Update:
                {
                    InventoryItemInfoBlock infoBlock;
                    if (inventoryItemInfoBlockDictionary.TryGetValue(info.InventoryItemInfoID, out infoBlock))
                    {
                        infoBlock.Initial(info);
                    }
                    else
                    {
                        RenderInventory(inventory);
                    }
                }
                break;
        }
    }
}
