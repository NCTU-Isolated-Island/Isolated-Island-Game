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
        if (UserManager.Instance.User.IsOnline)
        {
            RegisterPlayerEvents(UserManager.Instance.User.Player);
        }
        else
        {
            UserManager.Instance.User.OnPlayerOnline += RegisterPlayerEvents;
        }
        LoadBluePrint();
    }

    void RegisterPlayerEvents(Player player)
    {
        player.OnGetBlueprint += OnGetBluePrint;
        LoadBluePrint();
    }

    void OnGetBluePrint(Blueprint blueprint)
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

            Image[] material = new Image[3];
            for (int i = 0; i < 3; i++)
                material[i] = tmp.transform.FindChild("Material" + i).GetComponent<Image>();

            Image result = tmp.transform.FindChild("Result").GetComponent<Image>();

            foreach(var elementInfo in bluePrint.Requirements)
            {
                // Put Sprite to material.sprite
                material[elementInfo.positionIndex].sprite = Resources.Load<Sprite>("2D/" +  elementInfo.itemID);
            }
        }
    }

    public void ToCombineByBluePrint(Blueprint bluePrint)
    {
        // Swap to Combine Page
        UIManager.Instance.SwapPage(UIManager.UIPageType.Combine);
        // Put ingredients base on bluePrint formula
    }

}
