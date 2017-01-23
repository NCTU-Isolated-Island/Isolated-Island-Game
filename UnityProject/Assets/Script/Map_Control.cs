﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map_Control : MonoBehaviour {

    public GameObject UIControl;

    public Button Map_BackButton;
    public Button ShowList_Button;
    public Button ShowNear_Button;

    public GameObject FunctionList;

    float CanvasWidth;
    public bool ShowNearBool = false;
    float passtime = 0;
    int times = 0;

    float A, B, y_value;

    // Use this for initialization
    void Start()
    {
        SetGameObject();
    }

    void Back()
    {
        FunctionList.GetComponent<RectTransform>().localPosition = new Vector3(B, y_value, 0);
        UIControl.GetComponent<UImanager>().BlackFade = true;
        UIControl.GetComponent<UImanager>().ChangeUI((int)UImanager.UI.Main_Boat);
    }
    void Update()
    {
        if(ShowNearBool)
        {
            passtime += Time.deltaTime;
            if(passtime>0.05)
            {
                this.gameObject.transform.localPosition = this.gameObject.transform.localPosition - new Vector3(CanvasWidth/10, 0,0);
                if(times<9)
                { times++; }
                else { times = 0; ShowNearBool = false; }
            }         
        }

    }
    void ShowNear()
    {
        ShowNearBool = true;
       // this.gameObject.GetComponent<NearBoat_Control>().MoveList();
    }

    void SetGameObject()
    {
        if (!FunctionList)
            FunctionList = this.gameObject.transform.GetChild(1).gameObject;

        A = -FunctionList.transform.parent.parent.GetComponent<RectTransform>().rect.width / 2 + FunctionList.transform.GetComponent<RectTransform>().rect.width / 2;
        B = -FunctionList.transform.parent.parent.GetComponent<RectTransform>().rect.width / 2 - FunctionList.transform.GetComponent<RectTransform>().rect.width / 2;
        y_value = FunctionList.GetComponent<RectTransform>().localPosition.y;

        if (!UIControl)
            UIControl = GameObject.FindWithTag("UImanager");

        if (!ShowList_Button)
            ShowList_Button = this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Button>();
       // ShowList_Button.onClick.AddListener(ShowList);

        if (!Map_BackButton)
            Map_BackButton = this.gameObject.transform.GetChild(2).GetComponent<Button>();
        Map_BackButton.onClick.AddListener(Back);

        if (!ShowNear_Button)
            ShowNear_Button = this.gameObject.transform.GetChild(3).GetComponent<Button>();
        ShowNear_Button.onClick.AddListener(ShowNear);

        CanvasWidth = UIControl.GetComponent<UImanager>().Canvas.GetComponent<RectTransform>().rect.width;
    }
}
