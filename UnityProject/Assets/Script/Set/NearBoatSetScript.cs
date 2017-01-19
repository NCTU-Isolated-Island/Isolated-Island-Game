using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IsolatedIslandGame.Protocol;
public class NearBoatSetScript : MonoBehaviour {

    public GameObject PlayerName;
    public GameObject Camp;
    public GameObject Speach;

    // Use this for initialization
    void Start () {
        if (!PlayerName)
            PlayerName = this.gameObject.transform.GetChild(1).gameObject;
        if (!Camp)
            Camp = this.gameObject.transform.GetChild(2).gameObject;
        if (!Camp)
            Speach = this.gameObject.transform.GetChild(4).gameObject;
    }

  public  void SetInfo(string InPlayerName, GroupType InCamp, string InSpeach)
    {
        PlayerName.GetComponent<Text>().text = InPlayerName;
        switch(InCamp)
        {
            case GroupType.Animal: Camp.GetComponent<Text>().text = (">動物-等級10"); break;
            case GroupType.Businessman: Camp.GetComponent<Text>().text = (">商業-等級8"); break;
            case GroupType.Farmer: Camp.GetComponent<Text>().text = (">農林-等級15"); break;
        }
        Speach.GetComponent<Text>().text = InSpeach;
    }
}
