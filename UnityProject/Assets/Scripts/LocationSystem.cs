using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class LocationSystem : MonoBehaviour {

	public static LocationSystem Instance;

	public Text LocationInfo;

	private System.DateTime updateTime;
	private Vector3 previousLocation;

	public GameObject TestLocation;

	private bool DebugMode = false;

	void Awake()
	{
		if(Instance == null){
			Instance = this;
		}

		#if UNITY_EDITOR
		DebugMode = true;
		#endif

	}

	void Update()
	{	
		if(DebugMode)
		{
			if(Input.GetKeyDown(KeyCode.T))
			{

				Quaternion rotation = Quaternion.LookRotation(GetInGameCoordinate() - previousLocation);

				StartCoroutine(GameManager.Instance.OnPlayerLocationChange
					(
						GetInGameCoordinate(),
						rotation.eulerAngles.y
					));


				previousLocation = GetInGameCoordinate();
				updateTime = ConvertFromUnixTimestamp(Input.location.lastData.timestamp);
			}
		}
		else
		{
			if(updateTime != ConvertFromUnixTimestamp(Input.location.lastData.timestamp))
			{

				Quaternion rotation = Quaternion.LookRotation(GetInGameCoordinate() - previousLocation);

				StartCoroutine(GameManager.Instance.OnPlayerLocationChange
					(
						GetInGameCoordinate(),
						rotation.eulerAngles.y
					));


				previousLocation = GetInGameCoordinate();
				updateTime = ConvertFromUnixTimestamp(Input.location.lastData.timestamp);
			}
		}
	}

	public void StartLocationService()
	{
		StartCoroutine(TryStartLocationService());
	}

	public void StopLocationService()
	{
		Input.location.Stop();
	}

	private void UpdateLocationInfo()
	{
		LocationInfo.text =
			"Altitude: " + Input.location.lastData.altitude +
			" Vertical Accuracy: " + Input.location.lastData.verticalAccuracy +
			"\nLatitude: " + Input.location.lastData.latitude +
			" Longitude: " + Input.location.lastData.longitude +
			" HorizontalAccuracy: " + Input.location.lastData.horizontalAccuracy + 
			"\nTimeStamp: " + ConvertFromUnixTimestamp(Input.location.lastData.timestamp).ToLongTimeString();
		;
	}

	//Longitude 經度 1 = 110KM
	//Latitude 緯度 1 = 100KM

	public Vector3 GetInGameCoordinate()
	{
		//FOR DEVELOP
		if(Input.location.status != LocationServiceStatus.Running || DebugMode)
		{
			return new Vector3(TestLocation.transform.position.x,0f,TestLocation.transform.position.z);
		}

		Vector3 result;
		result.x = (Input.location.lastData.longitude - 120.997325f) * 110000;
		result.y = 0f;
		result.z = (Input.location.lastData.latitude - 24.787493f) * 100000;

		return result;
	}

	private IEnumerator TryStartLocationService()
	{	
		// First, check if user has location service enabled
		if (!Input.location.isEnabledByUser)
		{
			print("Location Service not enabled");
			yield break;
		}

		// Start service before querying location
		Input.location.Start();

		// Wait until service initializes
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
		{	
			yield return new WaitForSeconds(1);
			maxWait--;
		}

		// Service didn't initialize in 20 seconds
		if (maxWait < 1)
		{
			print("Timed out");
			yield break;
		}

		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed)
		{
			print("Unable to determine device location");
			yield break;
		}

	}

	public static System.DateTime ConvertFromUnixTimestamp(double timestamp)
	{
		System.DateTime origin = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
		return origin.AddSeconds(timestamp).AddHours(8);
	}
}
