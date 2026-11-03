using UnityEngine;
using UnityEngine.SceneManagement;

// Pause, Resume, Restart and Menu buttons. Time.timeScale = 0 stops the game.
public class PauseMenu : MonoBehaviour
{
    // The full-screen pause panel (hidden while playing)
    public GameObject pausePanel;
    public string menuScene = "MainMenu";

    // Start every level unpaused
    void Start()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // Pause button (top-right)
    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    // Resume button
    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // Restart button: reload this level
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Menu button: back to the main menu
    public void ToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuScene);
    }
}
