using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {


	public GameObject CurrentSelectDecoration;

	void Update()
	{
		if(Input.touchCount == 1)
		{
			//do below
		}

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

		//目前這部分只有拖拉功能 (先只做這些)
		if(Input.touches[0].phase == TouchPhase.Began)
		{
			
			if(entireHit && entireHitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Decoration"))
			{
				CurrentSelectDecoration = entireHitInfo.transform.gameObject;
			}

		}

		if(Input.touches[0].phase == TouchPhase.Stationary || Input.touches[0].phase == TouchPhase.Moved)
		{
	
		}

		if(Input.touches[0].phase == TouchPhase.Ended)
		{
			if(!vesselHit)
			{
//				if(!RemovedDecorations.Contains(CurrentSelectDecoration))
//					RemovedDecorations.Add(CurrentSelectDecoration);
				
				CurrentSelectDecoration.SetActive(false);

				CurrentSelectDecoration = null;
			}

		}

		if(CurrentSelectDecoration != null)
		{
			if(vesselHit)
			{
				CurrentSelectDecoration.transform.position = vesselHitInfo.point;
			}
			else
			{
				CurrentSelectDecoration.transform.position = 
					Camera.main.transform.position + Camera.main.ScreenPointToRay(Input.mousePosition).direction * 5;
			}
		}


	}

	void BeginDec()
	{
		// for dev
		int itemID = 1;

		CurrentSelectDecoration = Instantiate(GameManager.Instance.elementModels[itemID],Vector3.zero,Quaternion.identity) as GameObject;

//		if(!AddedDecorations.Contains(CurrentSelectDecoration))
//			AddedDecorations.Add(CurrentSelectDecoration);
	}


}
