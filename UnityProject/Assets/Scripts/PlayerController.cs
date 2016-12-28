using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using IsolatedIslandGame.Client;
using IsolatedIslandGame.Client.Communication;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;

public class PlayerController : MonoBehaviour {

	//FOR DEVELOP
	public GameObject instantiatedMaterial;
	public GameObject playerGameObject;

	//For Pinch Capability
	public float perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
	public float orthoZoomSpeed = 0.5f;        // The rate of change of the orthographic size in orthographic mode.

	private Vector3 originalMousePosition;

	private bool finishPlacing = false; // 完成放置素材

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.D))
		{
			UserManager.Instance.User.Player.OperationManager.DrawMaterial();
		}

		if(Input.GetMouseButtonDown(0))
		{
			CheckIfSelectVessel();
		}

	}

	void Awake()
	{
		UserManager.Instance.User.Player.OnDrawMaterial += OnDraw;
	}

	void OnDraw(Item item, int count)
	{
		print(item.ItemName + " : " + count);
	}


	public void StartPlaceDecoration()
	{
		finishPlacing = false;
		StartCoroutine(PlaceMaterial(1));

	}

	public void FinishPlaceDecoration()
	{
		finishPlacing = true;
	}
		
	IEnumerator PlaceMaterial(int materialID)
	{
		Vector3 position;
		Quaternion rotation;

		GameObject temp = Instantiate(instantiatedMaterial,Vector3.zero,Quaternion.identity) as GameObject;
		RaycastHit hitInfo = new RaycastHit();
		while(!finishPlacing)
		{
			//TODO 必須把Player模型設定成 "PlayerModel" Layer
			bool hit = Physics.Raycast(
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
		print("done");
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
		if(hit)
		{
			
		}

	}

	void AdjustViewAngle()
	{
//		if(Input.touchCount == 1)
//		{
//			Touch touch = Input.GetTouch(0);
//			Vector2 touchVector = touch.deltaPosition * 0.05f;
//
//			islandGameObject.transform.Rotate(touchVector.y,touchVector.x * (-1f),0f,Space.World);
//		}
		Vector3 deltaPosition = Input.mousePosition - originalMousePosition;
		deltaPosition *= 2.2f;
		//Quaternion rotation = Quaternion.Euler(deltaPosition.y * (-1f),deltaPosition.x * (-1f),0);




		//islandGameObject.transform.rotation = rotation;
		originalMousePosition = Input.mousePosition;
	}
//
//	void PinchToZoom()
//	{
//		if (Input.touchCount == 2)
//		{
//			// Store both touches.
//			Touch touchZero = Input.GetTouch(0);
//			Touch touchOne = Input.GetTouch(1);
//
//			// Find the position in the previous frame of each touch.
//			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
//			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
//
//			// Find the magnitude of the vector (the distance) between the touches in each frame.
//			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
//			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
//
//			// Find the difference in the distances between each frame.
//			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
//
//			// If the camera is orthographic...
//			if (Camera.main.orthographic)
//			{
//				// ... change the orthographic size based on the change in distance between the touches.
//				Camera.main.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;
//
//				// Make sure the orthographic size never drops below zero.
//				Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize, 0.1f);
//			}
//			else
//			{
//				print("1 " + deltaMagnitudeDiff);
//				// Otherwise change the field of view based on the change in distance between the touches.
//				print("2 " + deltaMagnitudeDiff * perspectiveZoomSpeed);
//				Camera.main.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
//
//				// Clamp the field of view to make sure it's between 0 and 180.
//				Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 10f, 85f);
//			}
//		}
//	}
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

