using IsolatedIslandGame.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialBehavior : MonoBehaviour
{
    //[SerializeField]
    //private Sprite[] tutorialPageList;
    public const int TutorialPageCount = 8;

    private GroupType playerGrouptype;

    [SerializeField]
    private Button nextPageButton;
    [SerializeField]
    private Button closeButton;

    private int currentPageNum;

    void ChangeSprite()
    {

        switch (playerGrouptype)
        {
            case GroupType.Animal:
                gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("TutorialSprite/信仰" + currentPageNum.ToString());
                print("TutorialSprite / 信仰" + currentPageNum.ToString());
                break;
            case GroupType.Businessman:
                gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("TutorialSprite/科技" + currentPageNum.ToString());
                break;
            case GroupType.Farmer:
                gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("TutorialSprite/自然" + currentPageNum.ToString());
                break;
        }
    }

    void Start()
    {
        currentPageNum = 1;
        ChangeSprite();

        nextPageButton.onClick.AddListener(delegate
        {
            NextPage();
        });
        closeButton.onClick.AddListener(delegate
        {

            Destroy(gameObject);
        });
    }

    public void NextPage()
    {
        if (currentPageNum == TutorialPageCount)
            return;

        currentPageNum++;
        ChangeSprite();

        if (currentPageNum == TutorialPageCount)
        {
            closeButton.gameObject.SetActive(true);
            nextPageButton.gameObject.SetActive(false);
        }
    }

    public void SetGroupType(GroupType grouptype)
    {
        playerGrouptype = grouptype;
    }

}
