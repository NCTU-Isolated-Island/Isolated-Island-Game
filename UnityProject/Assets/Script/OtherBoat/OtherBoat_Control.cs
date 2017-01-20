using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OtherBoat_Control : MonoBehaviour {

    public GameObject UIControl;
    public Button Back_Button;
    public int WhereIFrom;
    void Start () {
        WhereIFrom = 1;
        if (!UIControl)
            UIControl = GameObject.FindWithTag("UImanager");
        if(!Back_Button)
        Back_Button = this.gameObject.transform.GetChild(2).GetChild(8).GetComponent<Button>();
        Back_Button.onClick.AddListener(Back);
    }
	void Back()
    {
        UIControl.GetComponent<UImanager>().ChangeUI(WhereIFrom);
    }
    // Update is called once per frame
    void Update () {
		
	}
}
