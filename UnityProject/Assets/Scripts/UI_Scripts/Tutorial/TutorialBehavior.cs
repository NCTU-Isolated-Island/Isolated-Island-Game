using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialBehavior : MonoBehaviour
{
    [SerializeField]
    private Image[] tutorialPageList;

    [SerializeField]
    private Button nextPageButton;
    [SerializeField]
    private Button previousPageButton;
    [SerializeField]
    private Button closeButton;

    private int currentPageNum;

    void Start()
    {
        currentPageNum = 0;

        nextPageButton.onClick.AddListener(delegate
        {
            NextPage();
        });
        previousPageButton.onClick.AddListener(delegate
        {
            PreviousPage();
        });
        closeButton.onClick.AddListener(delegate
        {
            Destroy(gameObject);
        });
    }

    public void NextPage()
    {
        if (currentPageNum == tutorialPageList.Length - 1)
            return;

        tutorialPageList[currentPageNum].gameObject.SetActive(false);
        tutorialPageList[currentPageNum + 1].gameObject.SetActive(true);
        currentPageNum++;
    }

    public void PreviousPage()
    {
        if (currentPageNum == 0)
            return;

        tutorialPageList[currentPageNum].gameObject.SetActive(false);
        tutorialPageList[currentPageNum - 1].gameObject.SetActive(true);
        currentPageNum--;
    }

}
