using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ShowBag_ItemSelect : MonoBehaviour, IPointerClickHandler
{
    public GameObject UIControl;
    public GameObject DetailPanel;
    public GameObject Bag;
    public GameObject CombineAreaILocate;
    public int ItemNo;
    public int PositionIndex;
    // Use this for initialization
    void Start () {
        UIControl  = GameObject.FindWithTag("UImanager");
        if(!Bag)
        {
            Bag = UIControl.GetComponent<UImanager>().Bag;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (UIControl.GetComponent<UImanager>().GameUI == UImanager.UI.Show_Bag)
        { Show_Choose(); }
        else if(UIControl.GetComponent<UImanager>().GameUI == UImanager.UI.Combine)
        { Combine_Choose(); }
    }
    void Show_Choose()
    {
        DetailPanel.SetActive(true);
    }
    void Combine_Choose()
    {
        if (this.gameObject.GetComponent<Image>().sprite != null)
        {
            if (CombineAreaILocate != null)
            {
                CombineAreaILocate.transform.GetChild(0).GetComponent<Image>().sprite = null;
                CombineAreaILocate.GetComponent<Combine_block>().ItemInHere = null;
            }
            CombineAreaILocate = Bag.GetComponent<ShowBag_pos>().CombineArea;
            Bag.GetComponent<ShowBag_pos>().SetPicture(this.gameObject);
            Bag.GetComponent<ShowBag_pos>().UpAndDown();
        }
    }
}
