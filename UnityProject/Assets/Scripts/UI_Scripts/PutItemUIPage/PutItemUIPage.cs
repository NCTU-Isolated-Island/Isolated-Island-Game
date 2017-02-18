using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PutItemUIPage : MonoBehaviour
{

    public static PutItemUIPage Instance { get; private set; }

    private Button DoneButton;
    private Button RotateButton;

    // UI Variable

    //

    void InitSetting()
    {

    }

    void Update()
    {
        if (PlayerDecorationManager.Instance.CurrentControlMode == PlayerDecorationManager.ControlMode.Rotate)
        {
            RotateButton.interactable = true;
        }
        else
        {
            RotateButton.interactable = false;
        }
    }


    void Awake()
    {
        if (Instance == null)
            Instance = this;

        DoneButton = transform.Find("DoneButton").GetComponent<Button>();
        RotateButton = transform.Find("RotateButton").GetComponent<Button>();

        DoneButton.onClick.AddListener(delegate
        {
            PlayerDecorationManager.Instance.UpdateModifiedDecorationsToServer();
        });

        RotateButton.onClick.AddListener(delegate
        {
            PlayerDecorationManager.Instance.ChangeModelOrientation();
        });


        gameObject.SetActive(false);
    }
}
