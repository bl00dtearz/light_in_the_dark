using UnityEngine;

// Game-wide balance numbers: rewards, delays and traps. One asset: Assets/Data/GameRules.
[CreateAssetMenu(fileName = "GameRules", menuName = "Game/Game Rules")]
public class GameRules : ScriptableObject
{
    [Header("Score and timing")]
    public int orbScore = 10;
    public float restartDelay = 1f;      // death -> restart
    public float winDelay = 1f;          // door -> next level

    [Header("Fake floor")]
    public float fakeFloorDelay = 0.3f;  // touch -> fall
    public float fakeFloorGravity = 3f;
    public float fakeFloorLifetime = 3f;

    [Header("Near spikes (level 3)")]
    public float nearSpikesRange = 2.5f;     // flash when the hero is this close
    public float nearSpikesFlashTime = 0.25f;
    public float nearSpikesBrightness = 2.2f;
}
