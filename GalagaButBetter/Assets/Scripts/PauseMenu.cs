using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false; // Tracks Pause

    [Header("UI Reference")]
    public GameObject pauseMenuUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Hide Menu
        Time.timeScale = 1f; // Unfreezes the game
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true); // Show Menu
        Time.timeScale = 0f; // Freezes the game
        GameIsPaused = true;
    }

    public void LoadOptions() // options tbd
    {
        Debug.Log("Options Coming Soon");
    }

    public void LoadMainMenu() // Back to main menu
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene(0);
    }
}
