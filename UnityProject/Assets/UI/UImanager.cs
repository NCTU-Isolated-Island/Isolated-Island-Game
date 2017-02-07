using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IsolatedIslandGame.Library;

public class UImanager : MonoBehaviour
{

    public enum UI { Login, Main_Boat, Other_Boat, Map, Show_Bag, Combine, Mission, Friend, Chat, Trade };

    public bool StopForTest;

    public UI GameUI;
    private int NowAt;
    public string player_name, speech;
    public int CampNum;
    public GameObject[] UIObject;
    public static UImanager Instance;
    public GameObject Bag;
    //public GameObject ItemPic;
    public GameObject Canvas;
    public GameObject BG_Black;

    public GameObject FunctionList;

    float CanvasHeight;
    public bool BlackFade = false;
    float passtime = 0;
    int times = 0;

    // Use this for initialization
    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(transform.root.gameObject);
        }
        DontDestroyOnLoad(transform.root.gameObject);

        NowAt = (int)GameUI;
        if (!StopForTest)
        {
            GameUI = UI.Login;
            ChangeUI(NowAt);
            for (int i = 0; i < UIObject.Length; i++)
            {
                if (i == NowAt)
                {
                    UIObject[i].SetActive(true);
                }
                else
                {
                    UIObject[i].SetActive(false);
                }
            }
        }
        CanvasHeight = Canvas.GetComponent<RectTransform>().rect.height;
    }
    public void ChangeUI(int a)
    {
        GameUI = (UI)a;
        NowAt = a;
      
        if (a == 1)
        {
          if(GameManager.Instance.PlayerGameObject)
                CameraManager.Instance.ToNearAnchor(GameManager.Instance.PlayerGameObject);
          UIObject[1].SetActive(true);
        }
        else if (a == 2 || a == 3 || a == 5 || a == 9)//需要黑幕特效->等下Update換
        {
            BlackFade = true;

            if(a==9)
            {
                UIObject[9].transform.localPosition = new Vector3(0, 0, 0);
            }
        }
        else if (!BlackFade)  //無須黑幕特效->直接換
        {
            if (a == 4)
            {
                Bag.GetComponent<ShowBag_pos>().UpAndDown();

                if (UIObject[6].activeInHierarchy)
                { UIObject[6].GetComponent<Mission_Control>().MoveMis = true; }
                if (UIObject[7].activeInHierarchy)
                { UIObject[7].GetComponent<Friend_Control>().MoveFri = true; }
                if (UIObject[8].activeInHierarchy)
                { UIObject[8].GetComponent<Chat_Control>().MoveCha = true; }
            }
            else if (a == 6)
            {        
                UIObject[6].SetActive(true);
                UIObject[6].GetComponent<Mission_Control>().MoveMis = true;

                if(Bag.GetComponent<ShowBag_pos>().BagOut)
                {Bag.GetComponent<ShowBag_pos>().UpAndDown();}
                if (UIObject[7].activeInHierarchy)
                { UIObject[7].GetComponent<Friend_Control>().MoveFri = true; }
                if(UIObject[8].activeInHierarchy)
                { UIObject[8].GetComponent<Chat_Control>().MoveCha = true; }

            }
            else if (a == 7)
            {
                UIObject[7].SetActive(true);
                UIObject[7].GetComponent<Friend_Control>().MoveFri = true;

                if (Bag.GetComponent<ShowBag_pos>().BagOut)
                { Bag.GetComponent<ShowBag_pos>().UpAndDown(); }
                if (UIObject[6].activeInHierarchy)
                { UIObject[6].GetComponent<Mission_Control>().MoveMis = true; }
                if (UIObject[8].activeInHierarchy)
                { UIObject[8].GetComponent<Chat_Control>().MoveCha = true; }
            }
            else if (a == 8)
            {
                UIObject[8].SetActive(true);
                UIObject[8].GetComponent<Chat_Control>().MoveCha = true;

                if (Bag.GetComponent<ShowBag_pos>().BagOut)
                { Bag.GetComponent<ShowBag_pos>().UpAndDown(); }
                if (UIObject[6].activeInHierarchy)
                { UIObject[6].GetComponent<Mission_Control>().MoveMis = true; }
                if (UIObject[7].activeInHierarchy)
                { UIObject[7].GetComponent<Friend_Control>().MoveFri = true; }
            }

           /* else
                for (int i = 0; i < UIObject.Length; i++)
                {
                    if (i == NowAt)
                    {
                        UIObject[i].SetActive(true);
                    }
                    else
                    {
                        UIObject[i].SetActive(false);
                    }
                }*/
        }

    }
    // Update is called once per frame
    void Update()
    {
        if ((int)GameUI != NowAt)
        {
            ChangeUI((int)GameUI);
        }

        if (BlackFade)
        {
            passtime += Time.deltaTime;
            if (passtime > 0.04)
            {
                if (times == 0)
                {
                    BG_Black.SetActive(true);
                    BG_Black.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                }
                else if (times < 14)
                {
                    BG_Black.GetComponent<Image>().color = new Color(0, 0, 0, BG_Black.GetComponent<Image>().color.a + (20f / 255f));
                }
                else if (times <= 19)
                {
                    if (times == 19)
                    {
                        for (int i = 0; i < UIObject.Length; i++)
                        {
                            if (i == NowAt)
                            {
                                UIObject[i].SetActive(true);
                            }
                            else if(i==9)
                            {

                            }
                            else
                            {
                                UIObject[i].SetActive(false);
                            }
                        }

                        if (NowAt == 4)
                        {
                            UIObject[1].SetActive(true);
                            Bag.gameObject.transform.localPosition = new Vector3(0, 0, 0);
                            Bag.GetComponent<ShowBag_pos>().BagOut = true;
                        }
                        else
                        {
                            Bag.gameObject.transform.localPosition = new Vector3(0, -CanvasHeight, 0);
                            Bag.GetComponent<ShowBag_pos>().BagOut = false;
                        }

                    }
                }
                else if (times < 34)
                {
                    BG_Black.GetComponent<Image>().color = new Color(0, 0, 0, BG_Black.GetComponent<Image>().color.a - (20f / 255f));
                }
                else
                {
                    BG_Black.SetActive(false);
                    BG_Black.GetComponent<Image>().color = new Color(0, 0, 0, 255);
                    BlackFade = false;
                    times = -1;
                }
                passtime = 0;
                times++;
            }
        }

    }

    public void LoadResult(int Result)
    {
        UIObject[0].GetComponent<L_Loading>().LoadResult(Result);
    }
    public void Create_Charter()
    {
        switch (CampNum)
        {
            case 0: UserManager.Instance.User.Player.OperationManager.CreateCharacter(player_name, speech, IsolatedIslandGame.Protocol.GroupType.Farmer); break;
            case 1: UserManager.Instance.User.Player.OperationManager.CreateCharacter(player_name, speech, IsolatedIslandGame.Protocol.GroupType.Businessman); break;
            case 2: UserManager.Instance.User.Player.OperationManager.CreateCharacter(player_name, speech, IsolatedIslandGame.Protocol.GroupType.Animal); break;
        }
    }
}
