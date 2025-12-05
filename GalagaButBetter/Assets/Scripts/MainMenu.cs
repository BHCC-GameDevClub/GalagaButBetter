using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [Header("Cursor Animation Settings")]
    public Image arrowImage;
    public float offsetX = -50f;
    public Sprite[] arrowFrames = new Sprite[3];
    public float frameRate = 0.1f;
    private Coroutine arrowAnimationCoroutine;

    [Header("Logo Animation Settings")]
    public Image logoImage;
    public Sprite[] logoFrames = new Sprite[12];
    public float[] logoFrameDurations = new float[12];
    private Coroutine logoAnimationCoroutine;

    [Header("Background Animation Settings")]
    public Image background;
    public Sprite[] backgroundFrames = new Sprite[6];
    public float[] backgroundframeDurations = new float[6];
    private Coroutine backgroundAnimatedCoroutine;

    void Start()
    {
        if (arrowImage != null)
        {
            arrowAnimationCoroutine = StartCoroutine(AnimateArrow());
        }

        if (logoImage != null)
        {
            logoAnimationCoroutine = StartCoroutine(AnimateLogo());
        }
        
        if (background != null)
        {
            backgroundAnimatedCoroutine = StartCoroutine(AnimateBackground());
        }
    }
    public void PlayGame() // Called by Start Button
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // MainMenu index 0, Game 1
    }

    public void OpenOptions() // Called by Options Button
    {
        Debug.Log("Options Coming Soon");
    }

    public void QuitGame() // Called by Quit Button
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    // Cursor movement functions
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
            arrowImage.sprite = arrowFrames[currentIndex];
            currentIndex = (currentIndex +1) % arrowFrames.Length;
            yield return new WaitForSeconds(frameRate);
        }
    }

    IEnumerator AnimateLogo()
    {
        if (logoFrames.Length == 0 || logoFrames.Length != logoFrameDurations.Length)
        {
            Debug.LogError("frames and durations must be same size");
            yield break;
        }
        int currentIndex = 0;
        while (true)
        {
            logoImage.sprite = logoFrames[currentIndex];
            yield return new WaitForSeconds(logoFrameDurations[currentIndex]);
            currentIndex = (currentIndex + 1) % logoFrames.Length;
        }
    }

    IEnumerator AnimateBackground()
    {
        if (backgroundFrames.Length == 0)
        {
            Debug.LogError("background frame empty");
            yield break;
        }
        int currentIndex = 0;
        while (true)
        {
            background.sprite = backgroundFrames[currentIndex];
            yield return new WaitForSeconds(backgroundframeDurations[currentIndex]);
            currentIndex = (currentIndex + 1) % backgroundFrames.Length;
        }
    }
}