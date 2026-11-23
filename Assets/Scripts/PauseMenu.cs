using UnityEngine;
using UnityEngine.SceneManagement;

// Pause, Resume, Restart and Menu buttons. Time.timeScale = 0 stops the game.
// The music is softened by the AudioManager; a new scene always starts unpaused.
public class PauseMenu : MonoBehaviour
{
    // The full-screen pause panel (hidden while playing)
    public GameObject pausePanel;
    public string menuScene = "MainMenu";

    // Juice reads this: a hit stop must not end a pause
    public static bool IsPaused;

    // Start every level unpaused
    void Start()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
    }

    // Pause button (top-right)
    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
        AudioManager.Instance.SetPaused(true);
    }

    // Resume button
    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
        AudioManager.Instance.SetPaused(false);
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
