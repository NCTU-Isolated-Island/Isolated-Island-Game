using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Protocol;

public class TutorialManager : MonoBehaviour
{

    public static TutorialManager Instance { get; private set; }

    [SerializeField]
    private GameObject tutorialPage;

    [SerializeField]
    private Sprite[] groupAnimalTutorialPageList;
    [SerializeField]
    private Sprite[] groupBusinessmanTutorialPageList;
    [SerializeField]
    private Sprite[] groupFarmerTutorialPageList;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void AdjustPageStatus(GameObject page)
    {
        page.transform.SetParent(transform);
        page.GetComponent<RectTransform>().localScale = Vector3.one;
        page.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        page.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);

        PlayerInformation info;
        if (PlayerInformationManager.Instance.FindPlayerInformation(GameManager.Instance.PlayerID, out info))
        {
            switch (info.groupType)
            {
                case GroupType.Animal:
                    page.GetComponent<TutorialBehavior>().SetPageList(groupAnimalTutorialPageList);
                    break;
                case GroupType.Businessman:
                    page.GetComponent<TutorialBehavior>().SetPageList(groupBusinessmanTutorialPageList);
                    break;
                case GroupType.Farmer:
                    page.GetComponent<TutorialBehavior>().SetPageList(groupFarmerTutorialPageList);
                    break;
            }
        }
    }

    public void OpenTutorialPage()
    {
        GameObject page = Instantiate(tutorialPage);
        AdjustPageStatus(page);
    }

}