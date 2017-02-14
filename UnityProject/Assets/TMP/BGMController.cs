using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using IsolatedIslandGame.Library;

public class BGMController : MonoBehaviour {

    private bool Exited; // true if already exit previous
    private Dictionary<string, string> BGMDictionary;

    private string PrevBGMpath;

	void OnTriggerStay(Collider EnterOcean)
    {
        if (Exited == false) return;
        if (BGMDictionary.ContainsKey(EnterOcean.name) == false) return;

        string nextBGMpath = BGMDictionary[EnterOcean.name];

        if (nextBGMpath == PrevBGMpath) return;
        PrevBGMpath = nextBGMpath;

        AudioManager.Instance.SetBGM(nextBGMpath);
        Exited = false;
    }

    void OnTriggerExit(Collider ExitOcean)
    {
        Exited = true;
    }

    void Awake()
    {
        BGMDictionary = new Dictionary<string, string>();
    }

    void Start() // for testing
    {
        BGMDictionary["winter_light_1"] = "winter";
        BGMDictionary["cosy_and_warm_1"] = "cozyandwarm";

        Exited = true;
    }
}
