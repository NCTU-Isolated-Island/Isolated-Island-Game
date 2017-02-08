using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowBag_ViewSub : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int ButtonNum;
    public bool MouseIn;
	// Use this for initialization
	void Start () {
        MouseIn = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerEnter(PointerEventData eventData)
    { MouseIn = true; }
    public void OnPointerExit(PointerEventData eventData)
    { MouseIn = false; }
}
