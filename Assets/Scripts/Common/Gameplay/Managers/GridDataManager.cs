using UnityEngine;

public class GridDataManager : MonoBehaviour
{
    public Cell[,] CellTable;

    public int GridSize;

    public GridGenerator GridGenerator;

    public void Start()
    {
        if(GridGenerator == null)
        {
            Debug.LogError("The Grid Generator object should be passed to the Grid Data Manager");
            return;
        }        
    }

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

        GridSize = GridGenerator.GridSize;
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion
}
