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

    public Dictionary<int,GameObject> elementModels = new Dictionary<int, GameObject>(); //Using itemID to sort
    public List<GameObject> ShipModels;

    public Dictionary<int, GameObject> UserGameObject = new Dictionary<int, GameObject>(); //UserID to GO
    public Dictionary<int, GameObject> VesselIDGameObject = new Dictionary<int, GameObject>(); //VesselID to GO
    public Dictionary<int, Dictionary<int, GameObject>> UserDecoration = new Dictionary<int, Dictionary<int, GameObject>>(); // PlayerID to decorationID-decorationGO
    public Dictionary<int, Dictionary<int, GameObject>> VesselDecoration = new Dictionary<int, Dictionary<int, GameObject>>(); // VesselID to decorationID-decorationGO
    public GameObject PlayerGameObject { get; private set; }
    public int PlayerID;

    private bool isInMainScene;


    public enum CameraStat
    {
        Near,
        Far
    }
    private CameraStat cameraStat;

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
		LoadItemGameObject();
    }

    void Start()
    {
        StartCoroutine(SlowUpdate());

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
        //UserManager.Instance.User.OperationManager.PlayerIDLogin(37, "TestServer");
    }

    void LoadItemGameObject()
    {
        // itemID to item path in resources
		elementModels.Add(1002, Resources.Load("Ingredients/" + "Speaker") as GameObject);
		elementModels.Add(1004, Resources.Load("Ingredients/" + "take_copter_1") as GameObject);
		elementModels.Add(1017, Resources.Load("Ingredients/" + "stone") as GameObject);
		elementModels.Add(1009, Resources.Load("Ingredients/" + "rope") as GameObject);
		elementModels.Add(1023, Resources.Load("Ingredients/" + "pigment") as GameObject);
		elementModels.Add(1024, Resources.Load("Ingredients/" + "gold") as GameObject);
		elementModels.Add(1010, Resources.Load("Ingredients/" + "coal") as GameObject);
		elementModels.Add(1025, Resources.Load("Ingredients/" + "knife") as GameObject);
        elementModels.Add(2005, Resources.Load("Ingredients/" + "basket") as GameObject);
		elementModels.Add(1018, Resources.Load("Ingredients/" + "water") as GameObject);
		elementModels.Add(1006, Resources.Load("Ingredients/" + "wood") as GameObject);
		elementModels.Add(1021, Resources.Load("Ingredients/" + "milk") as GameObject);
        elementModels.Add(1011, Resources.Load("Ingredients/" + "fire") as GameObject);
		elementModels.Add(1007, Resources.Load("Ingredients/" + "steel") as GameObject);
		elementModels.Add(1035, Resources.Load("Ingredients/" + "oil") as GameObject);

		elementModels.Add(3015, Resources.Load("Ingredients/" + "Computer") as GameObject);
		elementModels.Add(3016, Resources.Load("Ingredients/" + "small_light") as GameObject);
        elementModels.Add(2012, Resources.Load("Ingredients/" + "guitar") as GameObject);
		elementModels.Add(2018, Resources.Load("Ingredients/" + "electricity") as GameObject);
        
		elementModels.Add(4017, Resources.Load("Ingredients/" + "imac") as GameObject);

    }

    IEnumerator SlowUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
        }
    }

    #endregion
    void OnGetPlayerConversation(IsolatedIslandGame.Library.TextData.PlayerConversation conversation)
    {
        //print(conversation.message.senderPlayerID + " : " +conversation.message.content );
    }

    void OnPlayerOnline(Player player)
    {
        UserManager.Instance.User.Player.OnCreateCharacter += OnCreateCharacter;
        UserManager.Instance.User.Player.OnGetPlayerConversation += OnGetPlayerConversation;
        if (UserManager.Instance.User.Player.GroupType == GroupType.No)
        {
            LogInUIManager.Instance.ToCreateCharacterPage();
            //SceneManager.LoadScene("RegisterScene");
            //LogInUIManager.Instance.ToCreateCharacterPage();

            //Create Charater by Uimanager ? (probably
            //UserManager.Instance.User.Player.OperationManager.CreateCharacter("ABC","signature", GroupType.Businessman);
        }
        else
        {

            //UImanager.Instance.LoadResult(0);
            //UIManager.Instance.LoadResult(0);
            LogInUIManager.Instance.ToMainScenePrepare();
        }
    }

    void OnCreateCharacter(Player player)
    {
        SceneManager.LoadScene("MainScene");
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
        }
        else
        {
            isInMainScene = false;
        }

        //TODO 設定完之後再把顯示出遊戲畫面（在這之前可能顯示loading bar
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

    void Register(string nickname, string signature, GroupType groupType)
    {
        UserManager.Instance.User.Player.OperationManager.CreateCharacter(nickname, signature, groupType);
    }

    void GetPlayerVesselGameObject()
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
					{print(decoration.Material.ItemID);
                            GameObject decorationGameObject = Instantiate(
                                elementModels[decoration.Material.ItemID],
                                userVesselGameObject.transform.Find("Decorations")
                            ) as GameObject;

                            decorationGameObject.transform.localPosition = new Vector3(decoration.PositionX, decoration.PositionY, decoration.PositionZ);
                            decorationGameObject.transform.localEulerAngles = new Vector3(decoration.RotationEulerAngleX, decoration.RotationEulerAngleY, decoration.RotationEulerAngleZ);
                            decorationGameObject.name = decoration.DecorationID.ToString();

                            if (!VesselDecoration[vesselID].ContainsKey(decoration.DecorationID))
                            {
                                //Find OwnerPlayerID by VesselID
                                Vessel v;
                                VesselManager.Instance.FindVessel(vesselID, out v);

                                VesselDecoration[vesselID].Add(decoration.DecorationID, decorationGameObject);

                                //TODO for some reason UserDecoration[v.PlayerInformation.playerID] already
                                // contain key "decoration.DecorationID"

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
                                VesselManager.Instance.FindVessel(vesselID, out v);

                                Destroy(VesselDecoration[vesselID][decoration.DecorationID]);

                                UserDecoration[v.OwnerPlayerID].Remove(decoration.DecorationID);
                                VesselDecoration[vesselID].Remove(decoration.DecorationID);
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
                                decorationGameObject.transform.localRotation = Quaternion.Euler(decoration.RotationEulerAngleX, decoration.RotationEulerAngleY, decoration.RotationEulerAngleZ);
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

                        GameObject userVesselGameObject = Instantiate
                            (
                                ShipModels[groupType],
                                new Vector3(vessel.LocationX, 0f, vessel.LocationZ),
                                Quaternion.Euler(0f, vessel.RotationEulerAngleY, 0f)
                            ) as GameObject;

                        userVesselGameObject.name = string.Format("OwnerID: {0}", vessel.OwnerPlayerID);

                        foreach (Decoration decoration in vessel.Decorations)
                        {
                            if (!decorationDictionary.ContainsKey(decoration.DecorationID))
                            {
                                GameObject decorationGameObject = Instantiate(
                                    elementModels[decoration.Material.ItemID],
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
								CameraManager.Instance.ToFarAnchor(PlayerGameObject);
							}

                            UserGameObject.Remove(vessel.OwnerPlayerID);
                            VesselIDGameObject.Remove(vessel.VesselID);
                            UserDecoration.Remove(vessel.OwnerPlayerID);
                            VesselDecoration.Remove(vessel.VesselID);

                            Destroy(userVesselGameObject);
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



}
