using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combine_BackButton : MonoBehaviour {
    public GameObject UIControl;

    public Button C_BackButton;

    // Use this for initialization
    void Start ()
    {
        UIControl = GameObject.FindWithTag("UImanager");
        C_BackButton = this.gameObject.GetComponent<Button>();
        C_BackButton.onClick.AddListener(Back);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void Back()
    {
        UIControl.GetComponent<UImanager>().GameUI = UImanager.UI.Show_Bag;
    }
}
