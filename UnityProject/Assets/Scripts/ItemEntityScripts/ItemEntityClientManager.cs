using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;

public class ItemEntityClientManager : MonoBehaviour
{
    public static ItemEntityClientManager Instance { get; private set; }

    private List<int> RenderedItemEntityList = new List<int>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            GenerateAllItemEntity();
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
        foreach (ItemEntity itemEntity in ItemEntityManager.Instance.ItemEntities)
        {
            GameObject item = Instantiate(GameManager.Instance.ElementModels[itemEntity.ItemID]);
            item.transform.localScale *= 5f;
            item.transform.position = new Vector3(itemEntity.PositionX, 0, itemEntity.PositionZ);
            item.AddComponent<ItemEntityBehavior>();
            item.GetComponent<ItemEntityBehavior>().SetItemEntityID(itemEntity.ItemEntityID);

            RenderedItemEntityList.Add(itemEntity.ItemEntityID);
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