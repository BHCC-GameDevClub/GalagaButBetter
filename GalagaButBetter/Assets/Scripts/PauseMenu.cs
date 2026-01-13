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

    [Tooltip("Drag custom cursor here")]
    public Texture2D customCursorTexture;

    [Tooltip("Pixel offset from top left corner of cursor")]
    public Vector2 cursorHotspot = Vector2.zero;

    void Start()
    {
        SetupInitialGameplayState();
    }

    void SetupInitialGameplayState()
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

// ######################## Hide Cursor
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

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

// ============================ Resume
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

// ######################## Hide Cursor
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

// ============================ Pause
    public void Pause()
    {
        pauseMenuUI.SetActive(true); // Show Menu
        Time.timeScale = 0f; // Freezes the game
        GameIsPaused = true;

        if (arrowImage != null)
        {
            cursorAnimation = StartCoroutine(AnimateArrow());
        }

// ######################## Show Cursor
        if (customCursorTexture != null)
        {
            Cursor.SetCursor(customCursorTexture, cursorHotspot, CursorMode.Auto);
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

// ============================ Options
    public void LoadOptions() // options tbd
    {
        if (GameManager.Instance != null)
        {
            pauseMenuUI.SetActive(false); // Hide current pause menu ui
            GameManager.Instance.LoadOptions(GameManager.OriginScene.PausedGame); // Return flag set
        }
        else
        {
            Debug.LogError("GameManager instance not found!");
        }
    }

// ============================ B2 Main Menu
    public void LoadMainMenu() // Back to main menu
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene(0);
    }

// ============================ Cursor Targeting & Animation
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
