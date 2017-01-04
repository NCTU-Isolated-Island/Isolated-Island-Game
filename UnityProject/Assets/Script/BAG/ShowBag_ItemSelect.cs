using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ShowBag_ItemSelect : MonoBehaviour, IPointerClickHandler
{
    public GameObject DetailPanel;
    public GameObject BAG;
	// Use this for initialization
	void Start () {
	
	}
    public void OnPointerClick(PointerEventData eventData)
    {
        if (BAG.GetComponent<ShowBag_pos>().enabled)
        { choose(); }
    }
    void choose()
    {
        DetailPanel.SetActive(true);
    }
        // Update is called once per frame
        void Update () {
	
	}
}
