using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ShowBag_DetailPanel : MonoBehaviour, IPointerClickHandler
{

	// Use this for initialization
	void Start () {
	
	}
    public void OnPointerClick(PointerEventData eventData)
    {
        back();
    }
    void back()
    {
        this.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update () {
	
	}
}
