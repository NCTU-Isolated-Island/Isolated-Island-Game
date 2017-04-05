using IsolatedIslandGame.Library;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemEntityBehavior : MonoBehaviour
{
    [SerializeField]
    private int itemEntityID;

    private float theta = 0f;

    private void OnMouseUpAsButton()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Vector3.Distance(transform.position, GameManager.Instance.PlayerGameObject.transform.position) > 30f)
            {
                //insert pop up info here
                UserManager.Instance.User.UserInform("通知", "你必須要再靠近一點才能撿取素材");
                return;
            }
            ItemEntityClientManager.Instance.PickupItemEntity(itemEntityID);
        }
    }

    public void SetItemEntityID(int itemEntityID)
    {
        this.itemEntityID = itemEntityID;
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Sin(theta) * 0.3f, transform.position.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Time.time * 30f, transform.eulerAngles.z);

        theta += Time.deltaTime;
    }
}