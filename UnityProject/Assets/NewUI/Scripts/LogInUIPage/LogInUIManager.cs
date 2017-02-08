using IsolatedIslandGame.Client.Communication;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Protocol;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogInUIManager : MonoBehaviour
{

    public static LogInUIManager Instance { get; private set; }

    // UI Variable
    private string playerName, speech;
    private GroupType groupType;

    [SerializeField]
    private Text playerNameText, playerSpeechText;
    [SerializeField]
    private GameObject farmerGroupInformationGameObject;
    [SerializeField]
    private GameObject bussinessManGroupInformationGameObject;
    [SerializeField]
    private GameObject animalGroupInformationGameObject;

    [SerializeField]
    private GameObject background;
    [SerializeField]
    private GameObject loginPage;
    [SerializeField]
    private GameObject createCharacterPage;
    [SerializeField]
    private GameObject chooseGroupPage;

    [SerializeField]
    private GameObject nameLengthExceedWarning;

    void InitSetting()
    {
        NextGroup();
    }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        InitSetting();
    }

    public void ToCreateCharacterPage()
    {
        loginPage.SetActive(false);
        createCharacterPage.SetActive(true);
        chooseGroupPage.SetActive(false);
        //background.transform.parent.gameObject.SetActive(false);
    }

    public void ToMainScenePrepare()
    {
        loginPage.SetActive(false);
        StartCoroutine(FadeBackground());
    }

    public IEnumerator FadeBackground()
    {
        float passTime = 0f;
        while(passTime < 1f)
        {
            Color color = background.GetComponent<Image>().color;
            color.a = Mathf.Lerp(1, 0, passTime / 1f);
            background.GetComponent<Image>().color = color;

            passTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        SceneManager.LoadScene("MainScene");
        background.transform.parent.gameObject.SetActive(false);
        UIManager.Instance.SwapPage(UIManager.UIPageType.Main);
    }

    public void LoginRedirection()
    {
        if(PhotonService.Instance.ServerConnected)
        {
            GameManager.Instance.DebugLogin(true);
        }
    }

    public void ToChooseGroupPage()
    {
        loginPage.SetActive(false);
        createCharacterPage.SetActive(false);
        chooseGroupPage.SetActive(true);
    }

    public void DetermineNameAndSpeech()
    {
        if (playerNameText.text.Length > 6)
        {
            nameLengthExceedWarning.SetActive(true);
            return;
        }
        playerName = playerNameText.text;
        speech = playerSpeechText.text;
        ToChooseGroupPage();
    }

    public void CreateCharacter()
    {
        UserManager.Instance.User.Player.OperationManager.CreateCharacter(playerName, speech, groupType);
    }
    public void NextGroup()
    {
        if (groupType == GroupType.Farmer)
            groupType = GroupType.Animal;
        else
            groupType++;

        farmerGroupInformationGameObject.SetActive(false);
        bussinessManGroupInformationGameObject.SetActive(false);
        animalGroupInformationGameObject.SetActive(false);

        switch (groupType)
        {
            case GroupType.Farmer:
                farmerGroupInformationGameObject.SetActive(true);
                break;
            case GroupType.Businessman:
                bussinessManGroupInformationGameObject.SetActive(true);
                break;
            case GroupType.Animal:
                animalGroupInformationGameObject.SetActive(true);
                break;
        }
    }
    public void PreviousGroup()
    {
        if (groupType == GroupType.Animal)
            groupType = GroupType.Farmer;
        else
            groupType--;

        farmerGroupInformationGameObject.SetActive(false);
        bussinessManGroupInformationGameObject.SetActive(false);
        animalGroupInformationGameObject.SetActive(false);

        switch (groupType)
        {
            case GroupType.Farmer:
                farmerGroupInformationGameObject.SetActive(true);
                break;
            case GroupType.Businessman:
                bussinessManGroupInformationGameObject.SetActive(true);
                break;
            case GroupType.Animal:
                animalGroupInformationGameObject.SetActive(true);
                break;
        }
    }
}