using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Shows the heart (one life) and the score on the screen.
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
        scoreText.text = "SCORE " + gm.score + "    ORBS " + collected + "/" + gm.totalOrbs;
        heartImage.sprite = gm.isDead ? heartEmpty : heartFull;
    }
}
