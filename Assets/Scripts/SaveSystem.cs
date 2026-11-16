using System.IO;
using UnityEngine;

// Reads and writes save.json in Application.persistentDataPath (a folder that survives app updates).
public static class SaveSystem
{
    static string FilePath => Path.Combine(Application.persistentDataPath, "save.json");
    static string TempPath => Path.Combine(Application.persistentDataPath, "save.tmp");

    // Missing, empty or broken file -> default values, the game always starts
    public static SaveData Load()
    {
        if (!File.Exists(FilePath)) return new SaveData();

        try
        {
            string json = File.ReadAllText(FilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            return data ?? new SaveData();   // empty text gives null
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Broken save file, using defaults: " + e.Message);
            return new SaveData();
        }
    }

    // Write a full new copy to save.tmp first, then swap it in.
    // If the battery dies while writing, save.json still holds the last good save.
    public static void Save(SaveData data)
    {
        File.WriteAllText(TempPath, JsonUtility.ToJson(data, true));
        if (File.Exists(FilePath)) File.Replace(TempPath, FilePath, null);
        else File.Move(TempPath, FilePath);
    }

    public static void Delete()
    {
        if (File.Exists(FilePath)) File.Delete(FilePath);
    }
}
