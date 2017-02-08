using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;

public class QRCodeSystem : MonoBehaviour {

	public static QRCodeSystem Instance;
	public RawImage CameraView;
	public GameObject CameraCanvas;

	private WebCamTexture qrCamera;
	private Result scanResult;
	private bool stopReading;
	private BarcodeReader reader = new BarcodeReader();


	void Awake()
	{
		if(Instance == null)
		{
			Instance = this;
		}
		else if(Instance != this)
		{
			Destroy(gameObject);
		}
		//StartReading();
	}

	void Update()
	{
		

	}

	public void StartReading()
	{
		StartCoroutine(ReadQrCode());
	}

	public void StopReading()
	{
		StopCoroutine(Scan());
		stopReading = true;
		qrCamera.Stop();
		CameraCanvas.SetActive(false);
	}

	IEnumerator ReadQrCode()
	{
		scanResult = null;
		stopReading = false;

		yield return TurnOnCamera();

		CameraCanvas.SetActive(true);

		if(scanResult == null && !stopReading)
		{
			if(qrCamera != null && qrCamera.isPlaying)
			{
				yield return Scan();

			}

		}

	}

	private IEnumerator TurnOnCamera()
	{
		yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);    //授權開啟鏡頭
		if (Application.HasUserAuthorization(UserAuthorization.WebCam))
		{
			//設置攝影機要攝影的區域
			qrCamera = new WebCamTexture(WebCamTexture.devices[0].name, Screen.width, Screen.height, 30);/* (攝影機名稱, 攝影機要拍到的寬度, 攝影機要拍到的高度, 攝影機的FPS) */
			CameraView.texture= qrCamera;
			qrCamera.Play();//開啟攝影機
		}
	}

	private IEnumerator Scan()
	{
		do {
			Texture2D t2D = new Texture2D(Screen.width, Screen.height);//掃描後的影像儲存大小，越大會造成效能消耗越大，若影像嚴重延遲，請降低儲存大小。
			yield return new WaitForEndOfFrame();//等待攝影機的影像繪製完畢

			t2D.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);//掃描的範圍，設定為整個攝影機拍到的影像，若影像嚴重延遲，請降低掃描大小。
			t2D.Apply();//開始掃描

			scanResult = reader.Decode(t2D.GetPixels32(), t2D.width, t2D.height);//對剛剛掃描到的影像進行解碼，並將解碼後的資料回傳
			Destroy (t2D);

			yield return new WaitForSeconds(.5f);

		} while (scanResult == null && !stopReading);

		if(scanResult != null)
		{
			print(scanResult.Text); //GOT QRCODE
			//call scan success

			qrCamera.Stop();
		}

	}


}
