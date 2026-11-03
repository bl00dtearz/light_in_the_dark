using UnityEngine;
using UnityEngine.SceneManagement;

// Buttons of the main menu.
public class MainMenu : MonoBehaviour
{
    // The first level of the game
    public string levelScene = "Level1";

    // Play button
    public void Play()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelScene);
    }

    // Quit button (works in a build; in the Editor it only prints a message)
    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
