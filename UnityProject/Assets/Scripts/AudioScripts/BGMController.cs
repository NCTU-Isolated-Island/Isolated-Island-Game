using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using IsolatedIslandGame.Library;

public class BGMController : MonoBehaviour
{
    public static BGMController Instance { get; private set; }

    private bool Exited; // true if already exit previous
    private Dictionary<string, string> BGMDictionary = new Dictionary<string, string>();

    private string PrevBGMpath;

    void OnTriggerEnter(Collider EnterOcean)
    {
        print("OnTriggerEnter");
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
        if (BGMDictionary.ContainsKey(oceanName) == false) return;

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

        BGMDictionary["winter_light_1"] = "computer_center";
        BGMDictionary["cosy_and_warm_1"] = "cozyandwarm";
        BGMDictionary["a_girl_from_osaka_1"] = "osaka";
        BGMDictionary["a_new_generation_1"] = "winter";
        BGMDictionary["hopeful_journey_1"] = "journey";
        BGMDictionary["indy_racing_1"] = "racing";
        BGMDictionary["lost_soul_1"] = "lostsoul";

        Exited = true;
    }
}