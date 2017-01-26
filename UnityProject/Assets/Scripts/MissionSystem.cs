using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionSystem : MonoBehaviour {

	public static MissionSystem Instance;
//	public List<Mission> Missions;

	void Awake()
	{
		if(Instance == null)
		{
			Instance = this;
		}
		else if(Instance != this)
		{
			Destroy(gameObject);
		}

	}

	void OnGetNewMission()
	{
		//store data to Missions
	}

	//開始執行任務 QRCODE
	public void GetMissionOne()
	{
		
		//qrcode~~~
	}

	// 指定地點移動
	public void GetMissionTwo()
	{
		//find where i am 
		//decide go where
		//add me as location trigger ' listener

	}



}

//class Mission
//{
//	
//}
