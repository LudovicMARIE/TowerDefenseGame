using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab of the enemy to spawn
    public int enemiesPerWave; // Number of enemies to spawn per wave
    public int waveNumber; // Current wave number

    private Transform spawnPoint; // Spawn position for enemies
    private List<GameObject> enemies = new List<GameObject>(); // List of alive enemies

    void Start()
    {
        GameObject spawnPointObject = GameObject.Find("Pos1");
        if (spawnPointObject != null && spawnPointObject.CompareTag("Pathpoint"))
        {
            spawnPoint = spawnPointObject.transform;
        }
        else
        {
            Debug.LogError("Spawn point 'Pos1' with tag 'Pathpoint' not found!");
        }

        StartWave();
    }

    void Update()
    {
        // Check if all enemies are destroyed to start the next wave
        enemies.RemoveAll(enemy => enemy == null); // Clean up null references
        if (enemies.Count == 0 && waveNumber > 0)
        {
            StartWave();
        }
    }

    private void StartWave()
    {
        waveNumber++;
        Debug.Log($"Starting wave {waveNumber} with {enemiesPerWave} enemies.");
        PlayerController playerController = FindAnyObjectByType<PlayerController>();
        if (playerController != null)
        {
            playerController.UpdateWaveInfo(waveNumber, enemiesPerWave);
        }
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        if (spawnPoint == null)
        {
            Debug.LogError("Spawn point is not set. Cannot spawn enemies.");
            return;
        }

        StartCoroutine(SpawnEnemiesWithDelay());
    }

    private IEnumerator SpawnEnemiesWithDelay()
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            enemies.Add(newEnemy); 
            yield return new WaitForSeconds(1f);
        }
    }
}
