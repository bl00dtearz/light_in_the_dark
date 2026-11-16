using UnityEngine;

// Keeps the player's progress in memory and decides when to write it to disk.
// Every scene has a copy; the first one survives scene loads, later copies destroy themselves.
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    public SaveData Data { get; private set; }

    // Load the save before any Start() runs (the AudioManager reads the volumes in Start)
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Data = SaveSystem.Load();
    }

    // Save moment 1: a level is completed. Open the next level, keep the best time.
    public void LevelCompleted(int levelNumber, float time)
    {
        if (!Data.unlockedLevels.Contains(levelNumber + 1)) Data.unlockedLevels.Add(levelNumber + 1);

        while (Data.bestTimes.Count < levelNumber) Data.bestTimes.Add(0f);
        float best = Data.bestTimes[levelNumber - 1];
        if (best == 0f || time < best) Data.bestTimes[levelNumber - 1] = time;

        SaveSystem.Save(Data);
    }

    // Save moment 2: the Settings panel is closed
    public void Save()
    {
        SaveSystem.Save(Data);
    }

    public bool IsUnlocked(int levelNumber)
    {
        return Data.unlockedLevels.Contains(levelNumber);
    }

    // 0 = the level was never finished
    public float BestTime(int levelNumber)
    {
        return levelNumber <= Data.bestTimes.Count ? Data.bestTimes[levelNumber - 1] : 0f;
    }

    // Reset progress: delete the file and clear the memory too
    // (otherwise the next OnApplicationPause would write the old data back)
    public void ResetProgress()
    {
        SaveSystem.Delete();
        Data = new SaveData();
    }

    // Save moment 3: the app goes to the background (Home button, a call). It may be the last code that runs.
    void OnApplicationPause(bool paused)
    {
        if (paused) SaveSystem.Save(Data);
    }
}
