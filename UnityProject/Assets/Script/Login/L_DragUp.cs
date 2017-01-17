using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class L_DragUp : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{

    public GameObject UIControl;
    public GameObject Canvas;
    public GameObject Login;
    public GameObject BG_Black,BG_DarkBlue;
    public bool DragStart;

    Vector3 MouseOriginPos, BGOriginPos;

    // Use this for initialization
    void Start () {

        

        UIControl = GameObject.FindWithTag("UImanager");
        if(!Canvas)
        Login = UIControl.GetComponent<UImanager>().UIObject[0];
        DragStart = false;
        BGOriginPos = Login.transform.localPosition;
    }
	
	// Update is called once per frame
	void Update () {
		if(Login.GetComponent<L_Loading>().LoadingCase == 0)
        {
            DragStart = true;
            BG_Black.active = true;
        }
	}

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (DragStart)
        { MouseOriginPos = eventData.position; }

    }
    public void OnDrag(PointerEventData eventData)
    {
        if (DragStart)
        {
            if (eventData.position.y > MouseOriginPos.y)
            {
                float delta = eventData.position.y - MouseOriginPos.y;
                Login.transform.localPosition = new Vector3(BGOriginPos.x, BGOriginPos.y + delta, 0);
            }

        }
       
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (DragStart)
        {
            if (Login.transform.localPosition.y - BGOriginPos.y > Canvas.GetComponent<RectTransform>().rect.height * 3 / 5)
            {   Login.active = false; BG_Black.active = false;
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
				UIControl.GetComponent<UImanager>().GameUI = UImanager.UI.Main_Boat;
			}
            else
            {
                Login.transform.localPosition = BGOriginPos;
            }
        }
          
    }
}
