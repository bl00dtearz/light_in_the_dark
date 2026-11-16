using UnityEngine;
using UnityEngine.UI;

// A volume slider in Settings. Slider Min Value 0.0001, Max Value 1.
[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour
{
    // The exposed mixer parameter: "MusicVol" or "SFXVol"
    public string param = "MusicVol";

    // Show the saved value, then send every change to the mixer (the AudioManager also saves it)
    void Start()
    {
        Slider slider = GetComponent<Slider>();
        slider.value = AudioManager.Instance.GetVolume(param);
        slider.onValueChanged.AddListener(OnSliderChanged);
    }

    void OnSliderChanged(float value)
    {
        AudioManager.Instance.SetVolume(param, value);
    }
}
