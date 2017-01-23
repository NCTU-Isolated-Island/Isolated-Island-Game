using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
        BGMDictionary["a_girl_from_osaka_1"] = "Sound1";
        BGMDictionary["a_new_generation_1"] = "Sound2";

        Exited = true;
    }
}
