using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PutItemUIPage : MonoBehaviour {

    public static PutItemUIPage Instance { get; private set; }

	private Button DoneButton;
	private Button RotateButton;

    // UI Variable

    //

    void InitSetting()
    {

    }

    void Awake()
	{
        if (Instance == null)
            Instance = this;

		DoneButton = transform.Find("DoneButton").GetComponent<Button>();
		RotateButton = transform.Find("RotateButton").GetComponent<Button>();


		DoneButton.onClick.AddListener(delegate {
			PlayerController.Instance.UpdateModifiedDecorationsToServer();
		});

		RotateButton.onClick.AddListener(delegate {
			print("SUCCESS");
			PlayerController.Instance.ChangeModelOrientation();
		});

		gameObject.SetActive(false);
    }

    void Start()
    {

    }
}
