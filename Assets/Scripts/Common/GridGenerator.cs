using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
#endif

[ExecuteAlways]
[RequireComponent(typeof(GridLayoutGroup), typeof(RectTransform))]
public class GridGenerator : MonoBehaviour
{
    [Range(1, 11)]
    public int GridSize;

    [SerializeField]
    public bool autoResize;

    [SerializeField]
    public bool setRandomColors;

    [SerializeField]
    private GameObject cellPrefab;

    private GridLayoutGroup grid;

    private RectTransform rectTransform;

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
            GridHelpers.ResizeGrid(grid, rectTransform, GridSize);
        }

        if (GridDataManager.HasInstance)
        {
            GridDataManager.Instance.CellTable = new Cell[GridSize, GridSize];
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


        grid.constraintCount = GridSize;

        for (int x = 0; x < GridSize; x++)
        {
            for (int y = 0; y < GridSize; y++)
            {
                InstantiateCell(new Vector2Int(x, y));
            }
        }

        GridHelpers.ResizeGrid(grid, rectTransform, GridSize);

        if (GridDataManager.HasInstance)
        {
            GridDataManager.Instance.GridSize = GridSize;
        }
    }

    public void InstantiateCell(Vector2Int coordinates)
    {

        Cell cell = Instantiate(cellPrefab, transform).GetComponent<Cell>();

        //TODO : Set the right color depending on level loading
        cell.InitializeCell(coordinates, setRandomColors ? CellGroupColorPalette.GetRandomColorGroup() : CellColorGroup.WHITE);

        if (GridDataManager.HasInstance)
        {
            GridDataManager.Instance.CellTable[coordinates.x, coordinates.y] = cell;
        }
    }

    public void ClearGrid()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            DestroyImmediate(child.gameObject);
        }

        if (GridDataManager.HasInstance)
        {
            GridDataManager.Instance.CellTable = new Cell[GridSize, GridSize];
        }
    }
}
