using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using System.Collections.Generic;
using System.Linq;
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

	[SerializeField]
	private Button toPutItemButton;

    //
    [SerializeField]
    private Button toMainPageButton;
        //

    public enum InventoryUsageType
    {
        CheckInventoryItemDetail , PutInCombineSlot , PutInTransactionSlot , PutItemOnVessel , Donation , ThrowMaterialToIsland
    }

    [SerializeField]
    private InventoryUsageType currentUsageType = InventoryUsageType.CheckInventoryItemDetail;

    public void SetUsageType(InventoryUsageType usageType)
    {
        currentUsageType = usageType;
    }

    private void Awake()
    {
        inventoryScrollViewContent = transform.Find("InventoryScrollView/Viewport/Content").GetComponent<RectTransform>();
        Instance = this;
    }
    private void Start()
    {
        if (UserManager.Instance.User.IsOnline)
        {
            RegisterEventsToPlayer(UserManager.Instance.User.Player);
        }
        else
        {
            UserManager.Instance.User.OnPlayerOnline += RegisterEventsToPlayer;
        }
        transform.gameObject.SetActive(false);

		toPutItemButton.onClick.AddListener (delegate {
			PlayerDecorationManager.Instance.CurrentControlMode = PlayerDecorationManager.ControlMode.Decorate;
			GameManager.Instance.PlayerGameObject.GetComponentInChildren<MeshCollider>().enabled = true;
			GameManager.Instance.PlayerGameObject.GetComponent<BoxCollider>().enabled = false;
			UIManager.Instance.SwapPage (UIManager.UIPageType.PutItem);
		});

    }

    public void ShowPanel(InventoryUsageType usageType)
    {
        currentUsageType = usageType;
        RenderInventory(inventory);
        // Add Effects
        if (currentUsageType == InventoryUsageType.PutInTransactionSlot)
            toMainPageButton.gameObject.SetActive(false);
        else
            toMainPageButton.gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        // Special case dealing base on UI layer issue
        if (currentUsageType == InventoryUsageType.PutInTransactionSlot)
            TransactionManager.Instance.TransactionUIPage.SetActive(true);

        UIManager.Instance.ToPreviousPage();
    }

    public void ShowItemInfoDetail(InventoryItemInfo info)
    {
        ItemInfoDetailPanel itemInfoDetailPanel = Instantiate(itemInfoDetailPanelPrefab);
        itemInfoDetailPanel.transform.SetParent(transform);
        itemInfoDetailPanel.GetComponent<RectTransform>().localScale = Vector3.one;
        itemInfoDetailPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -37.5f);
        // Fix offset so it won't shrink
        itemInfoDetailPanel.GetComponent<RectTransform>().offsetMax = new Vector2(0, -50);
        itemInfoDetailPanel.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        //
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
        foreach (var info in inventory.ItemInfos.OrderBy(x => x.InventoryItemInfoID))
        {
            InventoryItemInfoBlock infoBlock = Instantiate(inventoryItemInfoBlockPrefab);
            //
            infoBlock.GetComponent<RectTransform>().localScale = Vector3.one;
            //
            infoBlock.transform.SetParent(inventoryScrollViewContent);
            infoBlock.Initial(info, currentUsageType);
            inventoryItemInfoBlockDictionary.Add(info.InventoryItemInfoID, infoBlock);
			infoBlock.GetComponent<RectTransform>().localScale = Vector3.one;

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
                        infoBlock.Initial(info, currentUsageType);
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
