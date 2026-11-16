using UnityEngine;

// Balance numbers of the Watcher (shooter) and its light bolt.
[CreateAssetMenu(fileName = "WatcherStats", menuName = "Game/Watcher Stats")]
public class WatcherStats : ScriptableObject
{
    public float visionRange = 7f;
    public float visionAngle = 360f;
    public float aimTime = 0.6f;       // wind-up: red aim line
    public float shootTime = 0.2f;
    public float cooldownTime = 1.5f;
    public float boltSpeed = 7f;
    public float boltLifetime = 4f;
}
