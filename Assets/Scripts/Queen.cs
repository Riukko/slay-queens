using System;
using System.Collections.Generic;

[System.Serializable]
public sealed class Queen
{
    public int posX;
    public int posY;
    private HashSet<Queen> Conflicts = new();

    public Queen(int posX, int posY)
    {
        this.posX = posX;
        this.posY = posY;
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
