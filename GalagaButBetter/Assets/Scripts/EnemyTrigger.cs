using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnSetup
{
    public GameObject enemyPrefab;
    public Transform customSpawnPoint;
}
public class EnemyTrigger : MonoBehaviour
{
    [Header("Spawn Settings")]
    public List<EnemySpawnSetup> enemiesToSpawn; // what to spawn

    [Header("Behavior")]
    public bool destroyAfterTrigger = true; // destroys trigger post use
    private bool hasSpawned = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasSpawned)
        {
            Debug.Log("EnemyTrigger: Player entered! Spawning...");
            SpawnEnemy();
            hasSpawned = true; // block future spawns from trigger
            if (destroyAfterTrigger) Destroy(gameObject);
        }
    }

    private void SpawnEnemy()
    {
        Debug.Log("EnemyTrigger: SpawnEnemy called. List count: " + enemiesToSpawn.Count);
        // Loop through the list
        foreach (EnemySpawnSetup setup in enemiesToSpawn)
        {
            if (setup.enemyPrefab != null)
            {
                // Determine Position
                Vector3 spawnPos = transform.position;
                if (setup.customSpawnPoint != null)
                {
                    spawnPos = setup.customSpawnPoint.position;
                }
                Instantiate(setup.enemyPrefab, spawnPos, Quaternion.identity);
            }
        }
    }

    // Trigger Gizmo
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        BoxCollider box = GetComponent<BoxCollider>();
        if (box != null)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(box.center, box.size);
        }
    }
}
