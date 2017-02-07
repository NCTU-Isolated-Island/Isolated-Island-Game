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
        for (int i = 0; i < 3; i++)
        {        
                ShowBag_pos.Instance.AllCombineArea[i].GetComponent<Combine_block>().ItemInHere = 0;
                ShowBag_pos.Instance.AllCombineArea[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
            
        }
        UIControl.GetComponent<UImanager>().ChangeUI((int)UImanager.UI.Show_Bag);     
    }
}
