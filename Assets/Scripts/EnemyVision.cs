using UnityEngine;

// One shared check for all enemies: can this enemy see the hero?
public static class EnemyVision
{
    // True when the target is close enough, inside the view angle and no wall is in between
    public static bool CanSee(Vector2 eye, Vector2 facing, Vector2 target, float range, float angle, LayerMask walls)
    {
        Vector2 toTarget = target - eye;

        // 1. Distance
        if (toTarget.magnitude > range) return false;

        // 2. Angle: half of the view angle to each side of the facing direction
        if (Vector2.Angle(facing, toTarget) > angle / 2f) return false;

        // 3. Line of sight: a ray from the eye to the hero must not hit a wall (Ground layer)
        RaycastHit2D hit = Physics2D.Raycast(eye, toTarget.normalized, toTarget.magnitude, walls);
        return hit.collider == null;
    }
}
