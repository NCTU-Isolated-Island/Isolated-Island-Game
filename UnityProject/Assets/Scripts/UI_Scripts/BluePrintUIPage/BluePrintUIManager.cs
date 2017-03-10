using IsolatedIslandGame.Library;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BluePrintUIManager : MonoBehaviour {

    public static BluePrintUIManager Instance { get; private set; }

    // UI Variable
    [SerializeField]
    private GameObject bluePrintSet;
    [SerializeField]
    private GameObject bluePrintSetContent;

    private Button blueprintButton;
    //

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        gameObject.SetActive(false);
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
        foreach (Transform bluePrint in bluePrintSetContent.transform)
        {
            Destroy(bluePrint.gameObject);
        }

        foreach (Blueprint bluePrint in UserManager.Instance.User.Player.KnownBlueprints.OrderBy(blueprint => blueprint.BlueprintID))
        {
            // Show BluePrint in UI
            GameObject tmp = Instantiate(bluePrintSet);
            tmp.transform.SetParent(bluePrintSetContent.transform);
            tmp.GetComponent<RectTransform>().localScale = Vector2.one;

            Image[] material = new Image[3];
            for (int i = 0; i < 3; i++)
                material[i] = tmp.transform.FindChild("Material" + i).GetComponent<Image>();

            Image result = tmp.transform.FindChild("Result").GetComponent<Image>();

            foreach (var elementInfo in bluePrint.Requirements)
            {
                // Put Sprite to material.sprite
                material[elementInfo.positionIndex].sprite = Resources.Load<Sprite>("2D/" + elementInfo.itemID);
            }

            foreach (var elementInfo in bluePrint.Products)
            {
                // Put Sprite to result.sprite , currently only one product
                result.sprite = Resources.Load<Sprite>("2D/" + elementInfo.itemID);
            }

            foreach (Image element in material)
            {
                if (element.sprite == null)
                    Destroy(element.gameObject);
            }

            blueprintButton = tmp.GetComponent<Button>();
            blueprintButton.onClick.AddListener(delegate
            {
                ToCombineByBluePrint(bluePrint);
            });
        }
    }

    public void ToCombineByBluePrint(Blueprint bluePrint)
    {
        if (!HaveEnoughElement(bluePrint))
        {
            UserManager.Instance.User.UserInform("合成通知","素材不足");
            return;
        }

        // Swap to Combine Page
        UIManager.Instance.SwapPage(UIManager.UIPageType.Combine);

        foreach (var elementInfo in bluePrint.Requirements)
        {
            // Put Sprite to material.sprite
            Item item;
            ItemManager.Instance.FindItem(elementInfo.itemID, out item);
            CombineUIManager.Instance.materialSlots[elementInfo.positionIndex].SetSlotInfo(item);
        }
        // Put ingredients base on bluePrint formula
    }

    private bool HaveEnoughElement(Blueprint blueprint)
    {
        bool result = true;
        foreach (Blueprint.ElementInfo elementInfo in blueprint.Requirements)
        {
            if (elementInfo.itemCount > UserManager.Instance.User.Player.Inventory.ItemCount(elementInfo.itemID))
                result = false;
        }
        return result;
    }

}
