using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
        player.gameObject.SetActive(true);
        player.ResetHealth();

        // Hides panel
        respawnPanel.SetActive(false);
        isGameActive = true;
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
