using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ShowBag_View : MonoBehaviour,IPointerDownHandler, IPointerUpHandler
{
    public GameObject UIControl;

    public bool clicking = false;
    public float clicking_time = 0;
    public GameObject SubButton1, SubButton2;
	// Use this for initialization
	void Start () {
        UIControl = GameObject.FindWithTag("UImanager");
        clicking = false;
        clicking_time = 0;
        SubButton1.active = false;
        SubButton2.active = false;
    }
	
	// Update is called once per frame
	void Update () {
	if(clicking)
        { clicking_time = clicking_time + Time.deltaTime;
             if(clicking_time>1.2f && SubButton1.activeInHierarchy == false)
            {
                SubButton1.active = true;
                SubButton2.active = true;
            }
        }	
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Down");
        clicking = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
       
        if(SubButton1.GetComponent<ShowBag_ViewSub>().MouseIn)
        {
          Debug.Log("PutOnBoat:");
          SubButton1.GetComponent<ShowBag_ViewSub>().MouseIn = false;
        }
        else if(SubButton2.GetComponent<ShowBag_ViewSub>().MouseIn)
        {
            Debug.Log("Combine:" );
            SubButton2.GetComponent<ShowBag_ViewSub>().MouseIn = false;
            UIControl.GetComponent<UImanager>().GameUI = UImanager.UI.Combine;
        }
        else
        { Debug.Log("Up In None:" + clicking_time); }
        clicking = false;
        clicking_time = 0;
        SubButton1.active = false;
        SubButton2.active = false;
    }
}
