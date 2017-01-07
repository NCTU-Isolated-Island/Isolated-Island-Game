using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Combine_ItemSelect : MonoBehaviour, IPointerClickHandler
{
    public GameObject Bag;
    public GameObject CombineAreaILocate;
    public int ItemNo;
    // Use this for initialization
    void Start()
    {
       
    }
    public void OnPointerClick(PointerEventData eventData)
    {if (Bag.GetComponent<Combine_pos>().enabled)
        { choose(); }
    }
    void choose()
    {
        if (this.gameObject.GetComponent<Image>().sprite != null)
        {
            if (CombineAreaILocate != null)
            {
                CombineAreaILocate.transform.GetChild(0).GetComponent<Image>().sprite = null;
                CombineAreaILocate.GetComponent<Combine_block>().ItemInHere = null;
            }
            CombineAreaILocate = Bag.GetComponent<Combine_pos>().CombineArea;
            Bag.GetComponent<Combine_pos>().SetPicture(this.gameObject); 
            Bag.GetComponent<Combine_pos>().UpAndDown();
        }
    }

}
