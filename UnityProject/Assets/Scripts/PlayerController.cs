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

	void Awake()
	{
		if(Instance == null){
			Instance = this;
		}else if(Instance != this){
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);

		//After Setting Up, Deactivate PlayerController, and wait for into MainScene
		gameObject.SetActive(false);
	}
	void Update()
	{

		if(Input.GetKeyDown(KeyCode.P))
		{
			StartPlaceDecoration();

		}

		if(Input.GetKeyDown(KeyCode.D))
		{
			FinishPlaceDecoration();
		}

		CheckDoubleClick();

		AdjustViewAngle();
		PinchToZoom();
	}
		
	public void StartPlaceDecoration()
	{
		//Check have that material
		finishPlacing = false;
		StartCoroutine(PlaceMaterial(2));

	}

	public void FinishPlaceDecoration()
	{
		finishPlacing = true;
	}
		
	IEnumerator PlaceMaterial(int materialID)
	{
		placingMaterial = true;

		Vector3 position;
		Quaternion rotation;

		GameObject temp = Instantiate(GameManager.Instance.ElementModel[materialID],Vector3.zero,Quaternion.identity) as GameObject;
		temp.transform.SetParent(GameManager.Instance.PlayerGameObject.transform);
		RaycastHit hitInfo = new RaycastHit();

		while(!finishPlacing)
		{
			//TODO 必須把Player模型設定成 "PlayerModel" Layer
			Physics.Raycast(
				Camera.main.ScreenPointToRay(Input.mousePosition),
				out hitInfo,
				99999f,
				LayerMask.GetMask("PlayerModel")
			);
			temp.transform.position = hitInfo.point;
			yield return null;
		}


		position = temp.transform.localPosition;
		rotation = temp.transform.localRotation;

		UserManager.Instance.User.Player.OperationManager.AddDecorationToVessel
		(
			materialID,
			position.x, position.y, position.z,
			rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z
		);

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
			UImanager.Instance.GameUI = UImanager.UI.Other_Boat;
		}

	}


	//在FarAnchor的時候不能執行旋轉
	void AdjustViewAngle()
	{
		
		if(Input.touchCount == 1 && !placingMaterial)
		{
			Touch touch = Input.GetTouch(0);
			float x = touch.deltaPosition.x * 0.15f;
			float y = touch.deltaPosition.y * 0.15f;

			CameraManager.Instance.CameraRotate(x);

		}

	}

	public void ToPlayerFarAnchor()
	{
		CameraManager.Instance.ToFarAnchor(GameManager.Instance.PlayerGameObject);
		UImanager.Instance.GameUI = UImanager.UI.Map;
	}

	public void ToPlayerNearAnchor()
	{
		CameraManager.Instance.ToNearAnchor(GameManager.Instance.PlayerGameObject);
		UImanager.Instance.GameUI = UImanager.UI.Main_Boat;
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

			print(deltaMagnitudeDiff);
			CameraManager.Instance.Zoom((deltaMagnitudeDiff * 0.001f) + 1);


		}
	}
//
//	//Still Have Bugs
//	void TwoFingersRotate()
//	{
//		if(Input.touchCount == 2)
//		{
//			Touch touchZero = Input.GetTouch(0);
//			Touch touchOne = Input.GetTouch(1);
//
//			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
//			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
//
//			Vector2 previousVector = (touchZeroPrevPos - touchOnePrevPos);
//			Vector2 currentVector = (touchZero.position - touchOne.position);
//
//			float rotationAngle = Mathf.Acos(Vector2.Dot(previousVector,currentVector) / (previousVector.magnitude * currentVector.magnitude));
//			print(rotationAngle);
//			islandGameObject.transform.Rotate(0f,0f,rotationAngle * 10f,Space.World);
//		}
//	}
}

