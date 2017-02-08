using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogInUIManager : MonoBehaviour {

    public static LogInUIManager Instance { get; private set; }

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
