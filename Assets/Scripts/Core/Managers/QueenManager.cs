using System;
using System.Collections.Generic;

public class QueenManager : Singleton<QueenManager>
{
    public List<Queen> Queens = new();

    public static event Action<Queen> OnQueenAddedEvent;
    public static event Action<Queen> OnQueenRemovedEvent;

    public void AddQueen(Queen queenToAdd, bool updateConflicts = true)
    {
        Queens.Add(queenToAdd);

        if (updateConflicts)
            UpdateQueenConflicts(queenToAdd);

        OnQueenAddedEvent?.Invoke(queenToAdd);
    }

    public void RemoveQueen(Queen queenToRemove, bool updateConflicts = true)
    {
        Queens.Remove(queenToRemove);

        if (updateConflicts)
        {
            foreach (Queen queen in Queens)
            {
                queen.RemoveConflict(queenToRemove);
            }
        }

        OnQueenRemovedEvent?.Invoke(queenToRemove);
    }

    public void UpdateQueenConflicts(Queen queenToCheck)
    {
        foreach (Queen queen in Queens)
        {
            if (queen == queenToCheck) continue;

            if (GridHelpers.HaveSameColor(queen.Coordinates, queenToCheck.Coordinates))
            {
                queenToCheck.AddOrUpdateConflict(queen, ConflictType.Color);
                queen.AddOrUpdateConflict(queenToCheck, ConflictType.Color);
            }
            else if (GridHelpers.AreOnTheSameRow(queen.Coordinates, queenToCheck.Coordinates))
            {
                queenToCheck.AddOrUpdateConflict(queen, ConflictType.Row);
                queen.AddOrUpdateConflict(queenToCheck, ConflictType.Row);
            }
            else if(GridHelpers.AreOnTheSameColumn(queen.Coordinates, queenToCheck.Coordinates))
            {
                queenToCheck.AddOrUpdateConflict(queen, ConflictType.Column);
                queen.AddOrUpdateConflict(queenToCheck, ConflictType.Column);
            }
            else if (GridHelpers.AreDirectDiagonalNeighbors(queen.Coordinates, queenToCheck.Coordinates, GridManager.Instance.GridSize))
            {
                queenToCheck.AddOrUpdateConflict(queen, ConflictType.Color);
                queen.AddOrUpdateConflict(queenToCheck, ConflictType.Color);
            }
        }
    }

    public void UpdateAllQueensConflicts()
    {
        foreach (Queen queen in Queens)
        {
            UpdateQueenConflicts(queen);
        }
    }
}
