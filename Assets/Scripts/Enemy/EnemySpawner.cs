using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VitsehLand.Scripts.Pattern.Observer;
using VitsehLand.Scripts.TimeManager;

namespace VitsehLand.Scripts.Enemy
{
    public class EnemySpawner : GameObserver
    {
        public Transform player;
        public int numberOfEnemiesToSpawn;
        public float spawnDelay;
        public List<EnemyNavMesh> enemyPrefabs = new List<EnemyNavMesh>();
        public List<Transform> spawnPlace = new List<Transform>();

        public GameEvent atNightEvent;

        public int spawnDuration = 8;
        public int defaultIndex = 1;

        // Start is called before the first frame update
        void Start()
        {
            AddGameEventToObserver(atNightEvent);
            foreach (Transform child in transform)
            {
                spawnPlace.Add(child);
            }
        }

        public override void Execute(IGameEvent gEvent, float val)
        {
            Spawn(val);
        }

        public void Spawn(float seconInRealLifeVsIngame)
        {
            spawnDelay = spawnDuration * 60 * 60 / seconInRealLifeVsIngame / (numberOfEnemiesToSpawn * GameTimeManager.Instance.Days + 1);
            StartCoroutine(SpawnEnemies());
        }

        private IEnumerator SpawnEnemies()
        {
            WaitForSeconds wait = new WaitForSeconds(spawnDelay);

            int spawnedEnemies = 0;

            if (GameTimeManager.Instance.Days <= 1)
            {
                while (spawnedEnemies < numberOfEnemiesToSpawn)
                {
                    //SpawnRoundRobinEnemy(1, enemyPrefabs[defaultIndex].gameObject);
                    SpawnRoundRobinEnemy(1, enemyPrefabs[Random.Range(0, enemyPrefabs.Count)].gameObject);
                    spawnedEnemies++;

                    yield return wait;
                }
            }
            else if (GameTimeManager.Instance.Days > 1)
            {
                while (spawnedEnemies < numberOfEnemiesToSpawn * GameTimeManager.Instance.Days)
                {
                    SpawnRoundRobinEnemy(1, enemyPrefabs[Random.Range(0, enemyPrefabs.Count)].gameObject);
                    spawnedEnemies++;

                    yield return wait;
                }
            }
        }

        private void SpawnRoundRobinEnemy(int num, GameObject enemy)
        {
            for (int i = 0; i < num; i++)
            {
                int index = Random.Range(0, spawnPlace.Count - 1);
                GameObject newEnemy = Instantiate(enemy, spawnPlace[index].position, Quaternion.identity);
                newEnemy.GetComponent<EnemyNavMesh>().movePosTransform = player;
            }
        }

        private void OnDestroy()
        {
            RemoveGameEventFromObserver(atNightEvent);
        }

        private void OnDisable()
        {
            RemoveGameEventFromObserver(atNightEvent);
        }
    }
}