using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{

	private Vector3 TargetPosition;
	private Quaternion TargetRotaion;
	private Vector3 velocity;
	public int playerID;
	public string playerName;

	void Start()
	{
		TargetPosition = transform.position;
		TargetRotaion = transform.localRotation;
	}

	void Update ()
    {
        transform.position = Vector3.SmoothDamp(transform.position, TargetPosition, ref velocity, 1f, 50);
        Quaternion quaternion = Quaternion.Slerp(transform.localRotation, Quaternion.FromToRotation(transform.worldToLocalMatrix.MultiplyVector(transform.forward), velocity), Time.deltaTime);
        transform.localRotation = Quaternion.AngleAxis(quaternion.eulerAngles.y, Vector3.up);

    }

	public void UpdateLocation(Vector3 position,float eulurAngle)
	{
		TargetPosition = position;
		TargetRotaion = Quaternion.Euler(0f,eulurAngle,0f);
	}

}
