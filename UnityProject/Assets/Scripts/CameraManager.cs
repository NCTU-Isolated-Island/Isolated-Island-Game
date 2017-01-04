using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

    public static CameraManager Instance { get; private set; }

    private GameObject Camera;

	void Awake()
	{
		Camera = GameObject.FindGameObjectWithTag("MainCamera");
		Instance = this;
//		if(Instance == null){
//			Instance = this;
//		}else if(Instance != this){
//			Destroy(gameObject);
//		}
	}


    void Start()
    {
        
    }

    public void ToNearAnchor (GameObject user)
    {
        Camera.transform.parent = user.transform.Find("CameraAnchor");
        Camera.transform.position = user.transform.Find("NearAnchor").position;
        Camera.transform.LookAt(user.transform);
    }

    public void ToFarAnchor (GameObject user)
    {
        Camera.transform.parent = user.transform.Find("CameraAnchor");
        Camera.transform.position = user.transform.Find("FarAnchor").position;
        Camera.transform.LookAt(user.transform);
    }

    public void Zoom (float amount)
    {
        Vector3 delta = Camera.transform.position - Camera.transform.parent.position;// CameraAnchor position

        if (amount > 0)
            delta /= amount;
        else
            delta *= amount;
    }

    public void CameraRotate (float angle)
    {
        Camera.transform.parent.Rotate( new Vector3(0, angle ,0));
    }
}
