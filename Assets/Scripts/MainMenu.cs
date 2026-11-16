using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Buttons of the main menu.
public class MainMenu : MonoBehaviour
{
    // The one scene that builds every level from data
    public string gameScene = "Game";

    // The Settings panel with the volume sliders (hidden at start)
    public GameObject settingsPanel;

    // The level select panel: one button and one label per level (hidden at start)
    public GameObject levelsPanel;
    public Button[] levelButtons;
    public TMP_Text[] levelLabels;

    void Start()
    {
        settingsPanel.SetActive(false);
        levelsPanel.SetActive(false);
    }

    // Play button: show the levels. Locked ones can't be pressed; finished ones show the best time.
    public void OpenLevels()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int number = i + 1;
            bool unlocked = SaveManager.Instance.IsUnlocked(number);
            float best = SaveManager.Instance.BestTime(number);

            levelButtons[i].interactable = unlocked;
            if (!unlocked) levelLabels[i].text = "LEVEL " + number + "\n<size=60%>LOCKED</size>";
            else if (best > 0f) levelLabels[i].text = "LEVEL " + number + "\n<size=60%>BEST " + GameSession.FormatTime(best) + "</size>";
            else levelLabels[i].text = "LEVEL " + number + "\n<size=60%>NEW</size>";
        }
        levelsPanel.SetActive(true);
    }

    // Back button inside the level select
    public void CloseLevels()
    {
        levelsPanel.SetActive(false);
    }

    // A level button (index 0, 1, 2 is set on each button in the Inspector)
    public void PlayLevel(int index)
    {
        Time.timeScale = 1f;
        GameSession.levelIndex = index;
        SceneManager.LoadScene(gameScene);
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

    // Quit button (works in a build; in the Editor it only prints a message)
    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
