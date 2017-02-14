using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Protocol;

public class CombineUIManager : MonoBehaviour {

    public static CombineUIManager Instance { get; private set; }

    [SerializeField]
    public CombineSlotBehavior[] materialSlots = new CombineSlotBehavior[3];
    [SerializeField]
    private GameObject combineResultImage;
    [SerializeField]
    private Text combineResultMessage;
    public int puttingSlotIndex;

	// Use this for initialization
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
    }

    void RegisterPlayerEvents(Player player)
    {
        player.ResponseManager.OnSynthesizeMaterialResponse += OnSynthesizeMaterialResponse;
    }

    public void ClearCombineSlots()
    {
        foreach (CombineSlotBehavior slot in materialSlots)
        {
            slot.SetSlotInfo(null);
        }
        combineResultMessage.text = "";
    }

    public void PutInItemFromInventory(Item puttingItem)
    {
        materialSlots[puttingSlotIndex].SetSlotInfo(puttingItem);
    }

    public void CombineMaterials()
    {
        List<Blueprint.ElementInfo> materialInfos = new List<Blueprint.ElementInfo>();
        for(int i = 0; i < materialSlots.Length; i++)
        {
            if(materialSlots[i].item != null)
            {
                materialInfos.Add(new Blueprint.ElementInfo
                {
                    itemID = materialSlots[i].item.ItemID,
                    itemCount = 1,
                    positionIndex = i
                });
            }
        }
        UserManager.Instance.User.Player.OperationManager.SynthesizeMaterial(materialInfos.ToArray());

        foreach (var material in materialInfos)
            print(material.positionIndex + " " + material.itemID);
    }

    public void OnSynthesizeMaterialResponse(ErrorCode returnCode, Blueprint.ElementInfo[] requirements, Blueprint.ElementInfo[] products)
    {
        if (returnCode == ErrorCode.NoError)
        {
            combineResultImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("2D/" + products[0].itemID);
            combineResultMessage.text = "合成成功";
        }
        else
        {
            // combine fail
            Debug.Log("Combine Fail");
            combineResultMessage.text = "合成失敗";
        }
    }

}
