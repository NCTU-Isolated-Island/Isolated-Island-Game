using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainBoatUIManager : MonoBehaviour {

    public static MainBoatUIManager Instance { get; private set; }

    // Variables
    [SerializeField]
    private GameObject mask; private bool maskStatus;

    [SerializeField]
    private Button[] verticalButtonList;
    [SerializeField]
    private Button[] horizontalButtonList;

    //

    // Use this for initialization
    void InitSetting()
    {
        maskStatus = true;
    }

	void Start () {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        InitSetting();

        foreach(Button button in verticalButtonList)
        {
            switch(button.name)
            {
                case "ToIngredientsButton":
                    button.onClick.AddListener(delegate { UIManager.Instance.SwapPage(UIManager.UIPageType.Show_Bag); });
                    break;
                case "ToBluePrintButton":
                    button.onClick.AddListener(delegate { UIManager.Instance.SwapPage(UIManager.UIPageType.BluePrint); });
                    break;
                case "ToMissionButton":
                    button.onClick.AddListener(delegate { UIManager.Instance.SwapPage(UIManager.UIPageType.Mission); });
                    break;
                case "ToFriendButton":
                    button.onClick.AddListener(delegate { UIManager.Instance.SwapPage(UIManager.UIPageType.Friend); });
                    break;
                case "ToChatButton":
                    button.onClick.AddListener(delegate { UIManager.Instance.SwapPage(UIManager.UIPageType.Chat_Record); });
                    break;
                case "ToEverydayButton":
                    // TODO : Add listener to this button
                    break;
                case "ToPutItemButton":
                    button.onClick.AddListener(delegate { UIManager.Instance.SwapPage(UIManager.UIPageType.PutItem); });
                    break;
            }
        }
    }
	
	public void ReverseMaskStatus()
    {
        mask.SetActive(!maskStatus);
        maskStatus = !maskStatus;
    }

}
