using UnityEngine;
using UnityEngine.SceneManagement;

// Buttons of the main menu.
public class MainMenu : MonoBehaviour
{
    // The one scene that builds every level from data
    public string gameScene = "Game";

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

    // Back button inside the Settings panel. Save moment: the volumes go to save.json.
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        SaveManager.Instance.Save();
    }

    // Play button: start from Level 1
    public void Play()
    {
        Time.timeScale = 1f;
        GameSession.levelIndex = 0;
        SceneManager.LoadScene(gameScene);
    }

    // Quit button (works in a build; in the Editor it only prints a message)
    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
