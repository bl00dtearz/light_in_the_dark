using System.Collections.Generic;

// Everything that is saved to save.json. JsonUtility saves public fields only (List yes, Dictionary no).
[System.Serializable]
public class SaveData
{
    public int version = 1;                                // raise when the format changes
    public List<int> unlockedLevels = new List<int> { 1 }; // level 1 is always open
    public List<float> bestTimes = new List<float>();      // best time per level in seconds, 0 = not finished
    public float musicVolume = 0.8f;                       // slider values 0..1
    public float sfxVolume = 0.8f;
}
