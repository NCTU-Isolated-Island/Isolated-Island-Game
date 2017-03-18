using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeagullGenerator : MonoBehaviour {

    public static SeagullGenerator Instance { get; private set; }


    [SerializeField]
    private GameObject seagullGameObject;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;

    }

    private void Start()
    {
		InvokeRepeating("Generate",1f,2f);
        
    }

  

	void Generate()
	{
		Vector2 unitCircle = Random.insideUnitCircle * 500f;
		Vector3 position = new Vector3(unitCircle.x, 3f, unitCircle.y);

		Instantiate(seagullGameObject,position,Quaternion.identity);
	}

}