using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IsolatedIslandGame.Library;

public class FriendSetBehavior : MonoBehaviour {

    public static FriendSetBehavior Instance { get; private set; }

    [SerializeField]
    private GameObject friendDetailPanel;

    private FriendInformation information;

    void Awake()
    {
        if (Instance != null)
            Instance = this;
    }

    void LoadFriendDetail()
    {
        
    }

}
