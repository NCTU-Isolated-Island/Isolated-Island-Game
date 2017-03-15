using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using IsolatedIslandGame.Library;

public class Test : MonoBehaviour {






	void Update()
	{
		bool vesselHit;
		RaycastHit vesselHitInfo = new RaycastHit();
		vesselHit = Physics.Raycast
			(
				Camera.main.ScreenPointToRay(Input.mousePosition),
				out vesselHitInfo,
				99999f,
				LayerMask.GetMask("ShipModel")
			);

		if(vesselHit != null)
		print(vesselHit);
	}



}
