using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class Combine_block : MonoBehaviour,IPointerClickHandler {
    public GameObject Bag;
    public GameObject ItemInHere;
    // Use this for initialization
    void Start () {
	
	}
	public void OnPointerClick(PointerEventData eventData)
    {
        Bag.GetComponent<Combine_pos>().CombineArea = this.gameObject;
        Bag.GetComponent<Combine_pos>().UpAndDown();
    }
    // Update is called once per frame
    void Update () {
	
	}

    void set_picture()
    {


    }
}
