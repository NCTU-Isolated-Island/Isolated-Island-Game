using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutItemUIPage : MonoBehaviour {

    public static PutItemUIPage Instance { get; private set; }

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
}
