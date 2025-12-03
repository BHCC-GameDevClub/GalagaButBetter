using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [Header("CursorSettings")]
    public Image arrowImage;
    public float offsetX = -50f;
    public Sprite[] arrowFrames = new Sprite[3];
    public float frameRate = 0.1f;
    private Coroutine animationCoroutine;

    void Start()
    {
        if (arrowImage != null)
        {
            animationCoroutine = StartCoroutine(AnimateArrow());
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
}