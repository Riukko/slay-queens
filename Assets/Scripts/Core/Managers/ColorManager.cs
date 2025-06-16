public class ColorManager : Singleton<ColorManager>
{
    public ClickActionStatus CurrentStatus = ClickActionStatus.COLOR;

    public CellColorGroup CurrentColor = CellColorGroup.WHITE;
}

public enum ClickActionStatus
{
    COLOR,
    ERASE,
    QUEEN
}
