using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	public GameObject Target;
	private Vector3 vel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		transform.position = Vector3.SmoothDamp(transform.position, Target.transform.position, ref vel, 0.3f);
		transform.rotation = Quaternion.Slerp(transform.rotation, Target.transform.rotation,0.3f);
		if(Input.GetKeyDown(KeyCode.M))
		{
			
		}
	}

//	IEnumerator Move()
//	{
//		transform.position = Vector3.SmoothDamp(transform.position, Target.transform.position, ref vel, 0.3f);
//	}

}
