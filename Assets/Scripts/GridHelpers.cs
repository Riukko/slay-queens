using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public static class GridHelpers
{
    public static void ResizeGrid(GridLayoutGroup grid, RectTransform rectTransform, int gridSize)
    {
        if (grid == null || rectTransform == null || gridSize <= 0)
            return;

        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;

        float squareCellSize = Mathf.Min(width, height) / gridSize;

        grid.cellSize = new Vector2(squareCellSize, squareCellSize);
    }

    public static List<GameObject> GetNeighbors(int posX, int posY, GameObject[,] grid, int radius = 1, bool includeCenter = false)
    {
        List<GameObject> neighbors = new List<GameObject>();

        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        for (int offsetY = -radius; offsetY <= radius; offsetY++)
        {
            for (int offsetX = -radius; offsetX <= radius; offsetX++)
            {
                int nx = posX + offsetX;
                int ny = posY + offsetY;

                if (!includeCenter && offsetX == 0 && offsetY == 0)
                    continue;

                if (nx >= 0 && ny >= 0 && nx < width && ny < height)
                {
                    GameObject neighbor = grid[nx, ny];
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
}
