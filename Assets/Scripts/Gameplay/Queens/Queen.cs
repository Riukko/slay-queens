using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public sealed class Queen
{
    public Vector2Int Coordinates;

    private HashSet<Queen> conflicts = new();

    public HashSet<Queen> Conflicts => conflicts;

    public Queen(Vector2Int coordinates)
    {
        Coordinates = coordinates;
    }

    public event Action<bool> OnConflictsChanged;

    public void AddConflict(Queen other)
    {
        if (conflicts.Add(other))
            OnConflictsChanged?.Invoke(conflicts.Count > 0);
    }

    public void RemoveConflict(Queen other)
    {
        if (conflicts.Remove(other))
            OnConflictsChanged?.Invoke(conflicts.Count > 0);
    }
}
