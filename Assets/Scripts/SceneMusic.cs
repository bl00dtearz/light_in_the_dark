using UnityEngine;

// Tells the AudioManager which music track belongs to this scene.
public class SceneMusic : MonoBehaviour
{
    public AudioClip track;

    // Start runs after every Awake, so the AudioManager already exists
    void Start()
    {
        AudioManager.Instance.PlayMusic(track);
    }
}
