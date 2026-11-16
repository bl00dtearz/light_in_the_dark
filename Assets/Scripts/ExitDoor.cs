using UnityEngine;

// The exit: opens when every orb is collected, then the hero can walk in.
public class ExitDoor : MonoBehaviour
{
    // The pause that lets the win sound play before the next scene loads comes from GameRules
    public GameRules rules;
    public AudioClip openSound;
    public AudioClip winSound;

    Animator animator;
    bool isOpen;
    bool won;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Open the door once, when no orbs are left (Animator: Closed -> Open)
    void Update()
    {
        if (!isOpen && GameManager.Instance.orbsLeft == 0)
        {
            isOpen = true;
            animator.SetBool("Open", true);
            AudioManager.Instance.PlaySfx(openSound);
        }
    }

    // The hero stands in the doorway of an open door: he goes inside, the win sound plays,
    // and the next level loads a moment later
    void OnTriggerStay2D(Collider2D other)
    {
        if (isOpen && !won && other.CompareTag("Player") && !GameManager.Instance.isDead)
        {
            won = true;
            other.gameObject.SetActive(false);
            AudioManager.Instance.PlaySfx(winSound);
            // Save moment: the level is done (opens the next one, keeps the best time)
            SaveManager.Instance.LevelCompleted(GameSession.levelIndex + 1, Time.timeSinceLevelLoad);
            Invoke(nameof(LoadNextScene), rules.winDelay);
        }
    }

    void LoadNextScene()
    {
        LevelLoader.Instance.LoadNextLevel();
    }
}
