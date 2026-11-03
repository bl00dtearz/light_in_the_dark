using UnityEngine;

// Moves the camera smoothly after the hero.
public class CameraFollow : MonoBehaviour
{
    // What to follow and where the camera sits relative to it
    public Transform target;
    public Vector3 offset = new Vector3(0f, 1.5f, -10f);
    public float smoothSpeed = 5f;

    // LateUpdate runs after the hero has moved this frame
    void LateUpdate()
    {
        if (target == null) return;
        Vector3 goal = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, goal, smoothSpeed * Time.deltaTime);
    }
}
