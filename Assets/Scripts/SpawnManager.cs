using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject harderEnemyPrefab;
    public GameObject powerupPrefab;
    public GameObject misslePowerupPrefab;
    private float spawnRange = 6.0f;
    private int enemyCount;
    private int waveNumber = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        if (enemyCount == 0)
        {
            waveNumber++;
            SpawnEnemyWave(waveNumber);
            Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);

            int missleWaitTime = Random.Range(0,5);
            StartCoroutine(SpawnMisslePowerup(missleWaitTime));
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);

        Vector3 randomSpawnPos = new Vector3(spawnPosX, 0, spawnPosZ);

        return randomSpawnPos;
    }

    private void SpawnEnemyWave(int numOfEnemies)
    {
        for(int i = 0; i < numOfEnemies; i++)
        {
            // "Roll a dice" and spawn a difficult enemy if the outcome is 6
            int randomNumber = Random.Range(1,7);
            GameObject enemyToSpawn = (randomNumber == 6) ? harderEnemyPrefab : enemyPrefab;
            
            Instantiate(enemyToSpawn, GenerateSpawnPosition(), enemyToSpawn.transform.rotation);
        }
    }

    private IEnumerator SpawnMisslePowerup(int timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        Instantiate(misslePowerupPrefab, GenerateSpawnPosition(), misslePowerupPrefab.transform.rotation);
    }
}
