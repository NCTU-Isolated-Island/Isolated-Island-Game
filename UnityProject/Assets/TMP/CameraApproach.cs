using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraApproach : MonoBehaviour {
    [SerializeField]
    private float distance;
    [SerializeField]
    private float theta;

    private GameObject camera;

    // TESTING
    [SerializeField]
    private GameObject target;

	// Use this for initialization
    void InitSetting()
    {
        distance = 20f;
        theta = 0f;
    }

	void Start () {
        InitSetting();
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        ApproachObject(target);
	}

    public void ApproachObject(GameObject target)
    {
        Vector3 oriPos = target.transform.position;
        oriPos.x += distance * Mathf.Cos(theta);
        oriPos.z += distance * Mathf.Sin(theta);
        oriPos.y += 2f;

        camera.transform.position = oriPos;
        camera.transform.LookAt(target.transform);

        StartCoroutine(Approach(target));
    }

    IEnumerator Approach(GameObject target)
    {
        float passTime = 0f;

        while(true)
        {
            Vector3 oriPos = target.transform.position;
            oriPos.x += distance * Mathf.Cos(theta);
            oriPos.z += distance * Mathf.Sin(theta);
            oriPos.y += 2f;
            camera.transform.position = oriPos;
            camera.transform.LookAt(target.transform);

            if (distance > 5f)
                distance = Mathf.Lerp(20, 5, passTime / 1f);

            theta += Time.deltaTime * 2;

            passTime += Time.deltaTime;
            yield return null;
        }
    }
	
}
