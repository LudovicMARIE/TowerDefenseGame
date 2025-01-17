using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    private bool isPaused = false; 
    public GameObject startMenuUI;
    public GameObject HUD;
    public GameObject gameOverUI;

    public bool isGameOver = false;

    public int gold = 10; 
    public int score = 0;
    public int hp = 10;
    public int maxHP = 10;

    private int waveNumber; 
    public int numberOfEnemies;

    public TextMeshProUGUI textMeshProHP; 
    public TextMeshProUGUI textMeshProGold;
    private string sceneManager;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sceneManager = SceneManager.GetActiveScene().name;
        gold = 10;
        score = 0;
        hp = 10;
        waveNumber = 0;
        gameOverUI.SetActive(false);

        textMeshProHP.text = "10/10";
        textMeshProGold.text = gold.ToString();

        startMenuUI = GameObject.FindGameObjectWithTag("PlayMenu");
        HUD = GameObject.FindGameObjectWithTag("HUD");
        HUD.SetActive(false);
        PauseGame();
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


        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (Time.timeScale == 1f)
            {
                Time.timeScale = 3f;
            }
            else 
            {
                if (Time.timeScale == 3f)
                {
                    Time.timeScale = 1f;
                }
            }
        }
    }


    public void AddGold(int amount)
    {
        gold += amount;

        textMeshProGold.text = gold.ToString();
    }

    public void RemoveGold(int amount)
    {
        gold -= amount;

        textMeshProGold.text = gold.ToString();
    }

    public void AddScore(int amount)
    {
        score += amount;
    }


    public void LoseHP(int amount)
    {
        if (isGameOver)
        {
            return;
        }
        hp -= amount;
        textMeshProHP.text = hp.ToString() + "/" + maxHP.ToString();

        if (hp <= 0)
        {
            gameOverUI.SetActive(true);
            Time.timeScale = 0f;
            isGameOver = true;
            FindAnyObjectByType<AudioManager>().PlaySound("GameOver");
        }
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
        if (startMenuUI != null)
        {
            startMenuUI.SetActive(true); // Show the pause menu
        }
        Debug.Log("Game Paused");
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Resume game time
        if (startMenuUI != null)
        {
            startMenuUI.SetActive(false); // Hide the pause menu
        }
        if (HUD != null)
        {
            HUD.SetActive(true);
        }
        Debug.Log("Game Resumed");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit(); // Quit the application
    }

    public void RetryGame()
    {
        SceneManager.LoadScene(sceneManager);
        Start();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        gameOverUI.SetActive(false);
        Time.timeScale = 1f;
        isGameOver = false;
        HUD.SetActive(true);
        gold = 10;
        score = 0;
        hp = 10;
        waveNumber = 0;

    }


    #endregion
}
