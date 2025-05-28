using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public ClickActionStatus CurrentStatus = ClickActionStatus.COLOR;

    public CellColorGroup CurrentColor = CellColorGroup.WHITE;

    #region Singleton
    private static ColorManager instance = null;
    public static ColorManager Instance => instance;
    public static bool HasInstance => instance != null;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion
}

public enum ClickActionStatus
{
    COLOR,
    ERASE,
    QUEEN
}
