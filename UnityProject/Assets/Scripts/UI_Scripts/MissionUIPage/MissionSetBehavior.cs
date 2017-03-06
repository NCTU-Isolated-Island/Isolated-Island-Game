using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionSetBehavior : MonoBehaviour {

    private QuestRecordInformation information;
    private Button button;

    private void SetButtonListener()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(delegate {
            MissionUIManager.Instance.OpenMissionDetailPage(information);
        });
    }

    public void SetQuestInfo(QuestRecordInformation information)
    {
        this.information = information;
		SetButtonListener ();
    }
	
}
