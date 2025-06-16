using System.Collections.Generic;

public class QueenManager : Singleton<QueenManager>
{
    public List<Queen> Queens = new();

    public void AddQueen(Queen queenToAdd)
    {
        Queens.Add(queenToAdd);

        UpdateQueenConflicts(queenToAdd);
    }

    public void RemoveQueen(Queen queenToRemove)
    {
        Queens.Remove(queenToRemove);

        foreach (Queen queen in Queens)
        {
            queen.RemoveConflict(queenToRemove);
        }

        GridManager.Instance.CellTable[queenToRemove.Coordinates.x, queenToRemove.Coordinates.y].IsCellConflicted = false;
    }

    public void UpdateQueenConflicts(Queen queenToCheck)
    {
        foreach (Queen queen in Queens)
        {
            if (queen == queenToCheck) continue;

            if (queen.Coordinates.x == queenToCheck.Coordinates.x
                || queen.Coordinates.y == queenToCheck.Coordinates.y
                || GridHelpers.AreDirectDiagonalNeighbors(queen.Coordinates, queenToCheck.Coordinates, GridManager.Instance.GridSize))
            {
                queenToCheck.AddConflict(queen);
                queen.AddConflict(queenToCheck);
            }
        }
    }
}
