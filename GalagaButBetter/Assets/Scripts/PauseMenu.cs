using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false; // Tracks Pause

    [Header("UI Reference")]
    public GameObject pauseMenuUI;

    [Header("Cursor Settings")]
    public Image arrowImage;
    public float offsetX = -50f;
    public Sprite[] arrowFrames = new Sprite[3];
    public float frameRate = 0.1f;
    private Coroutine cursorAnimation;

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

        if (cursorAnimation != null)
        {
            StopCoroutine(cursorAnimation);
        }

        if (arrowImage != null)
        {
            arrowImage.gameObject.SetActive(false);
        }
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true); // Show Menu
        Time.timeScale = 0f; // Freezes the game
        GameIsPaused = true;

        if (arrowImage != null)
        {
            cursorAnimation = StartCoroutine(AnimateArrow());
        }
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

    public void SetCursorTarget(RectTransform targetButton)
    {
        if (arrowImage == null || targetButton == null) return;
        Vector3 targetPosition = targetButton.position;
        Vector3 newPosition = new Vector3(
            targetPosition.x + offsetX,
            targetPosition.y,
            targetPosition.z
        );

        arrowImage.transform.position = newPosition;
        arrowImage.gameObject.SetActive(true);
    }

    IEnumerator AnimateArrow()
    {
        int currentIndex = 0;
        while (true)
        {
            if (arrowFrames.Length > 0)
            {
                arrowImage.sprite = arrowFrames[currentIndex];
                currentIndex = (currentIndex + 1) % arrowFrames.Length;
            }
            yield return new WaitForSecondsRealtime(frameRate);
        }
    }
}
