using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
#endif

[RequireComponent(typeof(GridLayoutGroup), typeof(RectTransform))]
public class GridGenerator : MonoBehaviour
{
    [SerializeField]
    private GridLayoutGroup grid;

    [SerializeField]
    private RectTransform rectTransform;

    public Cell[,] GenerateEmptyGrid(int gridSize, int gridMargins, bool autoResizeCells, GameObject cellPrefab)
    {
        Cell[,] cellTable = new Cell[gridSize, gridSize];

        if (cellPrefab == null)
        {
            Debug.LogWarning("GridResizer needs a button prefab");
            return null;
        }

        ClearGridVisuals();

        grid.constraintCount = gridSize;

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                cellTable[x,y] = InstantiateCell(cellPrefab, new Vector2Int(x, y), CellColorGroup.WHITE);
            }
        }

        GridHelpers.ResizeGrid(grid, rectTransform, gridSize, gridMargins, autoResizeCells);
        GridHelpers.HighlightGridOuterLines(cellTable);

        return cellTable;
    }

    public Cell[,] GenerateGridFromTable(int gridSize, int gridMargins, bool autoResizeCells, GameObject cellPrefab, int[,] parsedTable)
    {
        Cell[,] cellTable = new Cell[gridSize, gridSize];

        if (cellPrefab == null)
        {
            Debug.LogError("GridResizer needs a button prefab");
            return null;
        }

        ClearGridVisuals();

        grid.constraintCount = gridSize;

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                cellTable[x, y] = InstantiateCell(cellPrefab, new Vector2Int(x, y), CellGroupColorPalette.GetColorGroupAtIndex(parsedTable[x,y]));
            }
        }

        GridHelpers.ResizeGrid(grid, rectTransform, gridSize, gridMargins, autoResizeCells);
        GridHelpers.HighlightCellOutlinesInGrid(cellTable);
        GridHelpers.HighlightGridOuterLines(cellTable);

        return cellTable;
    }

    public void ClearGridVisuals()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            DestroyImmediate(child.gameObject);
        }
    }

    private Cell InstantiateCell(GameObject cellPrefab, Vector2Int coordinates, CellColorGroup colorGroup)
    {

        Cell cell = Instantiate(cellPrefab, transform).GetComponent<Cell>();

        //TODO : Set the right color depending on level loading
        cell.InitializeCell(coordinates, colorGroup);

        return cell;
    }
}
