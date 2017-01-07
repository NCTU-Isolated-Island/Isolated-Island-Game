using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UImanager : MonoBehaviour {

    public enum UI { Login, Main_Boat, Other_Boat, Map, Show_Bag,Combine };
    public bool StopForTest;
    public UI GameUI;
    private int NowAt;

    public GameObject[] UIObject;
	public static UImanager Instance;
    // Use this for initialization
    void Awake () {
		if(Instance == null){
			Instance = this;
		}else if(Instance != this){
			Destroy(transform.root.gameObject);
		}
		DontDestroyOnLoad(transform.root.gameObject);

        GameUI = UI.Login;
        NowAt = (int)GameUI;
        if(!StopForTest)
        { ChangeUI(NowAt); }
    }
	void ChangeUI(int a)
    {
        GameUI = (UI)a;
        NowAt = a;
        for (int i = 0; i < UIObject.Length; i++)
        {
            if (i == a)
            {
                UIObject[i].active = true;
            }
            else
            {
                UIObject[i].active = false;
            }
        }
    }
	// Update is called once per frame
	void Update ()
    {
		if((int)GameUI != NowAt)
        {
            ChangeUI((int)GameUI);
        }
	}
    public void LoadResult(int Result)
    {
        UIObject[0].GetComponent<L_Loading>().LoadResult(Result);
    }
}
