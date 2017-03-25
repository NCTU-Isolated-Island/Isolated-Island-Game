﻿using IsolatedIslandGame.Client.Communication;
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
        //UserManager.Instance.User.OperationManager.PlayerIDLogin(22, "TestServer");
    }

    void LoadItemGameObject()
    {
        elementModels.Add(   1, Resources.Load("Ingredients/" + "trash") as GameObject);

        elementModels.Add(1005, Resources.Load("Ingredients/" + "stone") as GameObject);
		elementModels.Add(1006, Resources.Load("Ingredients/" + "Wheat") as GameObject);
		elementModels.Add(1007, Resources.Load("Ingredients/" + "Egg") as GameObject);
		elementModels.Add(1008, Resources.Load("Ingredients/" + "CoffeeBean") as GameObject);
        elementModels.Add(1009, Resources.Load("Ingredients/" + "pineapple") as GameObject);
        elementModels.Add(1010, Resources.Load("Ingredients/" + "apple") as GameObject);
        elementModels.Add(1011, Resources.Load("Ingredients/" + "water") as GameObject);
        elementModels.Add(1013, Resources.Load("Ingredients/" + "fire") as GameObject);

		elementModels.Add(1016, Resources.Load("Ingredients/" + "coal") as GameObject);


		elementModels.Add(1018, Resources.Load("Ingredients/" + "Patato") as GameObject);
		//elementModels.Add(1019, Resources.Load("Ingredients/" + "wood") as GameObject);

		elementModels.Add(1021, Resources.Load("Ingredients/" + "oil") as GameObject);
		elementModels.Add(1022, Resources.Load("Ingredients/" + "fish") as GameObject);
		elementModels.Add(1023, Resources.Load("Ingredients/" + "Sponge") as GameObject);

        elementModels.Add(2001, Resources.Load("Ingredients/" + "Silicone") as GameObject);
        elementModels.Add(2002, Resources.Load("Ingredients/" + "Ball") as GameObject);
        elementModels.Add(2003, Resources.Load("Ingredients/" + "rope") as GameObject);
        elementModels.Add(2005, Resources.Load("Ingredients/" + "StoneMill") as GameObject);
        elementModels.Add(2006, Resources.Load("Ingredients/" + "milk") as GameObject);
        elementModels.Add(2007, Resources.Load("Ingredients/" + "badminton") as GameObject);
        elementModels.Add(2008, Resources.Load("Ingredients/" + "knife") as GameObject);
        elementModels.Add(2009, Resources.Load("Ingredients/" + "Sugar") as GameObject);

        elementModels.Add(2011, Resources.Load("Ingredients/" + "pigment") as GameObject);
        elementModels.Add(2012, Resources.Load("Ingredients/" + "Pot") as GameObject);
        elementModels.Add(2013, Resources.Load("Ingredients/" + "Flour") as GameObject);
		elementModels.Add(2014, Resources.Load("Ingredients/" + "pencil") as GameObject);

		elementModels.Add(2016, Resources.Load("Ingredients/" + "electricity") as GameObject);
		elementModels.Add(2017, Resources.Load("Ingredients/" + "Propeller") as GameObject);

		elementModels.Add(2019, Resources.Load("Ingredients/" + "gold") as GameObject);
        elementModels.Add(2020, Resources.Load("Ingredients/" + "Candle") as GameObject);

        elementModels.Add(3001, Resources.Load("Ingredients/" + "Soup") as GameObject);
        elementModels.Add(3002, Resources.Load("Ingredients/" + "steel") as GameObject);
		elementModels.Add(3003, Resources.Load("Ingredients/" + "Chip") as GameObject);

		elementModels.Add(3005, Resources.Load("Ingredients/" + "Pillow") as GameObject);

		elementModels.Add(3008, Resources.Load("Ingredients/" + "Latte") as GameObject);
        elementModels.Add(3009, Resources.Load("Ingredients/" + "Coke") as GameObject);
        elementModels.Add(3011, Resources.Load("Ingredients/" + "Painter") as GameObject);
        elementModels.Add(3012, Resources.Load("Ingredients/" + "guitar") as GameObject);
		elementModels.Add(3013, Resources.Load("Ingredients/" + "Ghost") as GameObject);
		elementModels.Add(3014, Resources.Load("Ingredients/" + "small_light") as GameObject);

		elementModels.Add(3017, Resources.Load("Ingredients/" + "Oven") as GameObject);
		elementModels.Add(3018, Resources.Load("Ingredients/" + "take_copter_1") as GameObject);
		elementModels.Add(3019, Resources.Load("Ingredients/" + "Bread") as GameObject);

        elementModels.Add(4004, Resources.Load("Ingredients/" + "badminton_racket") as GameObject);
		elementModels.Add(4005, Resources.Load("Ingredients/" + "Computer") as GameObject);
        elementModels.Add(4006, Resources.Load("Ingredients/" + "Motor") as GameObject);

		elementModels.Add(4009, Resources.Load("Ingredients/" + "ShineMood") as GameObject);
        elementModels.Add(4010, Resources.Load("Ingredients/" + "black_horse") as GameObject);

        elementModels.Add(4013, Resources.Load("Ingredients/" + "imac") as GameObject);


		elementModels.Add(4016, Resources.Load("Ingredients/" + "Fries") as GameObject);


        // check
        foreach (var element in elementModels)
        {
            if (element.Value == null)
                print(element.Key);
        }


	
    }

    IEnumerator SlowUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
        }
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

    void Register(string nickname, string signature, GroupType groupType)
    {
        UserManager.Instance.User.Player.OperationManager.CreateCharacter(nickname, signature, groupType);
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
						if(!elementModels.ContainsKey(decoration.Material.ItemID))
							return;

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

                        GameObject userVesselGameObject = Instantiate
                            (
                                ShipModels[groupType],
                                new Vector3(vessel.LocationX, 0f, vessel.LocationZ),
                                Quaternion.Euler(0f, vessel.RotationEulerAngleY, 0f)
                            ) as GameObject;

                        userVesselGameObject.name = string.Format("OwnerID: {0}", vessel.OwnerPlayerID);
						
						PlayerInformation info;
						if (PlayerInformationManager.Instance.FindPlayerInformation(vessel.OwnerPlayerID, out info))
						{
							userVesselGameObject.GetComponent<PlayerBehavior>().playerName = info.nickname;
						}
						userVesselGameObject.GetComponent<PlayerBehavior>().playerID = vessel.OwnerPlayerID;

                        if(vessel.OwnerPlayerID == UserManager.Instance.User.Player.PlayerID)
                        {
                            userVesselGameObject.tag = "SelfVessel";
                            OceanController.Instance.SelfVesselTransform = userVesselGameObject.transform;
                        }

                        foreach (Decoration decoration in vessel.Decorations)
                        {
							if(!elementModels.ContainsKey(decoration.Material.ItemID))
								continue;
						
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

}
