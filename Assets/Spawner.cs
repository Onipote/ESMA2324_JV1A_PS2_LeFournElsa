using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float spawnRate = 1.0f;
    public float timeBetweenWaves = 3.0f;

    public int enemyCount;

    public GameObject zomBee;

    public bool waveIsDone = true;

    void Update()
    {
        if (waveIsDone == true)
        {
            StartCoroutine(waveSpawner());
        }
    }

    IEnumerator waveSpawner()
    {
        waveIsDone = false;

        for (int i = 0; i < enemyCount; i++) //spawn 1 wave
        {
            GameObject enemyClone = Instantiate(zomBee);

            yield return new WaitForSeconds(spawnRate); //delay between spawnning
        }

        spawnRate -= 0.1f; //wave harder and harder
        enemyCount ++;

        yield return new WaitForSeconds(timeBetweenWaves); //delay between waves

        waveIsDone = true;
    }
}
