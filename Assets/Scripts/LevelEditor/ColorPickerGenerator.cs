using UnityEngine;
using UnityEngine.UI;

public class ColorPickerGenerator : MonoBehaviour
{
    public GameObject ColorButtonPrefab;
    public RectTransform ColorPickerParent;

    public void Start()
    {
        InitializeColorPicker();
    }

    public void InitializeColorPicker()
    {
        if (!GridManager.HasInstance || GridManager.Instance.GridSize <= 0)
        {
            Debug.LogWarning("GridDataManager singleton is not initialized");
            return;
        }

        for (int i = 0; i < GridManager.Instance.GridSize; i++)
        {
            GameObject colorButton = Instantiate(ColorButtonPrefab, ColorPickerParent);
            colorButton.GetComponent<LevelEditorButton>().InitializeWithIndex(i);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(ColorPickerParent);

    }

    public void ClearColors()
    {
        for (int i = ColorPickerParent.childCount - 1; i >= 0; i--)
        {
            Transform color = ColorPickerParent.GetChild(i);
            DestroyImmediate(color.gameObject);
        }
    }
}
