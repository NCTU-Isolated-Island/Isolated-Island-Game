using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {




	public enum ViewMode
	{
		FirstPerson, BirdView, NormalView
	}

	public ViewMode CurrenViewMode = ViewMode.NormalView;

	public void ChangeViewMode(ViewMode mode)
	{
		
		switch (mode) {
		case ViewMode.FirstPerson:
			// move camera to first person anchor

			if(SystemInfo.supportsGyroscope)
			{
				Input.gyro.enabled = true;
			}
			else
			{
				//TODO Show No Gyro Support Warning
			}

			CurrenViewMode = ViewMode.FirstPerson;
			break;
		
		case ViewMode.NormalView:
			//
			break;
		
		case ViewMode.BirdView:

			break;
		default:
			break;
		}
		
	}

	void Update()
	{
		if(CurrenViewMode == ViewMode.FirstPerson)
		{
			//Camera.main.transform.rotation = Input.gyro.attitude;

			//TODO maybe add horizontal Y Rotate capability
		}

		if(Input.anyKeyDown)
		{
			bool a = Handheld.PlayFullScreenMovie ("IMG_7512.mov", Color.white, FullScreenMovieControlMode.Hidden);
			print(a);
		}
	}




}
