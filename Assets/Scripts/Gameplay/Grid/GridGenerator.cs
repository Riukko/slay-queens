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

    private Cell[,] GenerateGridCore(
    int gridSize,
    int gridMargins,
    bool autoResizeCells,
    GameObject cellPrefab,
    System.Func<int, int, CellColorGroup> colorSelector,
    bool highlightInnerLines = true)
    {
        Cell[,] cellTable = new Cell[gridSize, gridSize];

        if (cellPrefab == null)
        {
            Debug.LogWarning("GridGenerator needs a button prefab");
            return null;
        }

        DestroyGrid();

        grid.constraintCount = gridSize;

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                var color = colorSelector(x, y);
                cellTable[x, y] = InstantiateCell(cellPrefab, new Vector2Int(x, y), color);
            }
        }

        GridHelpers.ResizeGrid(grid, rectTransform, gridSize, gridMargins, autoResizeCells);

        if (highlightInnerLines)
            GridHelpers.HighlightCellOutlinesInGrid(cellTable);

        GridHelpers.HighlightGridOuterLines(cellTable);

        return cellTable;
    }

    public Cell[,] GenerateEmptyGrid(int gridSize, int gridMargins, bool autoResizeCells, GameObject cellPrefab)
    {
        return GenerateGridCore(
            gridSize,
            gridMargins,
            autoResizeCells,
            cellPrefab,
            (x, y) => CellColorGroup.WHITE,
            highlightInnerLines: false
        );
    }

    public Cell[,] GenerateGridFromTable(int gridSize, int gridMargins, bool autoResizeCells, GameObject cellPrefab, int[,] parsedTable)
    {
        return GenerateGridCore(
            gridSize,
            gridMargins,
            autoResizeCells,
            cellPrefab,
            (x, y) => CellGroupColorPalette.GetColorGroupAtIndex(parsedTable[x, y]),
            highlightInnerLines: true
        );
    }

    public void DestroyGrid()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            DestroyImmediate(child.gameObject);
        }
    }

    public void ShowGrid(bool show)
    {
        grid.gameObject.SetActive(show);
    }

    private Cell InstantiateCell(GameObject cellPrefab, Vector2Int coordinates, CellColorGroup colorGroup)
    {

        Cell cell = Instantiate(cellPrefab, transform).GetComponent<Cell>();

        //TODO : Set the right color depending on level loading
        cell.InitializeCell(coordinates, colorGroup);

        return cell;
    }
}
