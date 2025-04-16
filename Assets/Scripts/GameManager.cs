using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Net.Security;
using Mono.Cecil.Cil;

[ExecuteAlways]
public class GameManager : MonoBehaviour
{
    [SerializeField]
    public Cell[,] CellTable;

    public CellGroupColorPalette CellGroupColorPalette;

    public List<Queen> Queens = new();

    public GridManager GridManager;


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

        CellTable[queenToRemove.Coordinates.x, queenToRemove.Coordinates.y].IsCellConflicted = false;
    }

    public void UpdateQueenConflicts(Queen queenToCheck)
    {
        foreach (Queen queen in Queens)
        {
            if (queen == queenToCheck) continue;

            if (queen.Coordinates.x == queenToCheck.Coordinates.x 
                || queen.Coordinates.y == queenToCheck.Coordinates.y 
                || GridHelpers.AreDirectDiagonalNeighbors(queen.Coordinates, queenToCheck.Coordinates, GridManager.gridSize))
            {
                queenToCheck.AddConflict(queen);
                queen.AddConflict(queenToCheck);
            }
        }
    }

    #region Singleton
    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindFirstObjectByType<GameManager>();
            return instance;
        }
    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
    }
    #endregion
}