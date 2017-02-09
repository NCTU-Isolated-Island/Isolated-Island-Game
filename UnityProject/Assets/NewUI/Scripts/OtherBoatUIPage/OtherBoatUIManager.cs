using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherBoatUIManager : MonoBehaviour {

    public static OtherBoatUIManager Instance { get; private set; }

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
