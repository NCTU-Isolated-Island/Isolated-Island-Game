using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionUIManager : MonoBehaviour {
    
    public GameObject MissionDetailPanel;
    int Page = 1;
    public GameObject Page1, Page2;
    public Button Right, Left;
    public GameObject MissionSet;
    public GameObject MissionContent;

    public Button View_Button;
    public Button Back_Button;

    public Sprite[] Pic_ICON;

    public Button ListButton;
    public GameObject FunctionList;

    public float CanvasWidth ,  CanvasHeight;
}
