using IsolatedIslandGame.Library;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIslandController : MonoBehaviour {

	public static PlayerIslandController Instance;

	public GameObject IslandCanvas;
	public GameObject IslandModel;

	void Awake()
	{
		if(Instance == null)
			Instance = this;
	}

	void Start()
	{
		
	}

	void OnDestroy()
	{
		
	}

	public void ShowIslandPage()
	{
		//TODO load island model and match current influence
		AuxCameraSystem.Instance.RotationSpeed = 15;
		AuxCameraSystem.Instance.Show3DObject(IslandModel,new Vector3(0,-2,3),Quaternion.Euler(30,0,0));
		//
	}

	public void DismissItemCanvas()
	{
		
	}



	public IEnumerator ContributeIsland(int itemID)
	{
		
		yield return PlayerController.Instance.GetCurrentArea();

		bool containTrigger = false;
		foreach(GameObject entry in PlayerController.Instance.InArea)
		{
			if(entry.name == "IslandTrigger")
			{
				containTrigger = true;
			}
		}

		if(!containTrigger)
			ShowWarning("必須在竹湖旁才能佔領");
			yield return null;

		UserManager.Instance.User.Player.OperationManager.SendMaterialToIsland(itemID);

		//Refresh item list?
	}

	public void ShowWarning(string text)
	{
		//TODO show some warning text
	}

	public void ShowFriendRank()
	{
		List<Island.PlayerScoreInfo> infos = new List<Island.PlayerScoreInfo>();
		foreach(Island.PlayerScoreInfo entry in Island.Instance.PlayerScoreRanking)
		{
			infos.Add(entry);
		}
		infos.Sort();

		//TODO Show Rank
	}
}
