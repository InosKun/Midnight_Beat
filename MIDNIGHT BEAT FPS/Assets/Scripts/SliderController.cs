using UnityEngine;
using UnityEngine.UI;
using TMPro; // Si usas TextMeshPro

public class SliderController : MonoBehaviour
{
    public Slider slider; // Referencia al Slider
    public TextMeshProUGUI sliderValueText; // Para mostrar el valor (opcional)

    void Start()
    {
        // Asegúrate de que el Slider tenga un valor inicial
        if (slider != null)
        {
            slider.onValueChanged.AddListener(OnSliderValueChanged);
            UpdateSliderText(slider.value);
        }
    }

    // Método que se llama cuando el Slider cambia
    public void OnSliderValueChanged(float value)
    {
        Debug.Log("Valor del Slider: " + value);
        UpdateSliderText(value);
    }

    // Actualiza el texto con el valor del Slider
    private void UpdateSliderText(float value)
    {
        if (sliderValueText != null)
        {
            sliderValueText.text = Mathf.RoundToInt(value).ToString(); // Opcional: Redondea a entero
        }
    }
}

