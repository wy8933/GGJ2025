using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject gameOverMenu;
    public bool isPaused = false;
    public GameObject pauseMenu;
    public bool isGameOver;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            isGameOver = false;
        }
        else { 
            Destroy(gameObject);
        }
    }

    public void Pause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0;
            if(!isGameOver)
                pauseMenu.SetActive(true);
        }
        else 
        { 
            Time.timeScale = 1; 
            if(!isGameOver)
                pauseMenu.SetActive(false);
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        Pause();
        gameOverMenu.SetActive(true);
    }




}
