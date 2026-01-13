using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;


public class GameManager : MonoBehaviour
{

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>(); // tried to find existing instance

                if (_instance == null) // if no instance, create instance
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<GameManager>();
                    singletonObject.name = typeof(GameManager).ToString() + "(Singleton)";
                }

                DontDestroyOnLoad(_instance.gameObject); // ensure scene loads
            }
            return _instance;
        }
    }

    // ============================ Scene Tracking and Constants
    public enum OriginScene { MainMenu, PausedGame }; // enum to track where they came from
    public static OriginScene returnToScene = OriginScene.MainMenu;

    [Header("Scene Management")]
    [Tooltip("Build Index of Main Menu.")]
    public int mainMenuSceneIndex = 0;
    [Tooltip("Build Index of Game/Level Scene")]
    public int gameSceneIndex = 1;
    [Tooltip("Build Index of Options menu scene")]
    public int optionsSceneIndex = 2;

    // ============================ Game State & Objects

    [Header("Game State")]
    public bool isGameActive = true;

    [Header("Object References")]
    [SerializeField] private GameObject respawnPanel;
    [SerializeField] private PlayerHealth player;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private GameTimer gameTimer;

    // ============================ Score

    [Header("Score")]
    [SerializeField] private TextMeshProUGUI scoreText;
    private int currentScore = 0;

    // ============================ Camera Tings

    [Header("Camera")]
    [SerializeField] private CameraFollow cameraFollowScript;

    // ============================ Enemies

    [Header("Spawn Settings")]
    [SerializeField] private float initialSpawnX = -40f;
    [SerializeField] private float initialCameraX = 0f;

    // ============================ Event System

    [Header("UI Reference")]
    [Tooltip("Prefab of the Event system")]
    public GameObject eventSystemPrefab;
    private bool hasCreatedEventSystem = false;

    // ============================ Language Settings

    [Header("Language Settings")]
    [Tooltip("0 = English (default)")]
    public int CurrentLanguageIndex = 0;
    public static event Action<int> OnLanguageChanged;

    // ============================ Video Settings

    [Header("Resolution Settings")]
    private Resolution[] availableResolutions;

    [Header("Current Active Settings")]
    public int CurrentFPSLimit = -1;
    public bool IsFullscreen = true;

    public void SetFullscreen(bool isFullscreen)
    {
        IsFullscreen = isFullscreen;
        Screen.fullScreen = isFullscreen;
        Debug.Log($"Fullscreen set to: {isFullscreen}");
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = availableResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        Debug.Log($"Setting resolution to: {resolution.width} x {resolution.height}");

    }

    public void SetVSync(bool isVSync)
    {
        QualitySettings.vSyncCount = isVSync ? 1 : 0;
        if (isVSync)
        {
            Application.targetFrameRate = -1;
        }

        Debug.Log($"V-Sync set to: {QualitySettings.vSyncCount}");
    }

    public void SetFramerate(int fps)
    {
        CurrentFPSLimit = fps;
        Application.targetFrameRate = fps;

        PlayerPrefs.SetInt("SavedFPSLimit", fps);
        PlayerPrefs.Save();

        Debug.Log("Target FPS set and saved to: {fps}");
    }

    public void PopulateResolutions(TMP_Dropdown dropdown)
    {
        availableResolutions = Screen.resolutions;
        dropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < availableResolutions.Length; i++)
        {
            string option = availableResolutions[i].width + " x " + availableResolutions[i].height;

            if (!options.Contains(option))
            {
                options.Add(option);
            }

            if (availableResolutions[i].width == Screen.currentResolution.width &&
                availableResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = options.Count - 1;
            }

            dropdown.AddOptions(options);
            dropdown.value = currentResolutionIndex;
            dropdown.RefreshShownValue();
        }
    }

    // ============================ Broadcast Language Change
    public void SetLanguage(int index)
    {
        CurrentLanguageIndex = index;
        OnLanguageChanged?.Invoke(CurrentLanguageIndex);

        Debug.Log($"Language set to index: {CurrentLanguageIndex}");
    }

    public AudioMixer mainMixer;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;

        CurrentFPSLimit = PlayerPrefs.GetInt("SavedFPSLimit", -1);
        Application.targetFrameRate = CurrentFPSLimit;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene scene)
    {
        if (scene.buildIndex == optionsSceneIndex && returnToScene == OriginScene.PausedGame)
        {
            PauseMenu pm = GameObject.FindAnyObjectByType<PauseMenu>();
            if (pm != null)
            {
                pm.Pause();
            }
        }
    }

    private void Start()
    {
        LoadAudioSettings();

        isGameActive = true;
        respawnPanel.SetActive(false);
        scoreText.text = "Score: " + currentScore;

        Vector3 playerStartPos = playerSpawnPoint.position;
        playerStartPos.x = initialSpawnX;
        Rigidbody playerRb = player.GetComponent<Rigidbody>();

        if (playerRb != null)
        {
            playerRb.position = playerStartPos;
            playerRb.linearVelocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
        }
        else
        {
            player.gameObject.transform.position = playerStartPos;
        }

        if (cameraFollowScript != null)
        {
            cameraFollowScript.TeleportCameraToPlane(1, initialCameraX);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == mainMenuSceneIndex)
        {
            MusicManager musicManager = FindObjectOfType<MusicManager>();
            if (musicManager != null)
            {
                musicManager.PlayMusic();
            }

            StartCoroutine(MainMenuCursorFix());
        }
    }

    private IEnumerator MainMenuCursorFix()
    {
        yield return null;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // ============================ Audio

    public void LoadAudioSettings()
    {
        float master = PlayerPrefs.GetFloat("SavedMaster", 0.75f);
        float music = PlayerPrefs.GetFloat("SavedMusic", 0.75f);
        float sfx = PlayerPrefs.GetFloat("SavedSFX", 0.75f);

        SetMixerVolume("MasterVol", master);
        SetMixerVolume("MusicVol", music);
        SetMixerVolume("SFXVol", sfx);
    }

    private void SetMixerVolume(string parameterName, float sliderValue)
    {
        float dB = Mathf.Log10(Mathf.Max(0.0001f, sliderValue)) * 20;
        mainMixer.SetFloat(parameterName, dB);
    }

    // ============================ Scene Transition Methods

    public void LoadOptions(OriginScene origin)
    {
        returnToScene = origin; // return point
        SceneManager.LoadScene(optionsSceneIndex, LoadSceneMode.Additive); // Load Options Scene
    }

    private IEnumerator RePauseGameAfterLoad(int sceneIndex)
    {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().buildIndex == sceneIndex);
        yield return null;
        PauseMenu pm = GameObject.FindAnyObjectByType<PauseMenu>();
        if (pm != null)
        {
            pm.Pause();
        }
    }

    public void ReturnFromOptions()
    {
        // Called by Options Menu
        if (returnToScene == OriginScene.MainMenu)
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene(mainMenuSceneIndex);
        }
        else
        {

            SceneManager.UnloadSceneAsync(optionsSceneIndex);

            /*            SceneManager.LoadScene(gameSceneIndex);
                        StartCoroutine(RePauseGameAfterLoad(gameSceneIndex)); */
        }
    }

    // ============================ Player Death

    public void PlayerDied()
    {
        //Debug below
        Debug.Log("PlayerDied() was called! Showing respawn panel.");

        isGameActive = false;
        player.gameObject.SetActive(false);
        gameTimer.StopTimer();


        // GameOver Logic can go here
        respawnPanel.SetActive(true);
    }

    // ============================ Respawn

    public void OnRespawnButtonClicked()
    {
        // Resets timer, score, lives
        ResetScore();
        gameTimer.ResetTimer();

        // Resets player position etc
        player.gameObject.transform.position = playerSpawnPoint.position;

        float respawnX = playerSpawnPoint.position.x;

        Rigidbody playerRB = player.GetComponent<Rigidbody>();
        if (playerRB != null)
        {
            playerRB.linearVelocity = Vector3.zero;
            playerRB.angularVelocity = Vector3.zero;
        }

        player.gameObject.SetActive(true);
        player.ResetHealth();
        respawnPanel.SetActive(false);
        isGameActive = true;

        if (cameraFollowScript != null)
        {
            cameraFollowScript.TeleportCameraToPlane(1, respawnX);
            cameraFollowScript.isTeleporting = false;
        }
        else
        {
            Debug.LogError("CameraFollow Script missing from GM");
        }

    }

    // ============================ Score

    public void AddScore(int points)
    {
        currentScore += points;
        scoreText.text = "score: " + currentScore;
    }
    public void ResetScore()
    {
        currentScore = 0;
        scoreText.text = "Score: " + currentScore;
    }



}
