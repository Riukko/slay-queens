using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Cell[,] CellTable;

    public int GridSize;

    #region Singleton
    private static GridManager instance = null;
    public static GridManager Instance => instance;
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
