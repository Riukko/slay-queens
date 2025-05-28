using UnityEngine;
using UnityEngine.UI;

public class LevelEditorButton : MonoBehaviour
{
    public ClickActionStatus ClickingAction;
    public CellColorGroup AppliedColor;

    public Image LevelEditorButtonImg;

    public void InitializeWithIndex(int i)
    {
        AppliedColor = CellGroupColorPalette.GetColorGroupAtIndex(i);
        LevelEditorButtonImg = GetComponent<Image>();
        LevelEditorButtonImg.color = CellGroupColorPalette.GetColor(AppliedColor);
    }

    public void OnColorButtonClick()
    {
        if (!ColorManager.HasInstance)
            return;

        ColorManager.Instance.CurrentStatus = ClickingAction;

        if (ClickingAction == ClickActionStatus.COLOR)
        {
            ColorManager.Instance.CurrentColor = AppliedColor;
        }
        else
        {
            ColorManager.Instance.CurrentColor = CellColorGroup.WHITE;
        }
    }
}
