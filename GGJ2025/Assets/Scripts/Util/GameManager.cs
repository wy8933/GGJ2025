using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject gameOverMenu;
    public bool isPaused = false;

    void Awake()
    {
        Instance = this;
    }

    public void Pause()
    {
        isPaused = !isPaused;

        if (isPaused)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void GameOver() 
    {
        Pause();
        gameOverMenu.SetActive(true);
    }




}
