using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionSystem : MonoBehaviour {

	public static MissionSystem Instance;


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
