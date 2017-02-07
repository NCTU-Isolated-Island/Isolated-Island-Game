using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class Combine_block : MonoBehaviour,IPointerClickHandler {

    public GameObject UIControl;
    public GameObject Bag;
    public int ItemInHere;
    // Use this for initialization
    void Start () {
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
        Bag.GetComponent<ShowBag_pos>().CombineArea = this.gameObject;
        Bag.GetComponent<ShowBag_pos>().UpAndDown();
    }

}
