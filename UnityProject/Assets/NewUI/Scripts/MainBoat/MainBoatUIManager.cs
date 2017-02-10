using IsolatedIslandGame.Library;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainBoatUIManager : MonoBehaviour {

    public static MainBoatUIManager Instance { get; private set; }

    // Variables
    [SerializeField]
    private GameObject mask; public bool maskStatus;

    [SerializeField]
    private Button[] verticalButtonList;
    [SerializeField]
    private Button[] horizontalButtonList;

    [SerializeField]
    private Button drawMaterialButton;
    //

    //
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    void InitSetting()
    {
        maskStatus = true;
        ReverseMaskStatus();
    }

    void Start()
    {
        InitSetting();

        foreach (Button button in verticalButtonList)
        {
            switch (button.name)
            {
                case "ToIngredientsButton":
                    button.onClick.AddListener(delegate { UIManager.Instance.SwapPage(UIManager.UIPageType.Inventory); });
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
                case "DrawMaterialButton":
                    button.onClick.AddListener(delegate
                    {
                        Debug.Log("Draw Material");
                        UserManager.Instance.User.Player.OperationManager.DrawMaterial();
                    });
                    break;
                case "ToCombineButton":
                    button.onClick.AddListener(delegate { UIManager.Instance.SwapPage(UIManager.UIPageType.Combine); });
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
