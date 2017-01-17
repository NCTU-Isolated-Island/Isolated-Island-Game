using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class IB_BackButton : MonoBehaviour, IPointerClickHandler
{
    public GameObject Bag;
    // Use this for initialization
    void Start () {
	
	}
    public void OnPointerClick(PointerEventData eventData)
    {
        Bag.GetComponent<ShowBag_pos>().UpAndDown();
    }
    // Update is called once per frame
    void Update () {
	
	}
}
