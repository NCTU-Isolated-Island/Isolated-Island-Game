using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	LineRenderer lin;
	public GameObject cube;
	public GameObject cube2;
	// Use this for initialization
	void Start () {
		lin = GetComponent<LineRenderer>();
		lin.SetVertexCount(2);
		lin.SetPosition(0,Vector3.zero);
		lin.SetPosition(1,Quaternion.Euler(0f,1f,0f) * Vector3.forward);
	}
	
	// Update is called once per frame
	void Update () {
		lin.SetPosition(1,Quaternion.Euler(0f,Time.time,0f) * Vector3.forward * 10f);

		cube.transform.rotation = Quaternion.Euler(0f,Vector3.Angle(new Vector3(-1f,0f,-1f),Vector3.forward),0f);
		cube2.transform.rotation = Quaternion.LookRotation(new Vector3(-1f,0f,-1f));

		GameObject a = new GameObject();

		a.transform.rotation = Quaternion.LookRotation(new Vector3(-1f,0f,-1f));

		print(a.transform.eulerAngles.y); 
	}
}
