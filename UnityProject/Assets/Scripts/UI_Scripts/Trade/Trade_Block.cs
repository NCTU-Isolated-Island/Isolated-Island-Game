using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Trade_Block : MonoBehaviour, IPointerClickHandler
{
    public GameObject UIControl;
    public GameObject Bag;
    public int ItemInHere;
    public int AmountInHere;
    public int Position;

    // Use this for initialization
    void Start()
    {
        if (!UIControl)
        {
            UIControl = GameObject.FindWithTag("UImanager");
        }
        if (!Bag)
        {
            Bag = UIControl.GetComponent<UImanager>().Bag;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Bag.GetComponent<ShowBag_pos>().TradeArea = this.gameObject;
        ShowBag_pos.Instance.Reset_Bag();
        Bag.GetComponent<ShowBag_pos>().UpAndDown();
    }




}
