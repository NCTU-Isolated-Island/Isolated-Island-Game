using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	Vector3 initialPos;

	void Start()
	{
		initialPos = transform.position;
	}

	void Update()
	{
		transform.position = initialPos + Vector3.up * Mathf.Sin(Time.time * 1.5f) * 0.3f;
		transform.Rotate(0f,20f * Time.deltaTime, 0f);
	}

}
