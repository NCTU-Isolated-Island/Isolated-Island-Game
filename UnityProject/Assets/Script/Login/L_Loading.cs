using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IsolatedIslandGame.Library;

public class L_Loading : MonoBehaviour {

    public int LoadingCase = 2; // 0進入1創角2等待

    public GameObject UIControl;
    public GameObject BG_A, BG_B,BG_DarkBlue;
    public GameObject GameTitle, Loading, DragUp;
    public GameObject MainBoat;
    public GameObject CreateChar;
    //public Button FB_Button,NonFB_Button;
    //public GameObject NameScreen;

    // Use this for initialization
    void Start () {
        UIControl = GameObject.FindWithTag("UImanager");
        MainBoat = UIControl.GetComponent<UImanager>().UIObject[1];
        Loading.SetActive(true);
        DragUp.SetActive(false);
        //FB_Button = CreateChar.transform.GetChild(0).GetComponent<Button>();
        //FB_Button.onClick.AddListener(Next);
    }
	
    public void ShowLoadingIcon()
    {
        Loading.SetActive(true);
    }
    public void UnShowLoadingIcon()
    {
        Loading.SetActive(false);
    }

    // Update is called once per frame
    void Update () {

		if(LoadingCase == 0)
        {
            GameTitle.SetActive(true);
            Loading.SetActive(false);
            DragUp.SetActive(true);

            MainBoat.SetActive(true);

            BG_A.SetActive(true);
            BG_B.SetActive(false);

            CreateChar.SetActive(false);
        }
        else if(LoadingCase == 1)
        {
            GameTitle.SetActive(false);
            Loading.SetActive(false);
            DragUp.SetActive(false);

            BG_A.SetActive(false); 
            BG_B.SetActive(true);

            CreateChar.SetActive(true);
            LoadingCase = -1;
        }
	}
    public void LoadResult(int Result)
    {
        LoadingCase = Result;
    }
    //void Next() {  LoadingCase = -1; CreateChar.SetActive(true); }
}
