using TMPro;
using UnityEngine;

// Shows the frames per second on the screen (for the phone test). Also unlocks 60 FPS:
// Android and iOS run at 30 FPS by default.
public class FpsCounter : MonoBehaviour
{
    public TMP_Text label;

    float timer;
    int frames;

    void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        frames++;
        timer += Time.unscaledDeltaTime;     // real time: correct during pause and hit stop
        if (timer < 0.5f) return;            // new text twice a second
        label.text = Mathf.RoundToInt(frames / timer) + " FPS";
        frames = 0;
        timer = 0f;
    }
}
