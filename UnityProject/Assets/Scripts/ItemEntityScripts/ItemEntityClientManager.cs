using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using System.Linq;

public class ItemEntityClientManager : MonoBehaviour
{
    public static ItemEntityClientManager Instance { get; private set; }

    private Dictionary<int, GameObject> RenderedItemEntityList = new Dictionary<int, GameObject>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            DiscardItem();
        }
    }

    public void DiscardItem()
    {
        UserManager.Instance.User.Player.OperationManager.DiscardItem(1013, 1);
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        GenerateAllItemEntity();
        ItemEntityManager.Instance.OnItemEntityChange += OnItemEntityChange;
    }

    public void GenerateAllItemEntity()
    {        
        foreach (ItemEntity itemEntity in ItemEntityManager.Instance.ItemEntities)
        {
            GameObject item = Instantiate(GameManager.Instance.ElementModels[itemEntity.ItemID]);
            item.transform.localScale *= 5f;
            item.transform.position = new Vector3(itemEntity.PositionX, 0, itemEntity.PositionZ);

            item.AddComponent<ItemEntityBehavior>();
            item.GetComponent<ItemEntityBehavior>().SetItemEntityID(itemEntity.ItemEntityID);

            RenderedItemEntityList.Add(itemEntity.ItemEntityID, item);
        }
    }

    public void PickupItemEntity(int itemEntityID)
    {
        UserManager.Instance.User.Player.OperationManager.PickupItemEntity(itemEntityID);
    }

    private void OnItemEntityChange(DataChangeType type, ItemEntity itemEntity)
    {
        switch(type)
        {
            case DataChangeType.Add:
                {
                    GameObject item = Instantiate(GameManager.Instance.ElementModels[itemEntity.ItemID]);
                    item.transform.localScale *= 5f;
                    item.transform.position = new Vector3(itemEntity.PositionX, 0, itemEntity.PositionZ);

                    item.AddComponent<ItemEntityBehavior>();
                    item.GetComponent<ItemEntityBehavior>().SetItemEntityID(itemEntity.ItemEntityID);

                    RenderedItemEntityList.Add(itemEntity.ItemEntityID, item);
                }
                break;
            case DataChangeType.Remove:
                {
                    if(RenderedItemEntityList.ContainsKey(itemEntity.ItemEntityID))
                    {
                        Destroy(RenderedItemEntityList[itemEntity.ItemEntityID]);
                        RenderedItemEntityList.Remove(itemEntity.ItemEntityID);
                    }
                }
                break;
        }
    }

}