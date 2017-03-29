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
    private GameObject receiveMessageBubble;
    [SerializeField]
    private GameObject sendMessageBubble;

    private void RenderWorldChannelBubble()
    {
        

    }

    public void ToWorldChannelChatPage()
    {
        RenderWorldChannelBubble();
    }

}