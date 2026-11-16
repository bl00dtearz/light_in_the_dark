using UnityEngine;
using UnityEngine.SceneManagement;

// Reset progress button in Settings, with an in-game confirm panel.
public class ResetProgress : MonoBehaviour
{
    // Full-screen dark panel with Cancel and Delete (hidden at start)
    public GameObject confirmPanel;

    void Start()
    {
        confirmPanel.SetActive(false);
    }

    // Reset progress button: only asks
    public void AskReset()
    {
        confirmPanel.SetActive(true);
    }

    // Cancel: nothing is deleted
    public void Cancel()
    {
        confirmPanel.SetActive(false);
    }

    // Delete: clear the save file and the memory, put the default volumes back, reload the menu
    public void ConfirmReset()
    {
        SaveManager.Instance.ResetProgress();
        AudioManager.Instance.ApplyVolumes();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
