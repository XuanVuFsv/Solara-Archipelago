using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : Singleton<EnemySpawner>
{
    public Transform player;
    public int numberOfEnemiesToSpawn;
    public float spawnDelay;
    public List<EnemyNavMesh> enemyPrefabs = new List<EnemyNavMesh>();
    public List<Transform> spawnPlace = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            spawnPlace.Add(child);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Spawn(float seconInRealLifeVsIngame)
    {
        spawnDelay = 2 / seconInRealLifeVsIngame * (60 * 60 / (numberOfEnemiesToSpawn * TimeManager.Instance.Days));
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        WaitForSeconds wait = new WaitForSeconds(spawnDelay);

        int spawnedEnemies = 0;

        if (TimeManager.Instance.Days <= 1)
        {
            while (spawnedEnemies < numberOfEnemiesToSpawn)
            {
                SpawnRoundRobinEnemy(1, enemyPrefabs[0].gameObject);
                spawnedEnemies++;

                yield return wait;
            }
        }
        else if (TimeManager.Instance.Days > 1)
        {
            while (spawnedEnemies < numberOfEnemiesToSpawn * TimeManager.Instance.Days)
            {
                SpawnRoundRobinEnemy(1, enemyPrefabs[0].gameObject);
                spawnedEnemies++;

                yield return wait;
            }
        }
    }

    private void SpawnRoundRobinEnemy(int num, GameObject enemy)
    {
        for (int i = 0; i < num; i++)
        {
            int index = (int)Random.Range(0, spawnPlace.Count - 1);
            GameObject newEnemy = Instantiate(enemy, spawnPlace[index].position, Quaternion.identity);
            newEnemy.GetComponent<EnemyNavMesh>().movePosTransform = player;
        }
    }
}
