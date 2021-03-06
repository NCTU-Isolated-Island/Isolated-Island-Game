﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Protocol;

public class OtherBoatUIManager : MonoBehaviour
{
    public static OtherBoatUIManager Instance { get; private set; }

    private PlayerInformation otherPlayerInfo;

    // UI Variable
    [SerializeField]
    private Text titleText;
    [SerializeField]
    private GameObject otherPlayerInformationPanel;

    [SerializeField]
    private GameObject expandButton;
    [SerializeField]
    private GameObject interactionButtonsContent;

    [SerializeField]
    private Button addFriendButton;
    [SerializeField]
    private Button startChatButton;
    [SerializeField]
    private Button startTransactionButton;
    [SerializeField]
    private Button donateButton;

    [SerializeField]
    private GameObject chooseAmountPanel;
    [SerializeField]
    private Dropdown chooseAmountDropdown;

    //
    private bool expandStatus;

    private float ver_ori;
    private IEnumerator coroutine;

    class DonationInfo
    {
        public Item item;
        public int amount;
    }
    private DonationInfo donationInfo = new DonationInfo();

    //
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        gameObject.SetActive(false);
    }

    void Start()
    {
        Vector3 verTmp = interactionButtonsContent.GetComponent<RectTransform>().offsetMax;
        ver_ori = verTmp.y;
    }

    void OnDisable()
    {
        if (CameraManager.Instance != null)
            CameraManager.Instance.ToNearAnchor(GameManager.Instance.PlayerGameObject);
        chooseAmountPanel.SetActive(false);
    }

    void InitialSetting()
    {
        expandStatus = false;
    }

    public void SetOtherPlayerInfo(int otherPlayerID)
    {
        print("SetOtherPlayerInfo");
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
                    groupText.text = "科技";
                    break;
                case GroupType.Farmer:
                    groupText.text = "自然";
                    break;
            }

            // Control addFriend Button
            if (UserManager.Instance.User.Player.ContainsFriend(otherPlayerID) == true)
                addFriendButton.interactable = false;
            else
                addFriendButton.interactable = true;

        }
    }

    public void OpenInventoryPanelForDonation()
    {
        UIManager.Instance.SwapPage(UIManager.UIPageType.Inventory);
    }

    public void ConfirmDonationItem(Item item)
    {
        donationInfo.item = item;
        chooseAmountPanel.SetActive(true);
    }

    public void ConfirmDonationAmount()
    {
        donationInfo.amount = chooseAmountDropdown.value + 1;
        DonateToOtherPlayer();
    }

    public void DonateToOtherPlayer()
    {
        UserManager.Instance.User.Player.OperationManager.DonateItemToPlayer(otherPlayerInfo.playerID, donationInfo.item.ItemID, donationInfo.amount);

        chooseAmountPanel.SetActive(false);
    }

    public void SendTransactionRequest()
    {
        TransactionManager.Instance.SendTransactionRequest(otherPlayerInfo.playerID);
    }

    public void SendFriendRequest()
    {
        UserManager.Instance.User.Player.OperationManager.InviteFriend(otherPlayerInfo.playerID);
    }

    public void StartChatting()
    {
        UIManager.Instance.SwapPage(UIManager.UIPageType.Chat_Message);
        ChatUIManager.Instance.LoadMessagePage(otherPlayerInfo);
    }

    public void OnClickExpandButton()
    {
        if (expandStatus == false)
            ExpandInteractionButtons();
        else
            WithdrawInteractionButtons();
    }

    public void ExpandInteractionButtons()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = ExpandBtnCoroutine(true);
        StartCoroutine(coroutine);
    }

    public void WithdrawInteractionButtons()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = ExpandBtnCoroutine(false);
        StartCoroutine(coroutine);
    }

    IEnumerator ExpandBtnCoroutine(bool isOn)
    {
        float passTime = 0f;
        Vector3 verTmp = interactionButtonsContent.GetComponent<RectTransform>().offsetMax;

        float ver_st = verTmp.y;
        float intervalTime = 0.5f;

        while (passTime < intervalTime)
        {
            passTime += Time.deltaTime;

            float lerpAlpha = 0.1f * passTime / intervalTime + 0.9f * Mathf.Sqrt(1 - Mathf.Pow(passTime / intervalTime - 1, 2));
            if (isOn)
            {
                verTmp.y = Mathf.Lerp(ver_st, 0, lerpAlpha);
                interactionButtonsContent.GetComponent<RectTransform>().offsetMax = verTmp;
            }
            else
            {
                verTmp.y = Mathf.Lerp(ver_st, ver_ori, lerpAlpha);
                interactionButtonsContent.GetComponent<RectTransform>().offsetMax = verTmp;
            }

            yield return null;
        }
        expandStatus = isOn;
    }

}
