using System.Collections;
using UnityEngine;

// Drives the SpriteFlashDissolve shader (Assets/Shaders) on this sprite:
// Flash() = white for a moment, Dissolve() = the sprite burns away.
// A MaterialPropertyBlock changes the values for this one sprite, without making a new material.
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFx : MonoBehaviour
{
    public float flashTime = 0.08f;
    public float dissolveTime = 0.8f;

    static readonly int FlashId = Shader.PropertyToID("_FlashAmount");
    static readonly int DissolveId = Shader.PropertyToID("_Dissolve");

    SpriteRenderer spriteRenderer;
    MaterialPropertyBlock block;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        block = new MaterialPropertyBlock();
    }

    public void Flash()
    {
        if (Juice.On) StartCoroutine(FlashRoutine());
    }

    public void Dissolve()
    {
        if (Juice.On) StartCoroutine(DissolveRoutine());
    }

    IEnumerator FlashRoutine()
    {
        Set(FlashId, 1f);                                    // pure white
        yield return new WaitForSeconds(flashTime);          // holds longer during hit stop
        for (float a = 1f; a > 0f; a -= Time.deltaTime * 20f) { Set(FlashId, a); yield return null; }
        Set(FlashId, 0f);                                    // back to normal in 0.05 s
    }

    IEnumerator DissolveRoutine()
    {
        for (float t = 0f; t < dissolveTime; t += Time.deltaTime) { Set(DissolveId, t / dissolveTime); yield return null; }
        Set(DissolveId, 1f);                                 // fully gone
    }

    void Set(int id, float value)
    {
        spriteRenderer.GetPropertyBlock(block);
        block.SetFloat(id, value);
        spriteRenderer.SetPropertyBlock(block);
    }
}
