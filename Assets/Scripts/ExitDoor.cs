using UnityEngine;
using UnityEngine.SceneManagement;

// The exit: opens when every orb is collected, then the hero can walk in.
public class ExitDoor : MonoBehaviour
{
    // The scene to load when the hero enters the open door
    public string nextScene = "MainMenu";

    Animator animator;
    bool isOpen;

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
        }
    }

    // The hero stands in the doorway of an open door: level complete
    void OnTriggerStay2D(Collider2D other)
    {
        if (isOpen && other.CompareTag("Player") && !GameManager.Instance.isDead)
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
