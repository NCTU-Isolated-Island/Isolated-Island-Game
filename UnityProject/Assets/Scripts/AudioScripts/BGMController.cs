using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using IsolatedIslandGame.Library;

public class BGMController : MonoBehaviour {

    public static BGMController Instance { get; private set; }

    private bool Exited; // true if already exit previous
    private Dictionary<string, string> BGMDictionary;

    private string PrevBGMpath;

	void OnTriggerEnter(Collider EnterOcean)
    {
        if (Exited == false) return;
        if (BGMDictionary.ContainsKey(EnterOcean.name) == false) return;
        ChangeBGM(EnterOcean.name);

        Exited = false;
    }

    void OnTriggerExit(Collider ExitOcean)
    {
        Exited = true;
    }

    public void ChangeBGM(string oceanName)
    {
        string nextBGMpath = BGMDictionary[oceanName];

        if (nextBGMpath == PrevBGMpath) return;
        PrevBGMpath = nextBGMpath;

        AudioManager.Instance.SetBGM(nextBGMpath);
    }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);

        BGMDictionary = new Dictionary<string, string>();
    }

    void Start() // for testing
    {
        BGMDictionary["winter_light_1"] = "winter";
        BGMDictionary["cosy_and_warm_1"] = "cozyandwarm";

        Exited = true;
    }
}
