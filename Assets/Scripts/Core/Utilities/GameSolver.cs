using System;
using System.Collections.Generic;
using UnityEngine;

public static class GameSolver
{
    public static bool IsSolvable(int[,] grid)
    {
        int size = grid.GetLength(0);
        List<Vector2Int> placedQueens = new List<Vector2Int>();
        return SolveRecursive(grid, size, 0, placedQueens);
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
}