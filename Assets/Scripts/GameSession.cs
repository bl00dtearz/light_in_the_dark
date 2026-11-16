// Remembers which level the player picked in the menu while the Game scene loads.
// Static fields live in memory between scene loads (not saved to disk).
public static class GameSession
{
    public static int levelIndex;   // 0 = Level 1, 1 = Level 2, 2 = Level 3

    // 83.4 seconds -> "1:23" (used by the HUD and the level select)
    public static string FormatTime(float seconds)
    {
        int s = (int)seconds;
        return (s / 60) + ":" + (s % 60).ToString("00");
    }
}
