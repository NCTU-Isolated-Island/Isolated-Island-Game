using IsolatedIslandGame.Client.Communication;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Dictionary<int,GameObject> ElementModels = new Dictionary<int, GameObject>(); //Using itemID to sort
    public List<GameObject> ShipModels;

    public Dictionary<int, GameObject> UserGameObject = new Dictionary<int, GameObject>(); //UserID to GO
    public Dictionary<int, GameObject> VesselIDGameObject = new Dictionary<int, GameObject>(); //VesselID to GO
    public Dictionary<int, Dictionary<int, GameObject>> UserDecoration = new Dictionary<int, Dictionary<int, GameObject>>(); // PlayerID to decorationID-decorationGO
    public Dictionary<int, Dictionary<int, GameObject>> VesselDecoration = new Dictionary<int, Dictionary<int, GameObject>>(); // VesselID to decorationID-decorationGO
    public GameObject PlayerGameObject { get; private set; }
    public int PlayerID;

    private bool isInMainScene;

	public delegate void GameAction();
	public static event GameAction OnSetupDone;

    #region Setup

    void Awake()
    {
        if (Instance == null)
        {
			Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
		
    }

    void Start()
    {
		GameUtility.Instance.LoadItemGameObject();

        UserManager.Instance.User.OnPlayerOnline += OnPlayerOnline;
        SceneManager.sceneLoaded += OnSceneLoaded;

        VesselManager.Instance.OnVesselTransformUpdated += OnVesselTransformUpdated;
        VesselManager.Instance.OnVesselDecorationChange += OnVesselDecorationChange;
        VesselManager.Instance.OnVesselChange += OnVesselChange;
    }

    public void Login()
    {
        //SystemManager.Instance.OperationManager.AssignQuestToAllPlayer(1, "TestServer");
        FacebookService.LoginWithFacbook();
        //UserManager.Instance.User.OperationManager.PlayerIDLogin(22, "TestServer");
    }

    

   
    #endregion

    void OnPlayerOnline(Player player)
    {
        UserManager.Instance.User.Player.OnCreateCharacter += OnCreateCharacter;
        if (UserManager.Instance.User.Player.GroupType == GroupType.No)
        {
            LogInUIManager.Instance.ToCreateCharacterPage();
        }
        else
        {
            LogInUIManager.Instance.ToMainScenePrepare();
        }
    }

    void OnCreateCharacter(Player player)
    {
        LogInUIManager.Instance.ToMainScenePrepare();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene == SceneManager.GetSceneByName("MainScene"))
        {
            isInMainScene = true;

            LocationSystem.Instance.StartLocationService();
            SystemManager.Instance.OperationManager.FetchDataResolver.FetchAllVessels();

            foreach (Vessel vessel in VesselManager.Instance.Vessels)
            {
                OnVesselChange(DataChangeType.Add, vessel);
            }

            GetPlayerVesselGameObject();
            GetPlayerID();

            PlayerController.Instance.gameObject.SetActive(true);
            CameraManager.Instance.ToNearAnchor(PlayerGameObject);

			if(OnSetupDone != null)
				OnSetupDone.Invoke();
            
        }
        else
        {
            isInMainScene = false;
        }

    }

    public IEnumerator OnPlayerLocationChange(Vector3 position, float eulerAngleY)
    {
        PlayerGameObject.GetComponent<PlayerBehavior>().UpdateLocation(position, eulerAngleY);

        yield return PlayerController.Instance.GetCurrentArea();

        OceanType type = OceanType.Unknown;

        //目前如果是在海域重疊區域先以Type大的為準
        foreach (GameObject entry in PlayerController.Instance.InArea)
        {
            switch (entry.name)
            {
                case "a_girl_from_osaka_1":
                    type = OceanType.Type1;
                    break;
                case "a_new_generation_1":
                    type = OceanType.Type2;
                    break;
                case "cosy_and_warm_1":
                    type = OceanType.Type3;
                    break;
                case "hopeful_journey_1":
                    type = OceanType.Type4;
                    break;
                case "indy_racing_1":
                    type = OceanType.Type5;
                    break;
                case "lost_soul_1":
                    type = OceanType.Type6;
                    break;
                case "winter_light_1":
                    type = OceanType.Type7;
                    break;
                default:
                    break;
            }


        }


        UserManager.Instance.User.Player.OperationManager.UpdateVesselTransform
        (
            position.x,
            position.z,
            eulerAngleY,
            type
        );
    }

    public void GetPlayerVesselGameObject()
    {
        if (UserGameObject.ContainsKey(UserManager.Instance.User.Player.PlayerID))
        {
            PlayerGameObject = UserGameObject[UserManager.Instance.User.Player.PlayerID];
        }
        else
        {
            Debug.LogError("GetPlayerVesselGameObject Error!");
        }
    }

    void GetPlayerID()
    {
        PlayerID = UserManager.Instance.User.Player.PlayerID;
    }

    #region Vessel

    void OnVesselTransformUpdated(int vesselID, float locationX, float locationZ, float rotationEulerAngleY, OceanType oceanType)
    {
        if (isInMainScene)
        {
            GameObject userVesselGameObject;
            if (VesselIDGameObject.TryGetValue(vesselID, out userVesselGameObject))
            {
                userVesselGameObject.GetComponent<PlayerBehavior>().UpdateLocation
                (
                    new Vector3(locationX, 0f, locationZ),
                    rotationEulerAngleY
                );
            }
            else
            {
                Debug.LogFormat("Vessel: {0}, Not Existed", vesselID);
            }
        }
    }

    void OnVesselDecorationChange(DataChangeType changeType, int vesselID, Decoration decoration)
    {
        if (isInMainScene)
        {
            GameObject userVesselGameObject;
            if (VesselIDGameObject.TryGetValue(vesselID, out userVesselGameObject) && VesselDecoration.ContainsKey(vesselID))
            {
                switch (changeType)
                {
					case DataChangeType.Add:
					{	
						if(!ElementModels.ContainsKey(decoration.Material.ItemID))
							return;

							GameObject decorationGameObject = Instantiate(
								ElementModels[decoration.Material.ItemID],
								userVesselGameObject.transform.Find("Decorations")
							) as GameObject;

							decorationGameObject.transform.localPosition = new Vector3(decoration.PositionX, decoration.PositionY, decoration.PositionZ);
							decorationGameObject.transform.localEulerAngles = new Vector3(decoration.RotationEulerAngleX, decoration.RotationEulerAngleY, decoration.RotationEulerAngleZ);
							decorationGameObject.name = decoration.DecorationID.ToString();

							if (!VesselDecoration[vesselID].ContainsKey(decoration.DecorationID))
							{
								//Find OwnerPlayerID by VesselID
								Vessel v;
								if(VesselManager.Instance.FindVessel(vesselID, out v))
                                {
                                    VesselDecoration[vesselID].Add(decoration.DecorationID, decorationGameObject);
                                }
							}
							else
							{
								Debug.LogFormat("Add Decoration to Vessel Fail, Decoration Already Existed, VesselID: {0}, DecorationID: {1}", vesselID, decoration.DecorationID);
							}
                    }
                    break;
                    case DataChangeType.Remove:
                        {
                            if (VesselDecoration[vesselID].ContainsKey(decoration.DecorationID))
                            {
                                //Find OwnerPlayerID by VesselID
                                Vessel v;
                                if(VesselManager.Instance.FindVessel(vesselID, out v))
                                {
                                    Destroy(VesselDecoration[vesselID][decoration.DecorationID]);
                                    UserDecoration[v.OwnerPlayerID].Remove(decoration.DecorationID);
                                    VesselDecoration[vesselID].Remove(decoration.DecorationID);
                                }
                            }
                            else
                            {
                                Debug.LogFormat("Destroy Decoration Fail Decoration Not On Vessel, VesselID: {0}, DecorationID: {1}", vesselID, decoration.DecorationID);
                            }
                        }
                        break;
                    case DataChangeType.Update:
                        {
                            GameObject decorationGameObject;
                            if (VesselDecoration[vesselID].TryGetValue(decoration.DecorationID, out decorationGameObject))
                            {
                                decorationGameObject.transform.localPosition = new Vector3(decoration.PositionX, decoration.PositionY, decoration.PositionZ);
								decorationGameObject.transform.localEulerAngles = new Vector3(decoration.RotationEulerAngleX, decoration.RotationEulerAngleY, decoration.RotationEulerAngleZ);
                            }
                            else
                            {
                                Debug.LogFormat("Decoration Not On Vessel, VesselID: {0}, DecorationID: {1}", vesselID, decoration.DecorationID);
                            }
                        }
                        break;
                }
            }
            else
            {
                Debug.LogFormat("VesselID: {0}, GameObject Not Existed", vesselID);
            }
        }
    }//當船上的裝飾物更新時的回調事件

    void OnVesselChange(DataChangeType changeType, Vessel vessel)
    {
        if (isInMainScene)
        {
            switch (changeType)
            {
                case DataChangeType.Add:
                {
                    Dictionary<int, GameObject> decorationDictionary = new Dictionary<int, GameObject>();

                    int groupType = 0;
                    PlayerInformation playerInformation;
                    if (PlayerInformationManager.Instance.FindPlayerInformation(vessel.OwnerPlayerID, out playerInformation))
                    {
                        groupType = (int)playerInformation.groupType;
                    }
					
					//Render Player Online Message
					if(UserManager.Instance.User.Player.PlayerID != vessel.OwnerPlayerID && Time.timeSinceLevelLoad > 10f && GameManager.Instance.PlayerGameObject != null)
					{
						UIManager.Instance.RenderPlayerOnlineMessage(
							vessel.OwnerPlayerID,
							Vector3.Distance(GameManager.Instance.PlayerGameObject.gameObject.transform.position,new Vector3(vessel.LocationX,0,vessel.LocationZ))
						);
					}


					//--------

                    GameObject userVesselGameObject = Instantiate
                        (
                            ShipModels[groupType],
                            new Vector3(vessel.LocationX, 0f, vessel.LocationZ),
                            Quaternion.Euler(0f, vessel.RotationEulerAngleY, 0f)
                        ) as GameObject;

                    userVesselGameObject.name = string.Format("OwnerID: {0}", vessel.OwnerPlayerID);
					
					AssignInfoToPlayerBehavior(vessel.OwnerPlayerID,userVesselGameObject);



                    if(vessel.OwnerPlayerID == UserManager.Instance.User.Player.PlayerID)
                    {
                        userVesselGameObject.tag = "SelfVessel";
                        OceanController.Instance.SelfVesselTransform = userVesselGameObject.transform;
                    }

	                foreach (Decoration decoration in vessel.Decorations)
	                {
						if(!ElementModels.ContainsKey(decoration.Material.ItemID))
							continue;
					
	                    if (!decorationDictionary.ContainsKey(decoration.DecorationID))
	                    {
	                        GameObject decorationGameObject = Instantiate(
	                            ElementModels[decoration.Material.ItemID],
	                            userVesselGameObject.transform.Find("Decorations")
	                        ) as GameObject;

	                        decorationGameObject.transform.localPosition = new Vector3(decoration.PositionX, decoration.PositionY, decoration.PositionZ);
	                        decorationGameObject.transform.localEulerAngles = new Vector3(decoration.RotationEulerAngleX, decoration.RotationEulerAngleY, decoration.RotationEulerAngleZ);
	                        decorationGameObject.name = decoration.DecorationID.ToString();

	                        decorationDictionary.Add(decoration.DecorationID, decorationGameObject);
	                    }
	                }
		

                    if (!UserGameObject.ContainsKey(vessel.OwnerPlayerID))
                        UserGameObject.Add(vessel.OwnerPlayerID, userVesselGameObject);
                    if (!VesselIDGameObject.ContainsKey(vessel.VesselID))
                        VesselIDGameObject.Add(vessel.VesselID, userVesselGameObject);
                    if (!UserDecoration.ContainsKey(vessel.OwnerPlayerID))
                        UserDecoration.Add(vessel.OwnerPlayerID, decorationDictionary);
                    if (!VesselDecoration.ContainsKey(vessel.VesselID))
                        VesselDecoration.Add(vessel.VesselID, decorationDictionary);
                }
                    break;
                case DataChangeType.Remove:
                {
                    GameObject userVesselGameObject;
                    if (VesselIDGameObject.TryGetValue(vessel.VesselID, out userVesselGameObject))
                    {
						if(PlayerController.Instance.CurrentFocusPlayerGameObject == UserGameObject[vessel.OwnerPlayerID])
						{
							CameraManager.Instance.ToNearAnchor(PlayerGameObject);
						}

                        UserGameObject.Remove(vessel.OwnerPlayerID);
                        VesselIDGameObject.Remove(vessel.VesselID);
                        UserDecoration.Remove(vessel.OwnerPlayerID);
                        VesselDecoration.Remove(vessel.VesselID);

                        Destroy(userVesselGameObject);
                        if (vessel.OwnerPlayerID == UserManager.Instance.User.Player.PlayerID)
                        {
                            OceanController.Instance.SelfVesselTransform = null;
                        }
                    }
                }
                    break;
                case DataChangeType.Update:
                {
                    OnVesselChange(DataChangeType.Remove, vessel);
                    OnVesselChange(DataChangeType.Add, vessel);
                }
                    break;
            }
        }
    } //當船物件有變化時的回調事件

    #endregion


	void AddDecorationToVessel(int vessleID, Decoration decoration)
	{
		
	}

	void AssignInfoToPlayerBehavior(int ownerPlayerID, GameObject vesselGameObject)
	{
		PlayerInformation info;
		if (PlayerInformationManager.Instance.FindPlayerInformation(ownerPlayerID, out info))
		{
			vesselGameObject.GetComponent<PlayerBehavior>().playerName = info.nickname;
		}

		vesselGameObject.GetComponent<PlayerBehavior>().playerID = ownerPlayerID;

	}


}
