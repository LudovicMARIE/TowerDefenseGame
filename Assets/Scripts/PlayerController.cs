using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerController : MonoBehaviour
{

    private bool isPaused = false; 
    public GameObject pauseMenuUI;

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
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


    #region Pause

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Freeze game time
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true); // Show the pause menu
        }
        Debug.Log("Game Paused");
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Resume game time
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false); // Hide the pause menu
        }
        Debug.Log("Game Resumed");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit(); // Quit the application
    }


    #endregion
}
