using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeagullGenerator : MonoBehaviour {

    public static SeagullGenerator Instance { get; private set; }


    [SerializeField]
    private GameObject seagullGameObject;
	[SerializeField]
	private Transform SeagullsFolder;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

    }

    private void Start()
    {
		
		InvokeRepeating("Generate",Random.Range(10,30),Random.Range(45,60));
        
    }

	void Update()
	{
		
	}

	void Generate()
	{
		if(GameManager.Instance.PlayerGameObject == null)
			return;


		Instantiate(seagullGameObject,SeagullsFolder);
	}

}