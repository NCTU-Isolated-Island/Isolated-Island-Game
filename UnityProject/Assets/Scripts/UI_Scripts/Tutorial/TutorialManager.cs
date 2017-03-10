using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {
    
    public static TutorialManager Instance { get; private set; }

    [SerializeField]
    private GameObject tutorialPage;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            OpenTutorialPage();
    }

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
    }

    public void OpenTutorialPage()
    {
        GameObject page = Instantiate(tutorialPage);
        AdjustPageStatus(page);
    }

}
