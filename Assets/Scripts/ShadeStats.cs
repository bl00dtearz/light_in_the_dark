using UnityEngine;

// Balance numbers of the Shade (patroller). One asset per difficulty: Shade_Stats, Shade_Hard.
[CreateAssetMenu(fileName = "ShadeStats", menuName = "Game/Shade Stats")]
public class ShadeStats : ScriptableObject
{
    public float patrolSpeed = 1.5f;
    public float chaseSpeed = 3.5f;
    public float visionRange = 6f;
    public float visionAngle = 90f;
    public float alertTime = 0.5f;   // wind-up before the chase
    public float lostTime = 1.5f;    // cooldown before going back to patrol
}
