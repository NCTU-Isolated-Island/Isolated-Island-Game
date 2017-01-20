using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	public GameObject Target;
	private Vector3 vel;


	
	void Update () {

		if(Input.touchCount == 1)
		{
			Touch touch = Input.GetTouch(0);
			Target.transform.Rotate
			(
				touch.deltaPosition.y * 0.1f,
				touch.deltaPosition.x * -0.1f,
				0,
				Space.World
			);

		}
	}

//	IEnumerator Move()
//	{
//		transform.position = Vector3.SmoothDamp(transform.position, Target.transform.position, ref vel, 0.3f);
//	}

}
