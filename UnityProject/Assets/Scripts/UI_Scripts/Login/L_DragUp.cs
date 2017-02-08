using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class L_DragUp : MonoBehaviour, IPointerClickHandler
{
    public bool StopForTest;
    public GameObject UIControl;
    public GameObject Canvas;
    public GameObject Login;
    public GameObject BG_Black, BG_DarkBlue;
    public bool DragStart;

    Vector3 MouseOriginPos, BGOriginPos;
    public bool MakeLoginDisappear;
    public bool Fade;
    float passtime;
    int times;

   public GameObject BG_1, BG_2, BG_Title1,BG_Title2,BG_Title3,BG_Title4;
    // Use this for initialization
    void Start()
    {

        SetGameObject();

        DragStart = false;
        passtime = 0;
        times = 0;
        Fade = false;
        BGOriginPos = Login.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {

        StopForTest = UIControl.GetComponent<UImanager>().StopForTest;

        if (Login.GetComponent<L_Loading>().LoadingCase == 0)
        {
            DragStart = true;
           // BG_Black.SetActive(true);
           Fade = true;
        }

        if (MakeLoginDisappear)
        {
            passtime += Time.deltaTime;

           /* if (!Fade)
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

            }*/
            if (Fade)
            {
                if (passtime > 0.04)
                {
                    BG_1.GetComponent<Image>().color = new Color(49, 77, 121, BG_1.GetComponent<Image>().color.a - (10f / 255f));
                    BG_2.GetComponent<Image>().color = new Color(255, 255, 255, BG_2.GetComponent<Image>().color.a - (10f / 255f));
                    BG_Title1.GetComponent<Image>().color = new Color(0, 0, 0, BG_Title1.GetComponent<Image>().color.a - (10f / 255f));
                    BG_Title2.GetComponent<Image>().color = BG_Title1.GetComponent<Image>().color;
                    BG_Title3.GetComponent<Text>().color = BG_Title1.GetComponent<Image>().color;
                    BG_Title4.GetComponent<Image>().color = BG_Title1.GetComponent<Image>().color;
                    passtime = 0;
                    times++;
                }
                if (times == 25)
                {
                    Login.SetActive(false);
                    Fade = false;
                    //BG_Black.GetComponent<Image>().color = new Color(0, 0, 0, 255);               
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
           // Login = this.gameObject.transform.parent.parent.gameObject;
        Login = this.gameObject;
        if (!Canvas)
            Canvas = UIControl.GetComponent<UImanager>().Canvas;
        if (!BG_Black)
        { BG_Black = Canvas.transform.GetChild(2).gameObject; }
        if (!BG_DarkBlue)
        { BG_DarkBlue = Login.transform.GetChild(0).GetChild(0).gameObject; }

        if (!BG_1)
            BG_1 = Login.transform.GetChild(0).GetChild(0).gameObject;
        if (!BG_2)
            BG_2= Login.transform.GetChild(0).GetChild(1).gameObject;
        if (!BG_Title1)
            BG_Title1 = Login.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
        if (!BG_Title2)
            BG_Title2 = Login.transform.GetChild(1).GetChild(2).GetChild(0).gameObject;
        if (!BG_Title3)
            BG_Title3 = Login.transform.GetChild(1).GetChild(2).GetChild(1).gameObject;
        if (!BG_Title4)
            BG_Title4 = Login.transform.GetChild(1).GetChild(2).GetChild(2).gameObject;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (DragStart && MakeLoginDisappear ==false)
        {         
                DragStart = false;
                MakeLoginDisappear = true;
                if (!StopForTest)
                    UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");

        }

    }

        /*  public void OnBeginDrag(PointerEventData eventData)
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
          }*/
    }
