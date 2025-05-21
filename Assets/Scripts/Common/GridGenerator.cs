using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
#endif

[ExecuteAlways]
[RequireComponent(typeof(GridLayoutGroup), typeof(RectTransform))]
public class GridGenerator : MonoBehaviour
{
    [Range(1, 15)]
    public int gridSize;

    public bool autoResize;
    public GameObject cellPrefab;

    private GridLayoutGroup grid;

    private RectTransform rectTransform;

    [SerializeField]
    private CellGroupColorPalette cellGroupColorPalette;
    public static CellGroupColorPalette CellGroupColorPalette;

    public void Start()
    {
        grid = GetComponent<GridLayoutGroup>();
        rectTransform = GetComponent<RectTransform>();

        GenerateGrid();
    }

    private void OnValidate()
    {
        if (autoResize)
        {
            GridHelpers.ResizeGrid(grid, rectTransform, gridSize);
        }

        if (GridManager.HasInstance)
        {
            GridManager.Instance.CellTable = new Cell[gridSize, gridSize];
        }

        if (CellGroupColorPalette == null)
        {
            CellGroupColorPalette = cellGroupColorPalette;
        }
    }

    public void GenerateGrid()
    {
        if (cellPrefab == null)
        {
            Debug.LogWarning("GridResizer needs a button prefab");
            return;
        }

        ClearGrid();


        grid.constraintCount = gridSize;

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                InstantiateCell(new Vector2Int(x, y));
            }
        }

        GridHelpers.ResizeGrid(grid, rectTransform, gridSize);

        if (GridManager.HasInstance)
        {
            GridManager.Instance.GridSize = gridSize;
        }
    }

    public void InstantiateCell(Vector2Int coordinates)
    {

        Cell cell = Instantiate(cellPrefab, transform).GetComponent<Cell>();
        cell.InitializeCell(coordinates);

        if (GridManager.HasInstance)
        {
            GridManager.Instance.CellTable[coordinates.x, coordinates.y] = cell;
        }
    }

    public void ClearGrid()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            DestroyImmediate(child.gameObject);
        }

        if (GridManager.HasInstance)
        {
            GridManager.Instance.CellTable = new Cell[gridSize, gridSize];
        }
    }
}
