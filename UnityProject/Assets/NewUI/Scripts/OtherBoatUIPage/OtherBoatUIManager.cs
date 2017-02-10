using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Protocol;

public class OtherBoatUIManager : MonoBehaviour {

    public static OtherBoatUIManager Instance { get; private set; }

    private PlayerInformation otherPlayerInfo;

    // UI Variable
    [SerializeField]
    private Text titleText;
    [SerializeField]
    private GameObject otherPlayerInformationPanel;
    //

    public void SetOtherPlayerInfo(int otherPlayerID)
    {
        if (PlayerInformationManager.Instance.FindPlayerInformation(otherPlayerID, out otherPlayerInfo))
        {
            titleText.text = otherPlayerInfo.nickname;
            otherPlayerInformationPanel.transform.Find("otherPlayerSpeech").GetComponent<Text>().text = otherPlayerInfo.signature;
            Text groupText = otherPlayerInformationPanel.transform.Find("otherPlayerGroup").GetComponent<Text>();
            switch (otherPlayerInfo.groupType)
            {
                case GroupType.Animal:
                    groupText.text = "信仰";
                    break;
                case GroupType.Businessman:
                    groupText.text = "商人";
                    break;
                case GroupType.Farmer:
                    groupText.text = "農夫";
                    break;
            }
        }
    }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {

    }
}
