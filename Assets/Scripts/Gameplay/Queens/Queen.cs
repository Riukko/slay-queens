using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public sealed class Queen
{
    public Vector2Int Coordinates;

    [SerializeField]
    private List<Conflict> conflicts = new ();
    public List<Conflict> Conflicts => conflicts;

    public Queen(Vector2Int coordinates)
    {
        Coordinates = coordinates;
    }

    public event Action<bool> OnConflictsChanged;

    public void AddOrUpdateConflict(Queen other, ConflictType type)
    {
        var existing = conflicts.Find(c => c.OtherQueen == other);

        if (existing != null)
        {
            if (type < existing.Type)
            {
                conflicts.Remove(existing);
                conflicts.Add(new Conflict(other, type));
                OnConflictsChanged?.Invoke(true);
            }
        }
        else
        {
            conflicts.Add(new Conflict(other, type));
            OnConflictsChanged?.Invoke(true);
        }
    }

    public void RemoveConflict(Queen other)
    {
        conflicts.RemoveAll(c => c.OtherQueen == other);
        OnConflictsChanged?.Invoke(conflicts.Count > 0);
    }
}
