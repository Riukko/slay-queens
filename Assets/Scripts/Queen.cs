using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public sealed class Queen
{
    public Vector2Int Coordinates;
    private HashSet<Queen> Conflicts = new();

    public Queen(Vector2Int coordinates)
    {
        Coordinates = coordinates;
    }

    public event Action<bool> OnConflictsChanged;

    public void AddConflict(Queen other)
    {
        if (Conflicts.Add(other))
            OnConflictsChanged?.Invoke(Conflicts.Count > 0);
    }

    public void RemoveConflict(Queen other)
    {
        if (Conflicts.Remove(other))
            OnConflictsChanged?.Invoke(Conflicts.Count > 0);
    }
}
