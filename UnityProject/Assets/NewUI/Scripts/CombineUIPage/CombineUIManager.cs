using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsolatedIslandGame.Library;

public class CombineUIManager : MonoBehaviour {

    public static CombineUIManager Instance { get; private set; }

    [SerializeField]
    private CombineSlotBehavior[] materialSlots = new CombineSlotBehavior[3];
    public int puttingSlotIndex;

	// Use this for initialization
    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

	void OnEnable () {

    }

    public void ClearCombineSlots()
    {
        foreach (CombineSlotBehavior slot in materialSlots)
        {
            slot.SetSlotInfo(null);
        }
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
    }

}
