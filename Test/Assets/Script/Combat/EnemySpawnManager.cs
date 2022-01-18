using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [Header("要生成的Enemy物件")]
    [SerializeField] GameObject enemy;
    [Header("生成間隔時間")]
    [SerializeField] float spawnTime = 3f;
    [Header("生成數量")]
    [SerializeField] int spawnAmount = 10;
    [SerializeField] Transform[] spawnPoint;

    bool hasBeenTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (hasBeenTrigger) return;

        if (other.gameObject.tag == "Player")
        {
            hasBeenTrigger = true;
            StartCoroutine(Spawn());
        }
    }

    IEnumerator Spawn()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            int spawnPointIndex = Random.Range(0, spawnPoint.Length);

            Instantiate(enemy, spawnPoint[spawnPointIndex].position, spawnPoint[spawnPointIndex].rotation);

            yield return new WaitForSeconds(spawnTime);
        }
    }
}


