using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class LevelOptionsView : MonoBehaviour
{
    public Slider sizeSlider;
    public TextMeshProUGUI sizeValueText;

    public void Start()
    {
        if (!GridManager.HasInstance)
            return;

        sizeSlider.value = GridManager.Instance.GridSize;
        sizeValueText.text = GridManager.Instance.GridSize.ToString();

        GridManager.Instance.OnGridSizeChangedEvent += UpdateSliderValue;
    }

    public void UpdateSliderValueText(float value)
    {
        sizeValueText.text = value.ToString();
    }

    public void UpdateSliderValue(int newSize)
    {
        sizeSlider.value = newSize;
        sizeValueText.text = newSize.ToString();
    }
}
