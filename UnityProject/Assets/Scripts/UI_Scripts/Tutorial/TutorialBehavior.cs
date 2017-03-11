using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialBehavior : MonoBehaviour
{
    [SerializeField]
    private Sprite[] tutorialPageList;

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
        gameObject.GetComponent<Image>().sprite = tutorialPageList[currentPageNum];

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

        gameObject.GetComponent<Image>().sprite = tutorialPageList[currentPageNum + 1];

        currentPageNum++;
    }

    public void PreviousPage()
    {
        if (currentPageNum == 0)
            return;

        gameObject.GetComponent<Image>().sprite = tutorialPageList[currentPageNum - 1];

        currentPageNum--;
    }

    public void SetPageList(Sprite[] list)
    {
        tutorialPageList = list;
    }

}
