using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
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
}