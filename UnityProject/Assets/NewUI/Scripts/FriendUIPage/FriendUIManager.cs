using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendUIManager : MonoBehaviour {

    public static FriendUIManager Instance { get; private set; }

    // UI Variable

    //

    void InitSetting()
    {

    }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {

    }

	void LoadFriendList()
	{
		
	}
}
