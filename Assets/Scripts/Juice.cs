using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

// Game feel in one place: particle bursts, camera shake, hit stop and the EFFECTS on/off switch.
// Other scripts call Juice.Instance.Spawn / Shake / Freeze, like they call AudioManager.PlaySfx.
public class Juice : MonoBehaviour
{
    public static Juice Instance;

    // Effects on or off (button in the pause menu). Static, so it stays the same in the next level.
    public static bool On = true;

    // Main Camera is a child of CameraRig: CameraFollow moves the rig, the shake moves only the camera
    public Transform cameraTransform;
    public Volume postVolume;
    public TMP_Text switchLabel;

    Vector3 cameraHome;
    Coroutine shakeRoutine;
    bool frozen;

    void Awake()
    {
        Instance = this;
        cameraHome = cameraTransform.localPosition;
        ShowState();
    }

    // A one-shot particle effect. Looping is off and Stop Action is Destroy, so it removes itself.
    public void Spawn(ParticleSystem fx, Vector3 position)
    {
        if (On) Instantiate(fx, position, Quaternion.identity);
    }

    // Shake the camera for a short time; the shake gets weaker towards the end
    public void Shake(float time, float strength)
    {
        if (!On) return;
        if (shakeRoutine != null) StopCoroutine(shakeRoutine);
        shakeRoutine = StartCoroutine(ShakeRoutine(time, strength));
    }

    IEnumerator ShakeRoutine(float time, float strength)
    {
        for (float t = 0f; t < time; t += Time.deltaTime)   // scaled time: the shake stops on pause
        {
            float k = 1f - t / time;                          // 1 -> 0
            cameraTransform.localPosition = cameraHome + (Vector3)Random.insideUnitCircle * strength * k * k;
            yield return null;
        }
        cameraTransform.localPosition = cameraHome;
    }

    // Hit stop: freeze the whole game for a moment
    public void Freeze(float seconds)
    {
        if (!On || frozen || Time.timeScale == 0f) return;   // already frozen or paused
        StartCoroutine(FreezeRoutine(seconds));
    }

    IEnumerator FreezeRoutine(float seconds)
    {
        frozen = true;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(seconds);    // real time: WaitForSeconds would never end at timeScale 0
        frozen = false;
        if (!PauseMenu.IsPaused) Time.timeScale = 1f;        // the player pressed pause during the freeze: stay paused
    }

    // Scene change in the middle of a freeze: never leave the game frozen
    void OnDisable()
    {
        if (frozen) Time.timeScale = 1f;
        frozen = false;
    }

    // EFFECTS button in the pause menu
    public void Toggle()
    {
        On = !On;
        ShowState();
    }

    void ShowState()
    {
        postVolume.enabled = On;
        switchLabel.text = On ? "EFFECTS: ON" : "EFFECTS: OFF";
    }
}
