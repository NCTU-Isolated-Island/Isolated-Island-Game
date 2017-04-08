using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PutItemUIPage : MonoBehaviour
{
    public static PutItemUIPage Instance { get; private set; }

    private Button DoneButton;
    private Button RotateButton;
    private Button RemoveAllDecorationButton;

    [SerializeField]
    private GameObject confirmItemPanel;

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
        RemoveAllDecorationButton = transform.Find("RemoveAllDecorationButton").GetComponent<Button>();

        DoneButton.onClick.AddListener(delegate
        {
            PlayerDecorationManager.Instance.UpdateModifiedDecorationsToServer();
            UIManager.Instance.ToPreviousPage();
        });

        RotateButton.onClick.AddListener(delegate
        {
            PlayerDecorationManager.Instance.ChangeModelOrientation();
        });

        RemoveAllDecorationButton.onClick.AddListener(delegate
        {
            RemoveAllDecoration();
        });

        gameObject.SetActive(false);
    }

    private void RemoveAllDecoration()
    {
        GameObject confrimPanel = Instantiate(confirmItemPanel);

        confrimPanel.transform.SetParent(gameObject.transform);
        confrimPanel.transform.Find("Message").gameObject.GetComponent<Text>().text
            = string.Format("確定要將所有裝飾從船上撤下嗎?");
        confrimPanel.transform.Find("Cancel").gameObject.GetComponent<Button>().onClick.AddListener(delegate
        {
            Destroy(confrimPanel);
        });
        confrimPanel.transform.Find("Confirm").gameObject.GetComponent<Button>().onClick.AddListener(delegate
        {
            PlayerController.Instance.RemoveAllDecoration();
            Destroy(confrimPanel);
        });

        confrimPanel.transform.localScale = new Vector3(0.5213f, 0.5213f, 0.5213f);
        confrimPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -30, 0);
    }
}