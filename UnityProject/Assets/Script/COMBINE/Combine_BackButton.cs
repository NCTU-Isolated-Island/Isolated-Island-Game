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
        C_BackButton = this.transform.GetChild(5).GetComponent<Button>();
        C_BackButton.onClick.AddListener(Back);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void Back()
    {
        UIControl.GetComponent<UImanager>().BlackFade = true;
        UIControl.GetComponent<UImanager>().ChangeUI((int)UImanager.UI.Show_Bag);     
    }
}
