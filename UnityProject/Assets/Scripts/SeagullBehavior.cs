using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeagullBehavior : MonoBehaviour
{
    private Vector3 moveDirection;
    private float lifeTime;

    void Awake()
    {
        lifeTime = 120f;

        Vector2 unitCircle = Random.insideUnitCircle;
        Vector3 tempFlyDirection = new Vector3(unitCircle.x, 0f, unitCircle.y);

        Vector3 spawnPosition = GameManager.Instance.PlayerGameObject.transform.position - tempFlyDirection * 50f
            + Vector3.up * 7f;
        moveDirection = Quaternion.Euler(0f, Random.Range(-30f, 30f), 0f) * tempFlyDirection;

        transform.position = spawnPosition;
        transform.rotation = Quaternion.LookRotation(moveDirection * -1);
    }

    void Update()
    {
        Move();

        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    void Move()
    {
        transform.position += moveDirection * Time.deltaTime * 6.5f;
    }
}