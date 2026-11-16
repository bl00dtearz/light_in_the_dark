using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Shows the heart (one life), the level, the orbs, the score and the time on the screen.
public class HUD : MonoBehaviour
{
    // UI objects, set in the Inspector
    public TMP_Text scoreText;
    public Image heartImage;
    public Sprite heartFull;
    public Sprite heartEmpty;

    // Copy the values from the GameManager every frame
    void Update()
    {
        GameManager gm = GameManager.Instance;
        int collected = gm.totalOrbs - gm.orbsLeft;
        scoreText.text = LevelLoader.Instance.Current.levelName.ToUpper() + "   ORBS " + collected + "/" + gm.totalOrbs
                       + "   SCORE " + gm.score + "   " + GameSession.FormatTime(Time.timeSinceLevelLoad);
        heartImage.sprite = gm.isDead ? heartEmpty : heartFull;
    }
}
