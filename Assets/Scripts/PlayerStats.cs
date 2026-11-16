using UnityEngine;

// Balance numbers of the hero. One asset: Assets/Data/PlayerStats.
[CreateAssetMenu(fileName = "PlayerStats", menuName = "Game/Player Stats")]
public class PlayerStats : ScriptableObject
{
    public float moveSpeed = 6f;     // units per second
    public float jumpForce = 11f;    // upward speed at the start of a jump
}
