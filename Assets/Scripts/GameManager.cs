using UnityEngine;
using UnityEngine.SceneManagement;

// Keeps the level state: score, orbs and whether the hero is dead.
// A tiny singleton: other scripts reach it through GameManager.Instance.
public class GameManager : MonoBehaviour
{
    // The one GameManager of the current scene
    public static GameManager Instance;

    // Level state that the HUD and the door read
    public int score;
    public int totalOrbs;
    public int orbsLeft;
    public bool isDead;

    // Orb score and restart delay come from a data asset (GameRules)
    public GameRules rules;

    // Sounds: the hit at the moment of death, the "lose" jingle when the level restarts
    public AudioClip deathSound;
    public AudioClip loseSound;

    // Register this object as the singleton before any Start() runs
    void Awake()
    {
        Instance = this;
    }

    // Every orb calls this once in its Start(), so we know how many there are
    public void RegisterOrb()
    {
        totalOrbs++;
        orbsLeft++;
    }

    // Called by an orb when the hero picks it up
    public void AddOrb()
    {
        score += rules.orbScore;
        orbsLeft--;
    }

    // One life: the first death ends the run and restarts the level after a short delay
    public void Die()
    {
        if (isDead) return;
        isDead = true;
        AudioManager.Instance.PlaySfx(deathSound);
        // Invoke calls RestartLevel once, restartDelay seconds from now
        Invoke(nameof(RestartLevel), rules.restartDelay);
    }

    // Reload the current scene: everything, including the score, starts again
    void RestartLevel()
    {
        AudioManager.Instance.PlaySfx(loseSound);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
