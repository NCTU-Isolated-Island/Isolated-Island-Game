using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeagullGenerator : MonoBehaviour {

    public static SeagullGenerator Instance { get; private set; }

    [SerializeField]
    private float minGenerateTimeInterval;
    [SerializeField]
    private float maxGenerateTimeInterval;

    [SerializeField]
    private Vector3 minGeneratePosition;
    [SerializeField]
    private Vector3 maxGeneratePosition;

    [SerializeField]
    private GameObject seagullGameObject;

    private float coolDown;

    private Vector3 nextGeneratePosition;
    private float nextGenerateTimeInterval;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Update()
    {
        if (coolDown <= 0f)
        {
            GameObject seagull = Instantiate(seagullGameObject, nextGeneratePosition, Quaternion.identity);

            nextGeneratePosition = new Vector3(Random.Range(minGeneratePosition.x, maxGeneratePosition.x), Random.Range(minGeneratePosition.y, maxGeneratePosition.y), Random.Range(minGeneratePosition.z, maxGeneratePosition.z));

            nextGenerateTimeInterval = Random.Range(minGenerateTimeInterval, maxGenerateTimeInterval);

            coolDown = nextGenerateTimeInterval;

            StartCoroutine(DestroySeagull(seagull, Random.Range(minGenerateTimeInterval, maxGenerateTimeInterval)));
        }
        else
            coolDown -= Time.deltaTime;
    }

    private IEnumerator DestroySeagull(GameObject seagull , float countDown)
    {
        float passTime = 0f;
        while(passTime < countDown)
        {
            passTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Destroy(seagull);
    }

}