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
        UIManager.Instance.GetUIElement<ConfirmationPopup>(AccessibleUIElementTag.ConfirmationPopup)
            .Show("Are you sure you want to clear the grid?",
                  () =>
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
                  }, null,
                  new PopupStyle
                  {
                      ConfirmButtonText = "Clear",
                      CancelButtonText = "Cancel",
                  });
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
        int newGridSize = (int)newSize;

        if (!HasInstance || CellTable == null)
            return;

        int oldGridSize = GridSize;
        Cell[,] oldGrid = CellTable;

        int[,] oldData = LevelFileHelpers.ExtractGridDataTable(oldGrid);

        int[,] newData = new int[newGridSize, newGridSize];

        int whiteIndex = ((int)CellColorGroup.WHITE);

        for (int y = 0; y < newGridSize; y++)
        {
            for (int x = 0; x < newGridSize; x++)
            {
                newData[x, y] = whiteIndex;
            }
        }

        int minSize = Mathf.Min(oldGridSize, newGridSize);

        for (int y = 0; y < minSize; y++)
        {
            for (int x = 0; x < minSize; x++)
            {
                newData[x, y] = oldData[x, y];
            }
        }

        GridSize = newGridSize;

        CellTable = GridGenerator.GenerateGridFromTable(
            newGridSize,
            GridMargins,
            true,
            cellPrefab,
            newData
        );
    }

    public void OnQueenAdded(Queen queen)
    {

    }

    public void OnQueenRemoved(Queen queen)
    {
        CellTable[queen.Coordinates.x, queen.Coordinates.y].IsCellConflicted = false;
    }
}
