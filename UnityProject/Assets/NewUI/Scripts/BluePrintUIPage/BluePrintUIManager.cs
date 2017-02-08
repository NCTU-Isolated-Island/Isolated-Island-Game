using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IsolatedIslandGame.Library;

public class BluePrintUIManager : MonoBehaviour {

    public static BluePrintUIManager Instance { get; private set; }

    // UI Variable
    [SerializeField]
    private GameObject bluePrintSet;
    [SerializeField]
    private GameObject bluePrintSetContent;
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
        LoadBluePrint();
    }

    public void LoadBluePrint()
    {
        foreach(var bluePrint in UserManager.Instance.User.Player.KnownBlueprints)
        {
            // Show BluePrint in UI
            GameObject tmp = Instantiate(bluePrintSet);
            tmp.transform.parent = bluePrintSetContent.transform;
        }
    }

    public void ToCombineByBluePrint(Blueprint bluePrint)
    {
        // Swap to Combine Page
        UIManager.Instance.SwapPage(UIManager.UIPageType.Combine);
        // Put ingredients base on bluePrint formula
    }

}
