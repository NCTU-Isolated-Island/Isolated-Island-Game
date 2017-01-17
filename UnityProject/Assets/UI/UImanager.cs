using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsolatedIslandGame.Library;
public class UImanager : MonoBehaviour {

    public enum UI { Login, Main_Boat, Other_Boat, Map, Show_Bag,Combine };
    public bool StopForTest;
    public UI GameUI;
    private int NowAt;
    public string player_name, speech;
    public int CampNum;
    public GameObject[] UIObject;
    public static UImanager Instance;
    public GameObject Bag;
    public GameObject Canvas;
    // Use this for initialization
    void Awake() {

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
        if(!StopForTest)
        { GameUI = UI.Login; ChangeUI(NowAt); }
    }
	void ChangeUI(int a)
    {
        GameUI = (UI)a;
        NowAt = a;
        for (int i = 0; i < UIObject.Length; i++)
        {
            if (i == a)
            {
                UIObject[i].SetActive(true); 
            }
            else
            {
                UIObject[i].SetActive(false);
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
