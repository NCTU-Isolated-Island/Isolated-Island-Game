using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using IsolatedIslandGame.Client;
using IsolatedIslandGame.Client.Communication;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;

public class PlayerController : MonoBehaviour {

	private bool finishPlacing = false; // 完成放置素材
	private bool placingMaterial = false; //正在放置素材
	private float lastTimeClick = -99f ;

	public static PlayerController Instance;
	public List<GameObject> InArea;
	public GameObject CurrentSelectDecoration;
	public GameObject CurrentFocusPlayerGameObject;

	public int ModelOrientMode = 0;
	public enum ViewMode{ FirstPerson, BirdView, NormalView }
	public ViewMode CurrenViewMode = ViewMode.NormalView;

	public enum ControlMode{ Normal, Decorate, Rotate}
	public ControlMode CurrentControlMode = ControlMode.Normal;
	float clickTime = -99f;

	public List<GameObject> ModifiedDecorations = new List<GameObject>();
	public List<GameObject> AddedDecorations = new List<GameObject>();
	public List<GameObject> RemovedDecorations = new List<GameObject>();

	public delegate void PlayerAction();
	public static event PlayerAction OnGetArea;

	void Awake()
	{
		if(Instance == null){
			Instance = this;
		}else if(Instance != this){
			Destroy(gameObject);
		}

		Input.gyro.enabled = true;

		//After Setting Up, Deactivate PlayerController, and wait for into MainScene
		gameObject.SetActive(false);



	}

	void Update()
	{
		

	
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			ChangeViewMode(ViewMode.BirdView);
		}
		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			ChangeViewMode(ViewMode.FirstPerson);
		}
		if(Input.GetKeyDown(KeyCode.Alpha3))
		{
			ChangeViewMode(ViewMode.NormalView);
		}

		if(Input.GetKeyDown(KeyCode.Alpha4))
		{
			CurrentControlMode = ControlMode.Normal;
		}
		if(Input.GetKeyDown(KeyCode.Alpha5))
		{
			CurrentControlMode = ControlMode.Decorate;
		}

		if(Input.GetKeyDown(KeyCode.Alpha6))
		{
			UpdateModifiedDecorationsToServer();
		}
			
		if(CurrentControlMode == ControlMode.Decorate || CurrentControlMode == ControlMode.Rotate)
		{
			DecorateProcess();
		}

		if(CurrentControlMode == ControlMode.Rotate)
		{
			if(Input.touchCount == 1)
			{
				RotateArountY(Input.touches[0].deltaPosition.x * -0.2f);

			}
		}

		CheckDoubleClick();

		if(CurrentControlMode == ControlMode.Normal)
		{
			if(CurrenViewMode != ViewMode.FirstPerson)
			{
				AdjustViewAngle();

			}
		}

		if(Input.GetKeyDown(KeyCode.Alpha0))
		{
			int userID = 24;

			AuxCameraSystem.Instance.Show3DObject(GameManager.Instance.UserGameObject[userID],
				new Vector3(0,-10.3f,40.8f),Quaternion.Euler(12.8f,0,0));
		}

		PinchToZoom();
	}

	public IEnumerator GetCurrentArea()
	{
		PlayerController.Instance.InArea.Clear(); // 清除上次掃描記錄
		GameObject probe = Instantiate
			(
				Resources.Load("AreaProbe"), GameManager.Instance.PlayerGameObject.transform.position, Quaternion.identity
			) as GameObject;

		yield return new WaitForFixedUpdate();
		yield return new WaitForFixedUpdate();

		Destroy(probe);

		if(OnGetArea != null)
			OnGetArea.Invoke();
	}

	public void StartPlaceDecoration()
	{
		//Check have that material
		finishPlacing = false;
		StartCoroutine(PlaceMaterial(3));

	}

	public void FinishPlaceDecoration()
	{
		finishPlacing = true;
	}

	IEnumerator PlaceMaterial(int itemID)
	{
		placingMaterial = true;

		Vector3 position;
		Quaternion rotation;

		GameObject temp = Instantiate(GameManager.Instance.elementModels[itemID],Vector3.zero,Quaternion.identity) as GameObject;
		temp.transform.SetParent(GameManager.Instance.PlayerGameObject.transform);
		Transparentize(temp);

		RaycastHit hitInfo = new RaycastHit();

		while(!finishPlacing)
		{
			//TODO 必須把Player模型設定成 "PlayerModel" Layer
			Physics.Raycast
			(
				Camera.main.ScreenPointToRay(Input.mousePosition),
				out hitInfo,
				99999f,
				LayerMask.GetMask("PlayerModel")
			);
			temp.transform.position = hitInfo.point;
			yield return null;}


		position = temp.transform.localPosition;
		rotation = temp.transform.localRotation;

		UserManager.Instance.User.Player.OperationManager.AddDecorationToVessel
		(
			itemID,
			position.x, position.y, position.z,
			rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z
		);


		Destroy(temp);
		placingMaterial = false;

	}

	void CheckDoubleClick()
	{
		if(Input.GetMouseButtonDown(0))
		{
			if((Time.time - lastTimeClick) < 0.25f) 
			{
				//Player has double clicked
				CheckIfSelectVessel();

			}	

			lastTimeClick = Time.time;
		}
	}

	void CheckIfSelectVessel()
	{
		RaycastHit hitInfo = new RaycastHit();
		bool hit = Physics.Raycast(
			Camera.main.ScreenPointToRay(Input.mousePosition),
			out hitInfo,
			99999f,
			LayerMask.GetMask("PlayerModel")
		);

		//Select Vessel
		if(hit && !GameObject.Equals(hitInfo.transform.root.gameObject,GameManager.Instance.PlayerGameObject))
		{
			print("Select " + hitInfo.collider.transform.root.name + " Vessel");

			CameraManager.Instance.ToNearAnchor(hitInfo.transform.root.gameObject);


			string[] a =  hitInfo.transform.name.Split(' ');
			int id = System.Int32.Parse(a[1]);

			CurrentFocusPlayerGameObject = GameManager.Instance.UserGameObject[id];

		}

	}

	public void RemoveDecoration(GameObject target)
	{
		UserManager.Instance.User.Player.OperationManager.RemoveDecorationFromVessel(System.Int32.Parse(target.name));
	}

	void AdjustViewAngle()
	{

		if(Input.touchCount == 1)
		{
			Touch touch = Input.GetTouch(0);
			float x = touch.deltaPosition.x * 0.15f;
			float y = touch.deltaPosition.y * -0.1f;

			CameraManager.Instance.CameraRotate(x);
			CameraManager.Instance.CameraRotateHorizontal(y);
		}

	}

	public void ToPlayerFarAnchor()
	{
		CameraManager.Instance.ToFarAnchor(GameManager.Instance.PlayerGameObject);
		CurrentFocusPlayerGameObject = GameManager.Instance.PlayerGameObject;
	}

	public void ToPlayerNearAnchor()
	{
		CameraManager.Instance.ToNearAnchor(GameManager.Instance.PlayerGameObject);
		CurrentFocusPlayerGameObject = GameManager.Instance.PlayerGameObject;

	}

	void SelectDecoration()
	{

		RaycastHit hitInfo = new RaycastHit();

		bool hit = Physics.Raycast(
			Camera.main.ScreenPointToRay(Input.mousePosition),
			out hitInfo,
			99999f,
			LayerMask.GetMask("Decoration")
		);

		if(hit)
		{

			if(CurrentSelectDecoration)
			{
				DeTransparentize(CurrentSelectDecoration);
			}


			CurrentSelectDecoration = hitInfo.transform.gameObject;

			Transparentize(CurrentSelectDecoration);

		}
		else
		{
			//CurrentSelectDecoration = null;


		}


	}

	void PinchToZoom()
	{
		if (Input.touchCount == 2)
		{
			// Store both touches.
			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);

			// Find the position in the previous frame of each touch.
			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			// Find the magnitude of the vector (the distance) between the touches in each frame.
			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

			// Find the difference in the distances between each frame.
			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;


			CameraManager.Instance.Zoom((deltaMagnitudeDiff * 0.001f) + 1);


		}
	}

	public void RemoveAllDecoration()
	{
		Vessel vessel;
		VesselManager.Instance.FindVesselByOwnerPlayerID(UserManager.Instance.User.Player.PlayerID,out vessel);

		foreach(Decoration entry in vessel.Decorations)
		{
			UserManager.Instance.User.Player.OperationManager.RemoveDecorationFromVessel(entry.DecorationID);
		}


	}

	void Transparentize(GameObject target)
	{

		foreach(UnityEngine.Material entry in target.GetComponent<MeshRenderer>().materials)
		{
			entry.color = new Color( entry.color.r, entry.color.g, entry.color.b, 0.5f);
		}
	}

	void DeTransparentize(GameObject target)
	{
		foreach(UnityEngine.Material entry in target.GetComponent<MeshRenderer>().materials)
		{
			entry.color = new Color( entry.color.r, entry.color.g, entry.color.b, 1f);
		}
	}


	public void ChangeModelOrientation()
	{
		if(CurrentSelectDecoration == null)
			return;
		
		switch (ModelOrientMode) {
		case 1:
			CurrentSelectDecoration.transform.rotation = Quaternion.Euler(0,0,0);
			ModelOrientMode = 2;
			break;
		case 2:
			CurrentSelectDecoration.transform.rotation = Quaternion.Euler(180,0,0);
			ModelOrientMode = 3;
			break;
		case 3:
			CurrentSelectDecoration.transform.rotation = Quaternion.Euler(0,0,90);
			ModelOrientMode = 4;
			break;
		case 4:
			CurrentSelectDecoration.transform.rotation = Quaternion.Euler(0,0,-90);
			ModelOrientMode = 5;
			break;
		case 5:
			CurrentSelectDecoration.transform.rotation = Quaternion.Euler(90,0,0);
			ModelOrientMode = 6;
			break;
		case 6:
			CurrentSelectDecoration.transform.rotation = Quaternion.Euler(-90,0,0);
			ModelOrientMode = 1;
			break;
		default:
			break;
		}
	}

	public void RotateArountY(float degree)
	{
		CurrentSelectDecoration.transform.Rotate(0,degree,0,Space.World);
	}

	void ShowVessel(GameObject vessel)
	{
		vessel.transform.Find("ShipModel").gameObject.SetActive(true);
		vessel.transform.Find("Decorations").gameObject.SetActive(true);
	}

	void UnShowVessel(GameObject vessel)
	{
		vessel.transform.Find("ShipModel").gameObject.SetActive(false);
		vessel.transform.Find("Decorations").gameObject.SetActive(false);
	}

	public void OnlyShowFriendsVessel()
	{
		List<int> friendsID = new List<int>();
		friendsID.Add(UserManager.Instance.User.Player.PlayerID);
		foreach(FriendInformation entry in UserManager.Instance.User.Player.FriendInformations)
		{
			friendsID.Add(entry.friendPlayerID);
		}

		foreach(KeyValuePair<int,GameObject> entry in GameManager.Instance.UserGameObject)
		{
			if(!friendsID.Contains(entry.Key))
			{
				UnShowVessel(entry.Value);
			}	
		}

	}

	public void ShowAllVessel()
	{

		foreach(GameObject entry in GameManager.Instance.UserGameObject.Values)
		{
			ShowVessel(entry);	
		}

	}

	public void ChangeViewMode(ViewMode mode)
	{
		  
		switch (mode) {
		case ViewMode.FirstPerson:
			//Maybe need CameraManager.to first person anchor
			CameraManager.Instance.ToFirstPerson(GameManager.Instance.PlayerGameObject);
			//Camera.main.transform.position = GameManager.Instance.PlayerGameObject.transform.Find("FirstPersonAnchor").position;
			FirstPersonCameraController.Instance.enabled = true;
			CurrenViewMode = ViewMode.FirstPerson;
			break;

		case ViewMode.NormalView:
			CameraManager.Instance.ToNearAnchor(GameManager.Instance.PlayerGameObject);
			FirstPersonCameraController.Instance.enabled = false;
			CurrenViewMode = ViewMode.NormalView;
			break;

		case ViewMode.BirdView:
			CameraManager.Instance.ToFarAnchor(GameManager.Instance.PlayerGameObject);
			FirstPersonCameraController.Instance.enabled = false;
			CurrenViewMode = ViewMode.BirdView;
			break;
		default:
			break;
		}

	}
		
	//Final Stage After Clicking Done Button
	public void UpdateModifiedDecorationsToServer()
	{
		foreach(GameObject entry in AddedDecorations)
		{
			UserManager.Instance.User.Player.OperationManager.AddDecorationToVessel
			(
				System.Int32.Parse(entry.name),
				entry.transform.localPosition.x, entry.transform.localPosition.y, entry.transform.localPosition.z,
				entry.transform.rotation.eulerAngles.x, entry.transform.rotation.eulerAngles.y, entry.transform.rotation.eulerAngles.z
			);
		}

		foreach(GameObject entry in ModifiedDecorations)
		{
			UserManager.Instance.User.Player.OperationManager.UpdateDecorationOnVessel
			(
				System.Int32.Parse(entry.name),
				entry.transform.localPosition.x, entry.transform.localPosition.y, entry.transform.localPosition.z,
				entry.transform.rotation.eulerAngles.x, entry.transform.rotation.eulerAngles.y, entry.transform.rotation.eulerAngles.z
			);
		}

		foreach(GameObject entry in RemovedDecorations)
		{
			UserManager.Instance.User.Player.OperationManager.RemoveDecorationFromVessel(System.Int32.Parse(entry.name));
		}

		//Clear All 
		ModifiedDecorations.Clear();
		AddedDecorations.Clear();
		RemovedDecorations.Clear();
	}

	public void BeginDec(int itemID)
	{
		

		CurrentSelectDecoration = Instantiate(GameManager.Instance.elementModels[itemID],Vector3.zero,Quaternion.identity) as GameObject;

		if(!AddedDecorations.Contains(CurrentSelectDecoration))
			AddedDecorations.Add(CurrentSelectDecoration);

		CurrentControlMode = ControlMode.Decorate;

	}

	void DecorateProcess()
	{
		if(Input.touchCount == 1)
		{
			#region HitRaycaster
			bool entireHit;
			RaycastHit entireHitInfo = new RaycastHit();

			entireHit = Physics.Raycast
				(
					Camera.main.ScreenPointToRay(Input.mousePosition),
					out entireHitInfo,
					99999f
				);

			bool vesselHit;
			RaycastHit vesselHitInfo = new RaycastHit();
			vesselHit = Physics.Raycast
				(
					Camera.main.ScreenPointToRay(Input.mousePosition),
					out vesselHitInfo,
					99999f,
					LayerMask.GetMask("PlayerModel")
				);
			#endregion

			if(Input.touches[0].phase == TouchPhase.Began)
			{
				clickTime = Time.time;

				if(CurrentControlMode == ControlMode.Rotate && 
					entireHitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Decoration") &&
					entireHitInfo.transform.gameObject != CurrentSelectDecoration)
				{
					CurrentSelectDecoration.transform.localScale *= 0.5f;
				}

				if(entireHit && entireHitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Decoration"))
				{
					CurrentSelectDecoration = entireHitInfo.transform.gameObject;
					CurrentControlMode = ControlMode.Decorate;
					if(!ModifiedDecorations.Contains(CurrentSelectDecoration))
						ModifiedDecorations.Add(CurrentSelectDecoration);
				}

			}

			if(Input.touches[0].phase == TouchPhase.Stationary || Input.touches[0].phase == TouchPhase.Moved)
			{

			}

			if(Input.touches[0].phase == TouchPhase.Ended)
			{
				float deltaTime = Time.time - clickTime;
				clickTime = -99f;


				if(deltaTime < 0.5f) // 短按
				{
					print("short");
					CurrentControlMode = ControlMode.Rotate;
					if(entireHitInfo.transform.gameObject != CurrentSelectDecoration)
					{
						CurrentSelectDecoration.transform.localScale *= 2f;

					}
				}
				else // 長按
				{
					if(CurrentControlMode == ControlMode.Decorate)
					{
						if(vesselHit)
						{
							CurrentSelectDecoration = null;
						}
						else
						{
							if(!RemovedDecorations.Contains(CurrentSelectDecoration))
								RemovedDecorations.Add(CurrentSelectDecoration);

							CurrentSelectDecoration.SetActive(false);

							CurrentSelectDecoration = null;
						}
					}

				}



			}

			if(CurrentSelectDecoration != null && CurrentControlMode != ControlMode.Rotate)
			{
				if(vesselHit)
				{
					CurrentSelectDecoration.transform.position = vesselHitInfo.point;
				}
				else
				{
					CurrentSelectDecoration.transform.position = 
						Camera.main.transform.position + Camera.main.ScreenPointToRay(Input.mousePosition).direction * 9;
				}
			}
		}

		if(Input.touchCount == 2)
		{
			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);

			float x = (touchZero.deltaPosition.x + touchOne.deltaPosition.x) * 0.075f;
			float y = (touchZero.deltaPosition.x + touchOne.deltaPosition.x) * -0.05f;

			CameraManager.Instance.CameraRotate(x);
			CameraManager.Instance.CameraRotateHorizontal(y);

		}
	}


}

