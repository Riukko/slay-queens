using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static readonly string LevelFilePath = Path.Combine(Application.streamingAssetsPath, "Levels");
    public static string LevelFileNamePrefix = "GL_";

    public GameLevel CurrentLevel;

    public List<List<int>> aled = new();

    private bool hasUnsavedChanges;

    public void SaveCurrentLevel(string levelName)
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

        if(CurrentLevel != null)
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
                LevelName = levelName,
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
        string filePath = Path.Combine(LevelFilePath, $"{LevelFileNamePrefix}{gameLevel.LevelId}.json");
        string json = JsonUtility.ToJson(gameLevel, true);
        File.WriteAllText(filePath, json);

        Debug.Log($"Level saved: {filePath}");
        CurrentLevel = gameLevel;
    }

    private void OverwriteLevel(GameLevel gameLevel)
    {
      //TODO
    }

    #region Singleton
    private static LevelManager instance = null;
    public static LevelManager Instance => instance;
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
