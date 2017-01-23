using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class L_DragUp : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public bool StopForTest;
    public GameObject UIControl;
    public GameObject Canvas;
    public GameObject Login;
    public GameObject BG_Black, BG_DarkBlue;
    public bool DragStart;

    Vector3 MouseOriginPos, BGOriginPos;
    public bool MakeLoginDisappear;
    public bool BlackFade;
    float passtime;
    int times;
    // Use this for initialization
    void Start()
    {

        SetGameObject();

        DragStart = false;
        passtime = 0;
        times = 0;
        BlackFade = false;
        BGOriginPos = Login.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {

        StopForTest = UIControl.GetComponent<UImanager>().StopForTest;

        if (Login.GetComponent<L_Loading>().LoadingCase == 0)
        {
            DragStart = true;
            BG_Black.SetActive(true);
        }

        if (MakeLoginDisappear)
        {
            passtime += Time.deltaTime;

            if (!BlackFade)
            {
                if (passtime > 0.02)
                {
                    if (Login.transform.localPosition.y < Canvas.GetComponent<RectTransform>().rect.height)
                    {
                        Login.transform.localPosition = new Vector3(0, Login.transform.localPosition.y + 100, 0);
                        passtime = 0;
                    }
                    else
                    {
                        BlackFade = true;
                        passtime = 0;
                    }
                }

            }
            else if (BlackFade)
            {
                if (passtime > 0.04)
                {
                    BG_Black.GetComponent<Image>().color = new Color(0, 0, 0, BG_Black.GetComponent<Image>().color.a - (20f / 255f));
                    passtime = 0;
                    times++;
                }
                if (times == 13)
                {
                    BG_Black.SetActive(false); Login.SetActive(false);
                    BlackFade = false;
                    BG_Black.GetComponent<Image>().color = new Color(0, 0, 0, 255);               
                    UIControl.GetComponent<UImanager>().GameUI = UImanager.UI.Main_Boat;
                }

            }


        }


    }
    void SetGameObject()
    {
        if (!UIControl)
            UIControl = GameObject.FindWithTag("UImanager");
        if (!Login)
            Login = this.gameObject;
        if (!Canvas)
            Canvas = UIControl.GetComponent<UImanager>().Canvas;
        if (!BG_Black)
        { BG_Black = Canvas.transform.GetChild(2).gameObject; }
        if (!BG_DarkBlue)
        { BG_DarkBlue = this.gameObject.transform.GetChild(0).GetChild(0).gameObject; }
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
            {
                DragStart = false;
                MakeLoginDisappear = true;
                if (!StopForTest)
                    UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
            }
            else
            {
                Login.transform.localPosition = BGOriginPos;
            }
        }
    }
}
