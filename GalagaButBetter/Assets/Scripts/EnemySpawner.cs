using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyOnePrefab;
    // second enemy prefab will go here exactly the same as above along with serializedfield
    private float enemyOneInterval = 3.5f;
    // you will also need a new enemy interval for each enemy prefab

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     StartCoroutine(spawnEnemy(enemyOneInterval, enemyOnePrefab));
     // call new StartCoroutine for each prefab and interval
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(enemyOneInterval);
        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-6f, 6f)), Quaternion.Euler(new Vector3(90,0,0)));
        StartCoroutine(spawnEnemy(interval, enemy));
    }
}
