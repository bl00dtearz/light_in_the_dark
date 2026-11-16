using UnityEngine;
using UnityEngine.SceneManagement;

// Buttons of the main menu.
public class MainMenu : MonoBehaviour
{
    // The first level of the game
    public string levelScene = "Level1";

    // The Settings panel with the volume sliders (hidden at start)
    public GameObject settingsPanel;

    void Start()
    {
        settingsPanel.SetActive(false);
    }

    // Settings button
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    // Back button inside the Settings panel
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

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
