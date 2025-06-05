using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelDataManager : MonoBehaviour
{
    public static readonly string LevelFilePath = Path.Combine(Application.streamingAssetsPath, "Levels").Replace('/', '\\');
    public static string LevelFileNamePrefix = "GL_";

    public GameLevel CurrentLevel = null;

    public List<List<int>> aled = new();

    [SerializeField]
    private LevelDetailsView levelDetailsView;

    private bool hasUnsavedChanges;
    public static string GetLevelFilePathFromName(string levelFileName) => Path.Combine(LevelFilePath, $"{levelFileName}.json");

    public void SaveCurrentLevel()
    {
        if (!GridManager.HasInstance)
            return;

        int gridSize = GridManager.Instance.GridSize;
        Cell[,] cellTable = GridManager.Instance.CellTable;
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
                LevelName = $"{LevelFileNamePrefix}{levelDetailsView.LevelNameText.text}",
            };
            SaveLevelAsNew(newLevel);

            CurrentLevel = newLevel;
        }
    }

    public void LoadLevelFromFile(string levelFileName)
    {
        string levelFilePath = GetLevelFilePathFromName(levelFileName);
        if (!File.Exists(levelFilePath))
        {
            Debug.LogError($"Couldn't find file {levelFilePath} to load it");
            return;
        }

        string levelJson = File.ReadAllText(levelFilePath);
        GameLevel deserializedLevel = JsonConvert.DeserializeObject<GameLevel>(levelJson);

        CurrentLevel = deserializedLevel;
        GridManager.Instance.GenerateGridFromTable(deserializedLevel.CellTable);
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

        string filePath = GetLevelFilePathFromName(gameLevel.LevelName);
        string json = JsonConvert.SerializeObject(gameLevel);
        try
        {
            File.WriteAllText(filePath, json);
        }
        catch(Exception e)
        {
            Debug.LogError($"Saving file {filePath} has failed with error : {e.Message}");
            return;
        }

        Debug.Log($"Level saved: {filePath}");
        CurrentLevel = gameLevel;
    }

    private void OverwriteLevel(GameLevel gameLevel, string oldName = "")
    {


        if (!string.IsNullOrEmpty(oldName))
        {
            string oldFilePath = Path.Combine(GetLevelFilePathFromName(oldName));
            if (File.Exists(oldFilePath))
            {
                File.Delete(oldFilePath);
            }
            else
            {
                Debug.LogError($"Couldn't find file {oldFilePath}, couldn't delete it");
            }
        }
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
