using UnityEngine;

// Fits this UI panel into Screen.safeArea, so no button or text sits under a notch,
// a camera hole, a rounded corner or the home bar.
[RequireComponent(typeof(RectTransform))]
public class SafeArea : MonoBehaviour
{
    RectTransform panel;
    Rect lastSafeArea;

    // Apply once at the start
    void Awake()
    {
        panel = GetComponent<RectTransform>();
        Apply();
    }

    // Apply again if the safe area changes (rotation, another device in the Simulator)
    void Update()
    {
        if (Screen.safeArea != lastSafeArea) Apply();
    }

    // Turn the safe area from pixels into 0..1 anchors of the panel
    void Apply()
    {
        lastSafeArea = Screen.safeArea;
        Vector2 min = lastSafeArea.position;
        Vector2 max = lastSafeArea.position + lastSafeArea.size;
        min.x /= Screen.width;
        min.y /= Screen.height;
        max.x /= Screen.width;
        max.y /= Screen.height;
        panel.anchorMin = min;
        panel.anchorMax = max;
    }
}
