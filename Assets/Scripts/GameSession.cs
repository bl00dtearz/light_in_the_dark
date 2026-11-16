// Remembers which level the player picked in the menu while the Game scene loads.
// Static fields live in memory between scene loads (not saved to disk).
public static class GameSession
{
    public static int levelIndex;   // 0 = Level 1, 1 = Level 2, 2 = Level 3
}
