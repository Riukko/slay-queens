using UnityEngine;

public class GridDataManager : MonoBehaviour
{
    public Cell[,] CellTable;

    public int GridSize;

    #region Singleton
    private static GridDataManager instance = null;
    public static GridDataManager Instance => instance;
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
