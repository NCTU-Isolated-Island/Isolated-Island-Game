using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using IsolatedIslandGame.Library;

using IsolatedIslandGame.Protocol;
public class Bp_Button : MonoBehaviour {
    public Button yourButton;
    public GameObject Bp_list;
    public GameObject canvas;
    // Use this for initialization
    void Start()
    {
        
        // this.GetComponent<RectTransform>().localPosition = new Vector3(0, 90, 0);
        yourButton = this.gameObject.transform.GetChild(1).GetComponent<Button>();
        Bp_list.GetComponent<RectTransform>().localPosition = new Vector3(canvas.GetComponent<RectTransform>().rect.width / 2 + Bp_list.GetComponent<RectTransform>().rect.width / 2, 20, 0);
        yourButton.onClick.AddListener(move);
    }

    // Update is called once per frame
    void move () {
        float A_pos = canvas.GetComponent<RectTransform>().rect.width / 2 - Bp_list.GetComponent<RectTransform>().rect.width / 2;
        float B_pos = canvas.GetComponent<RectTransform>().rect.width / 2 + Bp_list.GetComponent<RectTransform>().rect.width / 2;
        if (Mathf.Abs(Bp_list.GetComponent<RectTransform>().localPosition.x - A_pos) <1)
        { Bp_list.transform.localPosition = new Vector3(B_pos, 20, 0);  }
        else if(Mathf.Abs(Bp_list.GetComponent<RectTransform>().localPosition.x - B_pos) < 1)
        { Bp_list.transform.localPosition = new Vector3(A_pos, 20, 0); }
    }

    void TEMP()
    {
        //查看已知的藍圖
        foreach (var blueprint in UserManager.Instance.User.Player.KnownBlueprints)
        {
            int blueprintID = blueprint.BlueprintID;//藍圖ID 使用藍圖的話會需要知道
            bool isOrderless = blueprint.IsOrderless;//合成的素材是否無關位置順序
            bool isBlueprintRequired = blueprint.IsBlueprintRequired;//是否必由藍圖合成
          //  bool isSufficient = blueprint.IsSufficientRequirements(new List<Blueprint.ElementInfo> { });//測試是否符合藍圖條件
          //  IEnumerable<Blueprint.ElementInfo> requirements = blueprint.Requirements;//需求的素材 
                                                                                     //用foreach來迭代這個集合的內容 你可能會用來先檢查物品欄中的東西是否符合條件
          //  IEnumerable<Blueprint.ElementInfo> products = blueprint.Products;//成品 和條件的用法差不多
        }
        //手動合成: 上次應該有給過用法了
        //使用藍圖合成
        int usedBlueprintID = 1;
        UserManager.Instance.User.Player.OperationManager.UseBlueprint(usedBlueprintID);//傳入要用的藍圖ID
                                                                                        //手動合成後的回調事件
        UserManager.Instance.User.Player.ResponseManager.OnSynthesizeMaterialResponse += (returnCode, requirements, products) =>
        {
            switch (returnCode)
            {
                case ErrorCode.NoError://如果成功的話
                                       //提取requirements 知道剛剛是拿什麼下去合成的
                                       //提取products 知道合成出了什麼
                    break;
            }
        };
        //使用藍圖的回調事件
        UserManager.Instance.User.Player.ResponseManager.OnUseBlueprintResponse += (returnCode, blueprint) =>
        {
            switch (returnCode)
            {
                case ErrorCode.NoError://成功的話
                                       //blueprint為使用的藍圖
                    break;
            }
        };
    }
}
