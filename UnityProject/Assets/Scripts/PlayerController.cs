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


	public GameObject CurrentSelectDecoration;

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

		if(Input.GetMouseButtonDown(0))
		{
			SelectDecoration();
		}

		if(Input.GetKeyDown(KeyCode.T))
		{
			RemoveAllDecoration();
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
			yield return null;
		}


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

/*	IEnumerator RotateDecoration()
	{
		CurrentSelectDecoration.transform.Rotate(0,0,0,Space.World);
	}*/

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

	public void RemoveDecoration(GameObject target)
	{
		UserManager.Instance.User.Player.OperationManager.RemoveDecorationFromVessel(System.Int32.Parse(target.name));
	}


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

			print(deltaMagnitudeDiff);
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

	public enum Mode{ Move,Rotate,Default}
	public Mode CurrentMode;

//	IEnumerator Dec()
//	{
//		while(true)
//		{
//			if(CursorMode == Mode.Move)
//			{
//				// GO.tran.po = hit point
//			}
//
//			if(CursorMode == Mode.Rotate)
//			{
//				if(Input.touchCount == 1)
//				{
//					Touch touch = Input.GetTouch(0);
//					CurrentSelectDecoration.transform.Rotate
//					(
//						touch.deltaPosition.y,
//						touch.deltaPosition.x,
//						0
//					);
//
//
//				}
//			}
//
//			yield return null;
//		}
//	}
}

