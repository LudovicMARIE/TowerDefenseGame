using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerController : MonoBehaviour
{

    public int gold = 10; 
    public int score = 0; 

    public int waveNumber; 
    public int numberOfEnemies;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void AddGold(int amount)
    {
        gold += amount;
    }

    public void AddScore(int amount)
    {
        score += amount;
    }

    public void UpdateWaveInfo(int currentWave, int enemiesInWave)
    {
        waveNumber = currentWave;
        numberOfEnemies = enemiesInWave;
    }
}
