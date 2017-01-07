using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

    public static CameraManager Instance { get; private set; }

	private float MoveCameraDelay = 1.5f;

    private GameObject Camera;
    private bool using_cor;

	void Awake()
	{
		Camera = GameObject.FindGameObjectWithTag("MainCamera");
		Instance = this;
        using_cor = false;
    }
    
    void Update()
    {
//        if (Input.GetKeyDown(KeyCode.A))
//            ToNearAnchor(GameManager.Instance.PlayerGameObject);
//        if (Input.GetKeyDown(KeyCode.S))
//            ToFarAnchor(GameManager.Instance.PlayerGameObject);
//        if (Input.GetKeyDown(KeyCode.W))
//            Zoom(2);

    }
    
    IEnumerator MoveObject(Transform source, Transform target, float overTime)
    {
        float startTime = Time.time;
        Vector3 start_pos = source.position;
        while (Time.time < startTime + overTime)
        {
            source.position = Vector3.Lerp(start_pos, target.position, (Time.time - startTime) / overTime);
			source.rotation = Quaternion.Slerp(source.rotation,target.rotation,(Time.time - startTime) / overTime);
			//source.LookAt(target.parent); // look at the vessel , not the anchor
            yield return null;
        }
        source.position = target.position;
        using_cor = false;
    }

    public void ToNearAnchor (GameObject user)
    {
        if (using_cor) return;

        Camera.transform.parent = user.transform.Find("CameraAnchor");
        using_cor = true;
        StartCoroutine(MoveObject(Camera.transform, user.transform.Find("NearAnchor"), MoveCameraDelay));
    }

    public void ToFarAnchor (GameObject user)
    {
        if (using_cor) return;

        Camera.transform.parent = user.transform.Find("CameraAnchor");
        using_cor = true;
        StartCoroutine(MoveObject(Camera.transform, user.transform.Find("FarAnchor"), MoveCameraDelay));
    }

    public void Zoom (float amount)
    {

		if(Vector3.Distance(Camera.transform.position,Camera.transform.parent.position) < 5f && amount < 1)
			return;
		if(Vector3.Distance(Camera.transform.position,Camera.transform.parent.position) > 50f && amount > 1)
			return;

		this.transform.position = Vector3.LerpUnclamped(Camera.transform.parent.position,this.transform.position,amount);

//        Vector3 delta = Camera.transform.position - Camera.transform.parent.position;// CameraAnchor position
//
//        if (amount > 0)
//            delta /= amount;
//        else
//			delta *= amount;
//
//        Camera.transform.position = Camera.transform.parent.position + delta;
    }

	public void CameraRotate (float amount)
    {
		Camera.transform.parent.Rotate( new Vector3(0, amount , 0));
    }
}
