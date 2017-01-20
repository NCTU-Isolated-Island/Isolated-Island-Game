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

        if (!UIControl)
        UIControl = GameObject.FindWithTag("UImanager");
        if (!SubButton1)
            SubButton1 = this.gameObject.transform.GetChild(0).gameObject;
        if (!SubButton2)
            SubButton2 = this.gameObject.transform.GetChild(1).gameObject;

        clicking = false;
        clicking_time = 0;
        SubButton1.SetActive(false);
        SubButton2.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
  
	if(clicking)
        { clicking_time = clicking_time + Time.deltaTime;
             if(clicking_time>1.2f && SubButton1.activeInHierarchy == false)
            {
                SubButton1.SetActive(true);
                SubButton2.SetActive(true);
            }
        }	
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Down");
        if(UIControl.GetComponent<UImanager>().GameUI == UImanager.UI.Show_Bag)
        { clicking = true; }    
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
            UIControl.GetComponent<UImanager>().ChangeUI((int)UImanager.UI.Combine);
        }
        else
        { Debug.Log("Up In None:" + clicking_time); }
        clicking = false;
        clicking_time = 0;
        SubButton1.SetActive(false);
        SubButton2.SetActive(false);
    }
}
