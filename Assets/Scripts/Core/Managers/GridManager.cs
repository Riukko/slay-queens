using System;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    [Range(1, 11)]
    [SerializeField]
    private int _gridSize;

    public int GridSize
    {
        get => _gridSize;
        set
        {
            _gridSize = Mathf.Clamp(value, 1, CellGroupColorPalette.MaxColorCount);
            OnGridSizeChangedEvent?.Invoke(value);
        }
    }

    public event Action<int> OnGridSizeChangedEvent;

    [Range(0, 500)]
    public int GridMargins;

    public Cell[,] CellTable = null;
    public GridGenerator GridGenerator;

    [SerializeField]
    private bool autoResizeCells;

    [SerializeField]
    private GameObject cellPrefab;

    protected override void Awake()
    {
        base.Awake();
        QueenManager.OnQueenAddedEvent += OnQueenAdded;
        QueenManager.OnQueenRemovedEvent += OnQueenRemoved;
    }

    public void Start()
    {
        if (GridGenerator == null)
        {
            Debug.LogError("The Grid Generator object should be passed to the Grid Data Manager");
            return;
        }

        GenerateEmptyGrid();
    }

    public void GenerateEmptyGrid()
    {
        CellTable = GridGenerator.GenerateEmptyGrid(GridSize, GridMargins, autoResizeCells, cellPrefab);
        GridGenerator.ShowGrid(true);
    }

    public void DestroyGrid()
    {
        GridGenerator.DestroyGrid();
        CellTable = null;
    }

    public void ClearGridContent()
    {
        for (int y = 0; y < GridSize; y++)
        {
            for (int x = 0; x < GridSize; x++)
            {
                CellTable[x, y].CellGroup = CellColorGroup.WHITE;
                if (CellTable[x, y].HasQueen)
                    CellTable[x, y].Queen = null;
            }
        }
        GridHelpers.HighlightCellOutlinesInGrid(CellTable);
    }

    public void GenerateGridFromTable(int[,] parsedTable)
    {
        GridSize = parsedTable.GetLength(0);
        CellTable = GridGenerator.GenerateGridFromTable(GridSize, GridMargins, autoResizeCells, cellPrefab, parsedTable);
        GridGenerator.ShowGrid(true);
    }

    public void RefreshAllCellsConflict()
    {
        for (int y = 0; y < GridSize; y++)
        {
            for (int x = 0; x < GridSize; x++)
            {
                Cell cell = CellTable[x, y];

                if (cell.Queen != null)
                {
                    cell.IsCellConflicted = cell.Queen.Conflicts.Count > 0;
                }
                else
                {
                    cell.IsCellConflicted = false;
                }
            }
        }
    }

    public void ChangeGridSizeFromLevelOptions(float newSize)
    {
        GridSize = (int)newSize;
        GenerateEmptyGrid();
    }

    public void OnQueenAdded(Queen queen)
    {

    }

    public void OnQueenRemoved(Queen queen)
    {
        CellTable[queen.Coordinates.x, queen.Coordinates.y].IsCellConflicted = false;
    }
}
