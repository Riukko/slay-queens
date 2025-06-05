using System;

public class GameCell : Cell
{
    public override void OnCellClick()
    {
        int nextStatus = ((int)CellStatus + 1) % Enum.GetValues(typeof(CellStatus)).Length;

        CellStatus = (CellStatus)nextStatus;
    }

    public override void OnCellHoldClick()
    {
        OnCellClick();
    }
}
