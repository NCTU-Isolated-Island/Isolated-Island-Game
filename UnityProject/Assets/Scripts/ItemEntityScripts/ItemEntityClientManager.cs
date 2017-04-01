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

    private Dictionary<ItemEntity, GameObject> RenderedItemEntityList
        = new Dictionary<ItemEntity, GameObject>();

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
        ItemEntityManager.Instance.OnItemEntityChange += OnItemEntityChange;
    }

    public void GenerateAllItemEntity()
    {
        print("Generate");

        foreach(var entry in RenderedItemEntityList.Where
            ( x => !ItemEntityManager.Instance.ItemEntities.Contains(x.Key)))
        {
            Destroy(entry.Value);
            RenderedItemEntityList.Remove(entry.Key);
        }
        
        foreach (ItemEntity itemEntity in ItemEntityManager.Instance.ItemEntities)
        {
            if (RenderedItemEntityList.ContainsKey(itemEntity)) continue;

            GameObject item = Instantiate(GameManager.Instance.ElementModels[itemEntity.ItemID]);
            item.transform.localScale *= 5f;
            item.transform.position = new Vector3(itemEntity.PositionX, 0, itemEntity.PositionZ);

            item.AddComponent<ItemEntityBehavior>();
            item.GetComponent<ItemEntityBehavior>().SetItemEntityID(itemEntity.ItemEntityID);

            RenderedItemEntityList.Add(itemEntity, item);
        }
    }

    public void PickupItemEntity(int itemEntityID)
    {
        UserManager.Instance.User.Player.OperationManager.PickupItemEntity(itemEntityID);
    }

    private void OnItemEntityChange(DataChangeType type, ItemEntity itemEntity)
    {
        GenerateAllItemEntity();
    }

}