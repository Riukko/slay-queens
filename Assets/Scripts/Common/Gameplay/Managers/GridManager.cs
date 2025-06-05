using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Range(1, 11)]
    public int GridSize;

    [Range(0, 500)]
    public int GridMargins;

    public Cell[,] CellTable = null;
    public GridGenerator GridGenerator;

    [SerializeField]
    private bool autoResizeCells;

    [SerializeField]
    private GameObject cellPrefab;

    public void Start()
    {
        if(GridGenerator == null)
        {
            Debug.LogError("The Grid Generator object should be passed to the Grid Data Manager");
            return;
        }

        GenerateGrid();
    }

    public void GenerateGrid()
    {
        CellTable = GridGenerator.GenerateEmptyGrid(GridSize, GridMargins, autoResizeCells, cellPrefab);
    }

    public void ClearGrid()
    {
        GridGenerator.ClearGridVisuals();
        CellTable = null;
    }

    public void GenerateGridFromTable(int[,] parsedTable)
    {
        GridSize = parsedTable.GetLength(0);
        CellTable = GridGenerator.GenerateGridFromTable(GridSize, GridMargins, autoResizeCells, cellPrefab, parsedTable);
    }

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
