using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combine_Control : MonoBehaviour {
    public GameObject UIControl;
    public Button ListButton;
    public GameObject FunctionList;
    // Use this for initialization
    void Start () {
        if (!UIControl)
            UIControl = GameObject.FindWithTag("UImanager");
        if (!ListButton)
            ListButton = this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Button>();
        ListButton.onClick.AddListener(ShowList);
        if (!FunctionList)
            FunctionList = UIControl.GetComponent<UImanager>().FunctionList;
    }

    void ShowList()
    {
        FunctionList.GetComponent<FunctionList_Control>().ShowList();
    }
}
