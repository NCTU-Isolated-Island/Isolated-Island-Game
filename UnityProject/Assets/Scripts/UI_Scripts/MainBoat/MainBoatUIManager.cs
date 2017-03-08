using IsolatedIslandGame.Library;
using System.Collections;
using System.Collections.Generic;
using IsolatedIslandGame.Protocol;
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
	private GameObject getMaterialPanel;
    [SerializeField]
    private Text areaTitle;

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

        foreach (Button button in horizontalButtonList)
        {
            switch (button.name)
            {
                case "ToFPVButton":
                    button.onClick.AddListener((delegate
                    {
                        PlayerController.Instance.ChangeViewMode(PlayerController.ViewMode.FirstPerson);
                    }));
                    break;
                case "ToBirdViewButton":
                    button.onClick.AddListener(delegate
                    {
                        PlayerController.Instance.ChangeViewMode(PlayerController.ViewMode.BirdView);
                    });
                    break;
                case "ToNormalViewButton":
                    button.onClick.AddListener(delegate
                    {
                        PlayerController.Instance.ChangeViewMode(PlayerController.ViewMode.NormalView);
                    });
                    break;
            }
            button.onClick.AddListener(delegate
            {
                if (MainBoatUIManager.Instance.maskStatus)
                    MainBoatUIManager.Instance.ReverseMaskStatus();
            });
        }

        foreach (Button button in verticalButtonList)
        {
            switch (button.name)
            {
                case "ToIngredientsButton":
                    button.onClick.AddListener(delegate
                    {
                        UIManager.Instance.SwapPage(UIManager.UIPageType.Inventory);
                    });
                    break;
                case "ToBluePrintButton":
                    button.onClick.AddListener(delegate
                    {
                        UIManager.Instance.SwapPage(UIManager.UIPageType.BluePrint);
                    });
                    break;
                case "ToMissionButton":
                    button.onClick.AddListener(delegate
                    {
                        UIManager.Instance.SwapPage(UIManager.UIPageType.Mission);
                    });
                    break;
                case "ToFriendButton":
                    button.onClick.AddListener(delegate
                    {
                        UIManager.Instance.SwapPage(UIManager.UIPageType.Friend);
                    });
                    break;
                case "ToChatButton":
                    button.onClick.AddListener(delegate
                    {
                        UIManager.Instance.SwapPage(UIManager.UIPageType.Chat_Record);
                    });
                    break;
                case "ToEverydayButton":
                    // TODO : Add listener to this button
                    break;
                case "DrawMaterialButton":
                    button.onClick.AddListener(delegate
                    {
                        Debug.Log("Draw Material");
                        UserManager.Instance.User.Player.OperationManager.DrawMaterial();
                    });
                    break;
                case "ToCombineButton":
                    button.onClick.AddListener(delegate
                    {
                        UIManager.Instance.SwapPage(UIManager.UIPageType.Combine);
                    });
                    break;
                case "ToPutItemButton":
                    button.onClick.AddListener(delegate
                    {
                        PlayerDecorationManager.Instance.CurrentControlMode = PlayerDecorationManager.ControlMode.Decorate;
                        UIManager.Instance.SwapPage(UIManager.UIPageType.PutItem);
                    });
                    break;
            }
        }
        if (UserManager.Instance.User.IsOnline)
        {
            RegisterPlayerEvents(UserManager.Instance.User.Player);
        }
        else {
            UserManager.Instance.User.OnPlayerOnline += RegisterPlayerEvents;
        }
    }
	
	void RegisterPlayerEvents(Player player)
	{
		player.ResponseManager.OnDrawMaterialResponse += 
			(delegate(IsolatedIslandGame.Protocol.ErrorCode returnCode, Item material, int count) {
			switch (returnCode) {
			case ErrorCode.NoError:
				RenderGetMaterialPanel (material, count);
				break;
			default:
				break;
			}
		});
        if(player.Vessel == null)
        {
            player.OnBindVessel += (vessel) => 
            {
                vessel.OnVesselTransformUpdated += Vessel_OnVesselTransformUpdated;
            };
        }
        else
        {
            player.Vessel.OnVesselTransformUpdated += Vessel_OnVesselTransformUpdated;
        }
    }

    private void Vessel_OnVesselTransformUpdated(int vesselID, float locationX, float locationY, float rotationEulerAngleY, OceanType locatedOceanType)
    {
        areaTitle.text = string.Format("{0}海域", OceanNameMapping.GetOceanName(locatedOceanType));
    }

    void RenderGetMaterialPanel(Item item , int count)
	{
		GameObject tmp = Instantiate (getMaterialPanel);
		tmp.transform.SetParent (transform);
		tmp.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, 30, 0);
		tmp.GetComponent<RectTransform> ().localScale = new Vector3 (0.5213f, 0.5213f, 0.5213f);

		tmp.transform.Find ("Message").GetComponent<Text> ().text = string.Format ("你抽到了 {0} 個 {1} !!!", count.ToString (), item.ItemName);
		tmp.transform.Find ("MaterialImage").GetComponent<Image> ().sprite = Resources.Load<Sprite> ("2D/" + item.ItemID);

		tmp.transform.Find ("Accept").GetComponent<Button> ().onClick.AddListener (delegate {
			Destroy (tmp);
		});

	}
	
	public void ReverseMaskStatus()
    {
        mask.SetActive(!maskStatus);
        maskStatus = !maskStatus;

        LExpand.Instance.OnClick();
    }

}
