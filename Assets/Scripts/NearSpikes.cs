using UnityEngine;
using UnityEngine.Rendering.Universal;

// Level 3 trap: invisible spikes that flash for a moment only when the hero comes close.
// Touching them kills the hero (Hazard on the same object).
public class NearSpikes : MonoBehaviour
{
    // Range, flash time and brightness come from a data asset (GameRules)
    public GameRules rules;
    public Light2D flashLight;

    Transform hero;
    bool heroWasNear;
    float flashTimer;

    void Start()
    {
        hero = GameObject.FindWithTag("Player").transform;
        flashLight.intensity = 0f;
    }

    void Update()
    {
        // Flash once each time the hero walks into the range
        bool heroIsNear = Vector2.Distance(hero.position, transform.position) < rules.nearSpikesRange;
        if (heroIsNear && !heroWasNear) flashTimer = rules.nearSpikesFlashTime;
        heroWasNear = heroIsNear;

        flashTimer -= Time.deltaTime;
        flashLight.intensity = flashTimer > 0f ? rules.nearSpikesBrightness : 0f;
    }
}
