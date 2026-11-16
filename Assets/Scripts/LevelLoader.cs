using UnityEngine;
using UnityEngine.SceneManagement;

// Builds the level the player picked (GameSession.levelIndex) from its LevelData asset.
// The Game scene is the same for every level: only the data is different.
public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;

    // Level_01, Level_02, Level_03 in order
    public LevelData[] levels;

    [Header("Scene objects")]
    public Transform player;
    public SpriteRenderer background;

    [Header("Prefabs")]
    public GameObject blockPrefab;
    public GameObject orbPrefab;
    public GameObject spikesPrefab;
    public GameObject nearSpikesPrefab;
    public GameObject fakeFloorPrefab;
    public GameObject doorPrefab;
    public ShadeEnemy shadePrefab;
    public WatcherEnemy watcherPrefab;
    public RusherEnemy rusherPrefab;

    public LevelData Current => levels[GameSession.levelIndex];

    // Awake runs before every Start, so orbs and enemies already exist when they start
    void Awake()
    {
        Instance = this;
        LevelData level = Current;

        player.position = level.playerStart;
        background.transform.position = level.backgroundCenter;
        background.size = level.backgroundSize;
        Spawn(doorPrefab, level.doorPosition);

        foreach (Block b in level.blocks)
        {
            GameObject block = Spawn(blockPrefab, b.center);
            block.GetComponent<SpriteRenderer>().size = b.size;
            block.GetComponent<BoxCollider2D>().size = b.size;
        }

        foreach (Vector2 p in level.orbs) Spawn(orbPrefab, p);
        foreach (Vector2 p in level.fakeFloors) Spawn(fakeFloorPrefab, p);
        foreach (Vector2 p in level.nearSpikes) Spawn(nearSpikesPrefab, p);

        foreach (Spikes s in level.spikes)
        {
            GameObject spikes = Spawn(spikesPrefab, s.position);
            spikes.GetComponent<SpriteRenderer>().size = new Vector2(s.width, 1f);
            BoxCollider2D box = spikes.GetComponent<BoxCollider2D>();
            box.size = new Vector2(s.width - 0.2f, box.size.y);   // a little smaller than the sprite
        }

        foreach (ShadeSpot s in level.shades)
        {
            ShadeEnemy shade = Instantiate(shadePrefab, s.position, Quaternion.identity, transform);
            shade.stats = s.stats;
            shade.pointA = Point("PointA", s.leftX, s.position.y);
            shade.pointB = Point("PointB", s.rightX, s.position.y);
        }

        foreach (Vector2 p in level.watchers)
        {
            WatcherEnemy watcher = Instantiate(watcherPrefab, p, Quaternion.identity, transform);
            watcher.stats = level.watcherStats;
        }

        foreach (RusherSpot r in level.rushers)
        {
            RusherEnemy rusher = Instantiate(rusherPrefab, r.position, Quaternion.identity, transform);
            rusher.stats = r.stats;
            rusher.facing = r.facing;
        }
    }

    GameObject Spawn(GameObject prefab, Vector2 position)
    {
        return Instantiate(prefab, position, Quaternion.identity, transform);
    }

    // An empty object that marks a patrol end
    Transform Point(string name, float x, float y)
    {
        Transform point = new GameObject(name).transform;
        point.SetParent(transform);
        point.position = new Vector2(x, y);
        return point;
    }

    // Called by the open door: next level, or back to the menu after the last one
    public void LoadNextLevel()
    {
        if (GameSession.levelIndex < levels.Length - 1)
        {
            GameSession.levelIndex++;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
