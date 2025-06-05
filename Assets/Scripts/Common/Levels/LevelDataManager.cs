using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelDataManager : MonoBehaviour
{
    public static readonly string LevelFilePath = Path.Combine(Application.streamingAssetsPath, "Levels");
    public static string LevelFileNamePrefix = "GL_";

    public GameLevel CurrentLevel = null;

    public List<List<int>> aled = new();

    [SerializeField]
    private LevelDetailsView levelDetailsView;

    private bool hasUnsavedChanges;

    public void SaveCurrentLevel()
    {
        if (!GridDataManager.HasInstance)
            return;

        int gridSize = GridDataManager.Instance.GridSize;
        Cell[,] cellTable = GridDataManager.Instance.CellTable;
        int[,] savedLevelTable = new int[gridSize, gridSize];

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                savedLevelTable[x, y] = cellTable[x, y].CellGroup.GetColorIndexFromGroup();
            }
        }

        if(!CurrentLevel.IsNull)
        {
            CurrentLevel.CellTable = savedLevelTable;
            OverwriteLevel(CurrentLevel);
        }
        else
        {
            GameLevel newLevel = new GameLevel
            {
                LevelId = Guid.NewGuid().ToString(),
                CellTable = savedLevelTable,
                LevelName = levelDetailsView.LevelNameText.text,
            };
            SaveLevelAsNew(newLevel);

            CurrentLevel = newLevel;
        }
    }

    public void LoadLevel(GameLevel level)
    {
     //TODO
    }

    public void CreateNewLevel()
    {
        //TODO
    }

    private void SaveLevelAsNew(GameLevel gameLevel)
    {
        if (!Directory.Exists(LevelFilePath))
        {
            Directory.CreateDirectory(LevelFilePath);
            Debug.Log($"Directory created: {LevelFilePath}");
        }

        string filePath = Path.Combine(LevelFilePath, $"{LevelFileNamePrefix}{gameLevel.LevelName}.json");
        string json = JsonUtility.ToJson(gameLevel, true);
        //File.WriteAllText(filePath, json);

        Debug.Log($"Level saved: {filePath}");
        CurrentLevel = gameLevel;
    }

    private void OverwriteLevel(GameLevel gameLevel)
    {
      //TODO
    }

    #region Singleton
    private static LevelDataManager instance = null;
    public static LevelDataManager Instance => instance;
    public static bool HasInstance => instance != null;
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
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion
}
