using UnityEngine;
using System.Collections;

public class Architecture : MonoBehaviour {

	void Awake()
	{
		


		// Player Login 
		// if (first login) 
		// Register



	}

	void Register()
	{
		//Register

		//Login
	}


	void OnLogin()
	{
		// switch to MainScene
	}

	void OnSwitchMainScene(){
		

		//InstantiateUserGameObject


		//設定完之後再把顯示出遊戲畫面（在這之前可能顯示loading bar
	}

	//5秒更新一次的Update
	void SlowUpdate()
	{
		//Send my locationInfo;
	}

	void OnUserLocationChange()
	{
		//改變對應的User位置
	}

	void OnUserPlaceElement()
	{
		//顯示該Element
	}

}

public class PlayerController : MonoBehaviour {

	void Update()
	{
		//if 連點兩下User or Player 
		// Zoom in

		//if 點選場上的Furnace
		//(Enable and)Slide in Furnace UICanvas
	}

	void ChangeViewingAngle()
	{
		
	}

	void PlaceElement()
	{
		
	}

	//融合場景的Code
}

//Attach under MainCamera
public class CameraController : MonoBehaviour {

	//放大（查看）Player or User 的船
	void ZoomIn(){
		
	}

	//回到鳥瞰視角
	void ZoomOut(){
		
	}


}
