using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	public GameObject Target;
	private Vector3 vel;


	
	void Update () {

		transform.position = Vector3.SmoothDamp(transform.position, Target.transform.position, ref vel, 0.3f);
		transform.rotation = Quaternion.Slerp(transform.rotation, Target.transform.rotation,Time.deltaTime);

	}

//	IEnumerator Move()
//	{
//		transform.position = Vector3.SmoothDamp(transform.position, Target.transform.position, ref vel, 0.3f);
//	}

}
