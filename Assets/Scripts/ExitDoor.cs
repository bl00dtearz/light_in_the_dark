using UnityEngine;
using UnityEngine.SceneManagement;

// The exit: opens when every orb is collected, then the hero can walk in.
public class ExitDoor : MonoBehaviour
{
    // The scene to load when the hero enters the open door
    public string nextScene = "MainMenu";

    // Sounds, and the pause that lets the win sound play before the next scene loads
    public AudioClip openSound;
    public AudioClip winSound;
    public float winDelay = 1f;

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
    // and the next scene loads a moment later
    void OnTriggerStay2D(Collider2D other)
    {
        if (isOpen && !won && other.CompareTag("Player") && !GameManager.Instance.isDead)
        {
            won = true;
            other.gameObject.SetActive(false);
            AudioManager.Instance.PlaySfx(winSound);
            Invoke(nameof(LoadNextScene), winDelay);
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextScene);
    }
}
