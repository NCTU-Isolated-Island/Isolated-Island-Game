using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_Loading : MonoBehaviour {

    public int LoadingCase = 2; // 0進入1創角2等待

    public GameObject UIControl;
    public GameObject BG_A, BG_B,BG_DarkBlue;
    public GameObject GameTitle, Loading, DragUp;
    public GameObject MainBoat;
    public GameObject Test_CreateChar_DEADEND;

	// Use this for initialization
	void Start () {
        UIControl = GameObject.FindWithTag("UImanager");
        MainBoat = UIControl.GetComponent<UImanager>().UIObject[1];
    }
	
	// Update is called once per frame
	void Update () {

		if(LoadingCase == 0)
        {
            GameTitle.active = true;
            Loading.active = false;
            DragUp.active = true;

            MainBoat.active = true;

            BG_A.active = true;
            BG_B.active = false;

            Test_CreateChar_DEADEND.active = false;
        }
        else if(LoadingCase == 1)
        {
            GameTitle.active = false;
            Loading.active = false;
            DragUp.active = false;

            BG_A.active = false;
            BG_B.active = true;

            Test_CreateChar_DEADEND.active = true;
        }
	}
    public void LoadResult(int Result)
    {
        LoadingCase = Result;
    }
}
