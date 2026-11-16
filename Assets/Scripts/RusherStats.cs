using UnityEngine;

// Balance numbers of the Rusher (charger). One asset per difficulty: Rusher_Stats, Rusher_Hard.
[CreateAssetMenu(fileName = "RusherStats", menuName = "Game/Rusher Stats")]
public class RusherStats : ScriptableObject
{
    public float visionRange = 9f;
    public float visionAngle = 20f;
    public float windUpTime = 0.7f;      // wind-up: shakes with red eyes
    public float chargeSpeed = 10f;
    public float maxChargeDistance = 12f;
    public float wallStopDistance = 0.6f;
    public float stunTime = 1.5f;        // cooldown after the dash
    public float returnSpeed = 2f;
}
