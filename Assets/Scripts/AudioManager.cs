using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// One sound manager for the whole game: music with a crossfade, sound effects, UI clicks,
// the pause sound and the volumes (stored in save.json by the SaveManager). Every scene has a copy; the first one survives
// scene loads, later copies destroy themselves.
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Mixer")]
    public AudioMixer mixer;
    public AudioMixerSnapshot normalSnapshot;
    public AudioMixerSnapshot pausedSnapshot;
    public float snapshotTime = 0.3f;

    [Header("Sources (2D, Play On Awake off)")]
    public AudioSource musicA;          // Loop on, Output = Tracks
    public AudioSource musicB;          // Loop on, Output = Tracks
    public AudioSource sfx;             // Output = SFX
    public AudioSource ui;              // Output = UI

    [Header("Tuning")]
    public float crossfadeTime = 1f;
    public float minPitch = 0.92f;      // random pitch so repeated sounds are not robotic
    public float maxPitch = 1.08f;
    public AudioClip clickSound;

    AudioSource currentMusic;

    // Keep only the first manager and let it live through scene loads
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        currentMusic = musicA;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Apply the saved volumes (SetFloat does not work in Awake) and run the mixer on real time,
    // so the pause snapshot also moves while Time.timeScale = 0
    void Start()
    {
        mixer.updateMode = AudioMixerUpdateMode.UnscaledTime;
        ApplyVolumes();
    }

    // Send both saved volumes to the mixer (at launch and after Reset progress)
    public void ApplyVolumes()
    {
        SetVolume("MusicVol", GetVolume("MusicVol"));
        SetVolume("SFXVol", GetVolume("SFXVol"));
    }

    // Every new scene starts unpaused, and all its buttons get the click sound
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        normalSnapshot.TransitionTo(0f);
        foreach (Button button in FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            button.onClick.AddListener(PlayClick);
    }

    // ---------- music ----------

    // Switch to another track with a crossfade; the same track keeps playing (no restart on Retry)
    public void PlayMusic(AudioClip clip)
    {
        if (currentMusic.clip == clip && currentMusic.isPlaying) return;

        AudioSource next = (currentMusic == musicA) ? musicB : musicA;
        next.clip = clip;
        next.volume = 0f;
        next.Play();

        StopAllCoroutines();
        StartCoroutine(Crossfade(currentMusic, next));
        currentMusic = next;
    }

    // The old track fades out while the new one fades in (real time, works during pause)
    IEnumerator Crossfade(AudioSource from, AudioSource to)
    {
        float startVolume = from.volume;
        for (float t = 0f; t < 1f; t += Time.unscaledDeltaTime / crossfadeTime)
        {
            from.volume = startVolume * (1f - t);
            to.volume = t;
            yield return null;
        }
        from.Stop();
        to.volume = 1f;
    }

    // ---------- effects ----------

    // One effect on the SFX group with a small random pitch
    public void PlaySfx(AudioClip clip)
    {
        if (clip == null) return;
        sfx.pitch = Random.Range(minPitch, maxPitch);
        sfx.PlayOneShot(clip);
    }

    // Button click on the UI group
    public void PlayClick()
    {
        ui.PlayOneShot(clickSound);
    }

    // Pause: the music goes quieter and muffled (Paused snapshot), Resume brings it back
    public void SetPaused(bool paused)
    {
        if (paused) pausedSnapshot.TransitionTo(snapshotTime);
        else normalSnapshot.TransitionTo(snapshotTime);
    }

    // ---------- volume (0..1 slider value <-> decibels) ----------

    // The slider value from the save (SaveData has the defaults for the first launch)
    public float GetVolume(string param)
    {
        SaveData data = SaveManager.Instance.Data;
        return param == "MusicVol" ? data.musicVolume : data.sfxVolume;
    }

    // Slider 0..1 -> decibels with Log10 (1 = 0 dB, 0.5 = -6 dB, 0.0001 = -80 dB).
    // The value goes into SaveData; the file is written when Settings close or the app pauses.
    public void SetVolume(string param, float value)
    {
        mixer.SetFloat(param, Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f);
        SaveData data = SaveManager.Instance.Data;
        if (param == "MusicVol") data.musicVolume = value;
        else data.sfxVolume = value;
    }
}
