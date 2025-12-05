using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    public bool isGameActive = true;

    [Header("Object References")]
    [SerializeField] private GameObject respawnPanel;
    [SerializeField] private PlayerHealth player;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private GameTimer gameTimer;

    [Header("Score")]
    [SerializeField] private TextMeshProUGUI scoreText;
    private int currentScore = 0;

    [Header("Camera")]
    [SerializeField] private CameraFollow cameraFollowScript;

    [Header("Spawn Settings")]
    [SerializeField] private float initialSpawnX = -40f;
    [SerializeField] private float initialCameraX = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
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
