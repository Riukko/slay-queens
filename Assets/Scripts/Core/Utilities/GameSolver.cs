using System.Collections.Generic;
using UnityEngine;

public static class GameSolver
{
    public static bool IsSolvable(int[,] grid)
    {
        int size = grid.GetLength(0);

        var uniqueColors = new HashSet<int>();
        for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
                uniqueColors.Add(grid[x, y]);

        if (!HasValidColorCount(grid, size))
        {
            Debug.LogWarning("Grid does not have exactly one color per row/column.");
            return false;
        }

        if (!CheckContiguousColors(grid, uniqueColors))
        {
            Debug.LogWarning("Grid has non-contiguous color regions.");
            return false;
        }

        List<Vector2Int> placedQueens = new List<Vector2Int>();
        return SolveRecursive(grid, size, 0, placedQueens);
    }

    private static bool HasValidColorCount(int[,] grid, int expectedCount)
    {
        var uniqueColors = new HashSet<int>();
        int size = grid.GetLength(0);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                uniqueColors.Add(grid[x, y]);
            }
        }

        return uniqueColors.Count == expectedCount;
    }

    private static bool SolveRecursive(int[,] grid, int size, int row, List<Vector2Int> placedQueens)
    {
        if (row >= size)
            return true;

        for (int col = 0; col < size; col++)
        {
            if (HasConflict(row, col, placedQueens, grid))
                continue;

            placedQueens.Add(new Vector2Int(row, col));

            if (SolveRecursive(grid, size, row + 1, placedQueens))
                return true;

            placedQueens.RemoveAt(placedQueens.Count - 1);
        }

        return false;
    }

    private static bool HasConflict(int row, int col, List<Vector2Int> placedQueens, int[,] grid)
    {
        Vector2Int current = new Vector2Int(row, col);

        foreach (var q in placedQueens)
        {
            if (GridHelpers.AreOnTheSameColumn(current, q))
                return true;

            if (GridHelpers.AreOnTheSameRow(current, q))
                return true;

            if (GridHelpers.AreDirectDiagonalNeighbors(current, q, grid.GetLength(0)))
                return true;

            if (GridHelpers.HaveSameColor(current, q, grid))
                return true;
        }

        return false;
    }

    private static bool CheckContiguousColors(int[,] grid, HashSet<int> colors)
    {
        int size = grid.GetLength(0);
        bool[,] visited = new bool[size, size];

        foreach (int color in colors)
        {
            bool foundRegion = false;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    if (grid[x, y] == color && !visited[x, y])
                    {
                        if (foundRegion)
                        {
                            return false;
                        }

                        FloodFill(grid, visited, x, y, color);
                        foundRegion = true;
                    }
                }
            }
        }

        return true;
    }

    private static void FloodFill(int[,] grid, bool[,] visited, int startX, int startY, int color)
    {
        int size = grid.GetLength(0);
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(new Vector2Int(startX, startY));
        visited[startX, startY] = true;

        Vector2Int[] directions = {
        new Vector2Int(0, 1),
        new Vector2Int(1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0)
    };

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            foreach (var dir in directions)
            {
                int nx = current.x + dir.x;
                int ny = current.y + dir.y;

                if (nx >= 0 && ny >= 0 && nx < size && ny < size)
                {
                    if (!visited[nx, ny] && grid[nx, ny] == color)
                    {
                        visited[nx, ny] = true;
                        queue.Enqueue(new Vector2Int(nx, ny));
                    }
                }
            }
        }
    }
}