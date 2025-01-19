using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderController : MonoBehaviour
{
    [Header("UI References")]
    public Slider slider;                 // Reference to the slider
    public TextMeshProUGUI sliderValueText; // Optional: To display the slider value

    [Header("Slider Settings")]
    public float displayMin = 1f;         // Minimum value displayed on the slider
    public float displayMax = 100f;       // Maximum value displayed on the slider
    public float displayStart = 50f;      // Starting value displayed on the slider

    private float internalMin = -50f;     // Internal minimum value for Audio Mixer
    private float internalMax = 50f;      // Internal maximum value for Audio Mixer

    void Start()
    {
        if (slider != null)
        {
            slider.minValue = displayMin;
            slider.maxValue = displayMax;

            // Initialize slider to match current volume
            float currentVolume = AudioManager.Instance.GetVolume();
            slider.value = Remap(currentVolume, internalMin, internalMax, displayMin, displayMax);

            slider.onValueChanged.AddListener(OnSliderValueChanged);
            UpdateSliderText(slider.value);
        }
    }

    public void OnSliderValueChanged(float displayValue)
    {
        float internalValue = Remap(displayValue, displayMin, displayMax, internalMin, internalMax);

        // Update the volume via the AudioManager
        AudioManager.Instance.SetVolume(internalValue);

        UpdateSliderText(displayValue);
    }

    private void UpdateSliderText(float displayValue)
    {
        if (sliderValueText != null)
        {
            sliderValueText.text = Mathf.RoundToInt(displayValue).ToString();
        }
    }

    // Remaps a value from one range to another
    private float Remap(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        return toMin + (value - fromMin) * (toMax - toMin) / (fromMax - fromMin);
    }
}
