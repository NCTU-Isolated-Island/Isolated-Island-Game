using IsolatedIslandGame.Library;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionSetBehavior : MonoBehaviour {

    private Quest thisQuest;
    private Button button;

    void Start()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(delegate
        {
            MissionUIManager.Instance.OpenMissionDetailPage(thisQuest);
        });
    }

    public void SetQuestInfo(Quest quest)
    {
        thisQuest = quest;
    }
	
}
