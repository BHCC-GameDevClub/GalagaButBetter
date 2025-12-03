using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnConfig
{
    public GameObject enemyOnePrefab;
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
        while (config.spawnEnemys)
        {
            // waits for specific enemies interval
            yield return new WaitForSeconds(config.spawnInterval);

            // randomizaed spawn points
            float spawnX = Random.Range(config.spawnBoundsMin.x, config.spawnBoundsMax.x);
            float spawnY = Random.Range(config.spawnBoundsMin.y, config.spawnBoundsMax.y);

            // spawn Z position always 0
            Vector3 spawnPosition = new Vector3(spawnX, spawnY, -7);

            // no rotation
            Instantiate(config.enemyOnePrefab, spawnPosition, Quaternion.identity);

        }
/*        yield return new WaitForSeconds(enemyOneInterval);
        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-6f, 6f)), Quaternion.Euler(new Vector3(90, 0, 0)));
        StartCoroutine(spawnEnemy(interval, enemy)); */
    }
}
