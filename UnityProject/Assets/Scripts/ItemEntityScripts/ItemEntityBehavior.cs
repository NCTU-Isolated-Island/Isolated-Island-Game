using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEntityBehavior : MonoBehaviour
{
    [SerializeField]
    private int itemEntityID;

    private float theta = 0f;

    private void OnMouseUpAsButton()
    {
        ItemEntityClientManager.Instance.PickupItemEntity(itemEntityID);
    }

    public void SetItemEntityID(int itemEntityID)
    {
        this.itemEntityID = itemEntityID;
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Sin(theta) * 0.3f, transform.position.z);

        theta += Time.deltaTime;
    }
}