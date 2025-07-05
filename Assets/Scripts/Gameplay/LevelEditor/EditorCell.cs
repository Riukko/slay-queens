using System;

public class EditorCell : Cell
{
    public static event Action OnEditorCellChangedEvent;
    public override void OnCellClick()
    {
        if (!ColorManager.HasInstance)
            return;

        switch (ColorManager.Instance.CurrentStatus)
        {
            case ClickActionStatus.COLOR:
            case ClickActionStatus.ERASE:
                CellGroup = ColorManager.Instance.CurrentColor;
                break;

            case ClickActionStatus.QUEEN:
                CellStatus = (CellStatus == CellStatus.IDLE) ? CellStatus.QUEEN : CellStatus.IDLE;
                break;
        }

        OnEditorCellChangedEvent?.Invoke();
    }

    public override void OnCellHoldClick()
    {
        OnCellClick();
    }

    protected override void OnQueenAssigned(Queen newQueen)
    {
        queenSprite.enabled = true;
        QueenManager.Instance.AddQueen(newQueen, false);
    }
}
