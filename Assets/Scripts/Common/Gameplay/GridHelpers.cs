using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class GridHelpers
{
    public static void ResizeGrid(GridLayoutGroup grid, RectTransform rectTransform, int gridSize, int gridMargins = 0, bool autoResizeCells = false)
    {
        if (grid == null || rectTransform == null || gridSize <= 0)
            return;

        float width = rectTransform.rect.width - gridMargins;
        float height = rectTransform.rect.height - gridMargins;

        if (autoResizeCells)
        {
            float squareCellSize = Mathf.Min(width, height) / gridSize;
            grid.cellSize = new Vector2(squareCellSize, squareCellSize);
        }
    }

    public static List<Cell> GetCellNeighbors(this Cell cell, Cell[,] grid, int radius = 1, bool includeCenter = false)
    {
        List<Cell> neighbors = new List<Cell>();

        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        for (int offsetY = -radius; offsetY <= radius; offsetY++)
        {
            for (int offsetX = -radius; offsetX <= radius; offsetX++)
            {
                int nx = cell.Coordinates.x + offsetX;
                int ny = cell.Coordinates.y + offsetY;

                if (!includeCenter && offsetX == 0 && offsetY == 0)
                    continue;

                if (nx >= 0 && ny >= 0 && nx < width && ny < height)
                {
                    Cell neighbor = grid[nx, ny];
                    if (neighbor != null)
                        neighbors.Add(neighbor);
                }
            }
        }

        return neighbors;
    }
    public static bool AreDirectDiagonalNeighbors(Vector2Int a, Vector2Int b, int gridSize)
    {
        if (b.x < 0 || b.x >= gridSize || b.y < 0 || b.y >= gridSize)
            return false;

        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);

        return dx == 1 && dy == 1;
    }


    public static void HighlightCellOutlinesInGrid()
    {
        if (!GridDataManager.HasInstance)
            return;

        Cell[,] grid = GridDataManager.Instance.CellTable;
        int gridSize = GridDataManager.Instance.GridSize;

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                Cell cell = grid[x, y];
                CellOutlines outlines = cell.CellOutlines;

                if(x != gridSize - 1)
                    outlines.RightOutlineVisible(cell.CellGroup != grid[x + 1, y].CellGroup);

                if(y != gridSize - 1)
                    outlines.BottomOutlineVisible(cell.CellGroup != grid[x, y + 1].CellGroup);
            }
        }
    }

    public static void HighlightGridOuterLines()
    {
        if (!GridDataManager.HasInstance)
            return;

        int gridSize = GridDataManager.Instance.GridSize;

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                Cell cell = GridDataManager.Instance.CellTable[x, y];
                CellOutlines outlines = cell.CellOutlines;

                if(x == 0)
                    outlines.LeftOutlineVisible(true);

                if(x == gridSize - 1)
                    outlines.RightOutlineVisible(true);

                if(y == 0)
                    outlines.TopOutlineVisible(true);

                if(y == gridSize - 1)
                    outlines.BottomOutlineVisible(true);
            }
        }
    }
}
