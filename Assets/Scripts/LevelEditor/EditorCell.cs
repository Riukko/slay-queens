public class EditorCell : Cell
{
    public override void OnCellClick()
    {
        if (!ColorManager.HasInstance)
            return;

        switch (ColorManager.Instance.CurrentStatus)
        {
            case ClickActionStatus.COLOR:
            case ClickActionStatus.ERASE:
                CellGroup = ColorManager.Instance.CurrentColor;
                ApplyColor();
                break;

            case ClickActionStatus.QUEEN:
                CellStatus = (CellStatus == CellStatus.IDLE) ? CellStatus.QUEEN : CellStatus.IDLE;
                break;
        }
    }
}
