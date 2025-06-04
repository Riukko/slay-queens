using UnityEngine;

[ExecuteAlways]
public class CellOutlines : MonoBehaviour
{
    [SerializeField]
    private GameObject OutlineTop;
    [SerializeField]
    private GameObject OutlineBottom;
    [SerializeField]
    private GameObject OutlineLeft;
    [SerializeField]
    private GameObject OutlineRight;

    [Range(1, 20)]
    public int OutlineThickness;
    public void ApplyOutlineThickness(int outlineThickness)
    {
        OutlineTop.GetComponent<RectTransform>().sizeDelta = new Vector2(0, outlineThickness);
        OutlineBottom.GetComponent<RectTransform>().sizeDelta = new Vector2(0, outlineThickness);
        OutlineLeft.GetComponent<RectTransform>().sizeDelta = new Vector2(outlineThickness, 0);
        OutlineRight.GetComponent<RectTransform>().sizeDelta = new Vector2(outlineThickness, 0);
    }

    public void TopOutlineVisible(bool visibility) => OutlineTop.SetActive(visibility);
    public void BottomOutlineVisible(bool visibility) => OutlineBottom.SetActive(visibility);
    public void LeftOutlineVisible(bool visibility) => OutlineLeft.SetActive(visibility);
    public void RightOutlineVisible(bool visibility) => OutlineRight.SetActive(visibility);

    public void SetAllVisibility(bool visibility)
    {
        TopOutlineVisible(visibility);
        BottomOutlineVisible(visibility);
        LeftOutlineVisible(visibility);
        RightOutlineVisible(visibility);
    }

}
