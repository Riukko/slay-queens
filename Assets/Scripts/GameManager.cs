using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GameManager : MonoBehaviour
{
    public GameObject[,] GridTable;

    public CellGroupColorPalette CellGroupColorPalette;

    public Dictionary<Vector2, Queen> Queens = new();

    public void AddQueen(Cell cell)
    {
        Debug.Log(cell);
        Queens.Add(new Vector2(cell.PosX, cell.PosY), new Queen(cell));
    }

    public void RemoveQueen(Cell cell)
    {
        Queens.Remove(new Vector2(cell.PosX, cell.PosY));
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