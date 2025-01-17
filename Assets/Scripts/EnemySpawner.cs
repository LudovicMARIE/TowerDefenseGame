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
    public PlayerController playerController;

    void Start()
    {
        GameObject spawnPointObject = GameObject.Find("Pos01");
        if (spawnPointObject != null && spawnPointObject.CompareTag("Pathpoint"))
        {
            spawnPoint = spawnPointObject.transform;
        }
        else
        {
            Debug.LogError("Spawn point 'Pos01' with tag 'Pathpoint' not found!");
        }

        StartWave();
    }

    void Update()
    {
        // Check if all enemies are destroyed to start the next wave
        enemies.RemoveAll(enemy => enemy == null); // Clean up null references
        if (playerController.isGameOver == false)
        {
            if (enemies.Count == 0 && waveNumber >= 0)
            {
                StartWave();
            }
        }
        else
        {
            waveNumber = 0;
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
            if (playerController.isGameOver == true)
            {
                i = 6;
                break;
            }
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            enemies.Add(newEnemy);


            EnemyController enemyController = newEnemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.hp = (float)(1 + (waveNumber * 0.2));
                enemyController.goldValue = 1;
                enemyController.scoreValue = 20;
            }
            else
            {
                Debug.LogError("EnemyController script not found on the spawned enemy!");
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
