using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.TextData;
using IsolatedIslandGame.Protocol;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WorldChannelChatManager : MonoBehaviour
{
    public static WorldChannelChatManager Instance { get; private set; }

    [SerializeField]
    private GameObject worldChannelChatPage;
    [SerializeField]
    private GameObject receiveMessageBubble;
    [SerializeField]
    private GameObject sendMessageBubble;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void OpenWorldChannelChatPage()
    {
        GameObject page = Instantiate(worldChannelChatPage);
        AdjustPageStatus(page);
    }

    private void AdjustPageStatus(GameObject page)
    {
        page.transform.SetParent(transform);
        page.GetComponent<RectTransform>().localScale = Vector3.one;
        page.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        page.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
    }

    private void RenderWorldChannelBubble()
    {
        

    }



}