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
    float time = 0;

    // Use this for initialization
    void Start () {
        UIControl = GameObject.FindWithTag("UImanager");
        MainBoat = UIControl.GetComponent<UImanager>().UIObject[1];
        Loading.SetActive(true);
        DragUp.SetActive(false);
        CreateChar.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(FBconfirm);
    }
	
    public void ShowLoadingIcon()
    {
        Loading.SetActive(true);
    }
    public void UnShowLoadingIcon()
    {
        Loading.SetActive(false);
    }
    public void FBconfirm()
    {
        BG_A.SetActive(true);
        BG_B.SetActive(true);
        GameTitle.SetActive(false);
        CreateChar.SetActive(false);
        this.gameObject.transform.GetChild(3).gameObject.SetActive(true);

    }
    // Update is called once per frame
    void Update () {

        if(LoadingCase == 2)
        {
            time += Time.deltaTime;
            if(time>0.1)
            {
                Loading.transform.Rotate(new Vector3(0,0,-30));
                time = 0;
            }
        }
		else if(LoadingCase == 0)
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
            GameTitle.SetActive(true);
            Loading.SetActive(false);
            DragUp.SetActive(false);

            BG_A.SetActive(true); 
            BG_B.SetActive(false);

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
