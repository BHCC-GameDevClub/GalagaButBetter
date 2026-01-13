using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnConfig
{
    public GameObject enemyOnePrefab;
    public int maxEnemies = 10;
    // second enemy prefab will go here exactly the same as above along with serializedfield
    public float spawnInterval = 3.5f;
    // you will also need a new enemy interval for each enemy prefab
    public bool spawnEnemys = true;

    [Header("Individual Spawn Area")]
    public Vector2 spawnBoundsMin;
    public Vector2 spawnBoundsMax;

}
public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Config")]
    [Tooltip("Add all enemy types here")]
    [SerializeField]
    private List<EnemySpawnConfig> enemyList; // Unity Inspector to add and edit enemies

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Enemy type loop through list
        foreach (EnemySpawnConfig enemyConfig in enemyList)
        {
            StartCoroutine(SpawnEnemy(enemyConfig)); // starts a spawn loop for each one
        }
        /*StartCoroutine(spawnEnemy(enemyOneInterval, enemyOnePrefab));
        // call new StartCoroutine for each prefab and interval */
    }

    private IEnumerator SpawnEnemy(EnemySpawnConfig config)
    {
        List<GameObject> activeEnemies = new List<GameObject>(); // Tracker

        while (config.spawnEnemys)
        {

            // Remove Dead Bodies
            for (int i = activeEnemies.Count - 1; i >= 0; i--)
            {
                if (activeEnemies[i] == null) activeEnemies.RemoveAt(i);
            }

            // Limit Check
            if (activeEnemies.Count < config.maxEnemies)
            {
                // Random Box Calculation
                float randomOffsetX = Random.Range(config.spawnBoundsMin.x, config.spawnBoundsMax.x);
                float randomOffsetY = Random.Range(config.spawnBoundsMin.y, config.spawnBoundsMax.y);

                // Offsets
                Vector3 spawnPosition = transform.position + new Vector3(randomOffsetX, randomOffsetY, 0);

                // Force Z 
                spawnPosition.z = -7;

                // Spawn
                GameObject newEnemy = Instantiate(config.enemyOnePrefab, spawnPosition, Quaternion.identity);

                // Add to tracker
                activeEnemies.Add(newEnemy);
            }

            //  Buffer Check
            yield return new WaitForSeconds(config.spawnInterval);
        }
    }
}
