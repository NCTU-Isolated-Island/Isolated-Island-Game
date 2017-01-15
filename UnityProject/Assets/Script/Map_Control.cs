using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map_Control : MonoBehaviour {

    public GameObject UIControl;

    public GameObject IslandInfoList;

    public Button Map_BackButton;

    // Use this for initialization
    void Start () {
        UIControl = GameObject.FindWithTag("UImanager");
        Map_BackButton = this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Button>();
        Map_BackButton.onClick.AddListener(Back);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void Back()
    {
        UIControl.GetComponent<UImanager>().GameUI = UImanager.UI.Main_Boat;
    }
}
