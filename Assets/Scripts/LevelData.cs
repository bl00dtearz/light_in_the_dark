using UnityEngine;

// One level as data: where the stone blocks, orbs, traps, enemies, the hero and the door are.
// The LevelLoader in the Game scene builds any level from one of these assets.
[CreateAssetMenu(fileName = "Level", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    public string levelName = "Level 1";
    public Vector2 playerStart;
    public Vector2 doorPosition;

    [Header("Background (center and size)")]
    public Vector2 backgroundCenter;
    public Vector2 backgroundSize = new Vector2(80f, 20f);

    [Header("Level parts")]
    public Block[] blocks;            // ground, platforms, walls, ceilings
    public Vector2[] orbs;
    public Spikes[] spikes;           // flash every few seconds (Spikes prefab)
    public Vector2[] nearSpikes;      // flash only when the hero comes close (level 3)
    public Vector2[] fakeFloors;

    [Header("Enemies")]
    public ShadeSpot[] shades;
    public Vector2[] watchers;
    public WatcherStats watcherStats;
    public RusherSpot[] rushers;
}

// A stone block: center and size in units
[System.Serializable]
public class Block
{
    public Vector2 center;
    public Vector2 size = Vector2.one;
}

// A row of flashing spikes
[System.Serializable]
public class Spikes
{
    public Vector2 position;
    public float width = 1f;
}

// A Shade walks between leftX and rightX
[System.Serializable]
public class ShadeSpot
{
    public Vector2 position;
    public float leftX;
    public float rightX;
    public ShadeStats stats;
}

// A Rusher waits at its position and looks left (-1) or right (1)
[System.Serializable]
public class RusherSpot
{
    public Vector2 position;
    public int facing = -1;
    public RusherStats stats;
}
