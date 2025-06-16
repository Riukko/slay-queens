using System;
using UnityEngine;

public class LevelDataManager : Singleton<LevelDataManager>
{
    private GameLevelData _currentLevel = null;
    public GameLevelData CurrentLevel
    {
        get => _currentLevel;
        set
        {
            if (value != null)
            {
                levelDetailsView.LevelIdText.text = value.LevelId;
                levelDetailsView.LevelNameInputField.text = value.LevelName;
            }

            _currentLevel = value;
        }
    }

    [SerializeField]
    private LevelDetailsView levelDetailsView;

    public void SaveCurrentLevel()
    {
        if (!GridManager.HasInstance)
            return;

        int[,] savedLevelTable = LevelFileHelpers.ExtractGridDataTable(GridManager.Instance.CellTable); ;
        string newLevelName = LevelFileHelpers.GetValidatedLevelFileName(levelDetailsView.LevelNameInputField.text);

        if (!CurrentLevel.IsEmpty)
        {
            string oldLevelName = CurrentLevel.LevelName;

            CurrentLevel.CellTable = savedLevelTable;
            CurrentLevel.LevelName = newLevelName;

            if (string.Equals(oldLevelName, newLevelName, StringComparison.OrdinalIgnoreCase))
                LevelFileHelpers.OverwriteLevelFile(CurrentLevel);
            else
                LevelFileHelpers.OverwriteLevelFile(CurrentLevel, oldLevelName);
        }
        else
        {
            var newLevel = new GameLevelData
            {
                LevelId = Guid.NewGuid().ToString(),
                CellTable = savedLevelTable,
                LevelName = newLevelName,
            };

            LevelFileHelpers.SaveLevelFileAsNew(newLevel);
            CurrentLevel = newLevel;
        }
    }

    public void LoadLevelFromFile(string levelFileName)
    {
        GameLevelData level = LevelFileHelpers.DeserializeLevelFromFileName(levelFileName);
        if (level != null)
        {
            CurrentLevel = level;
            GridManager.Instance.GenerateGridFromTable(level.CellTable);
        }
        else
        {
            Debug.LogError($"Couldn't deserialize the Level {levelFileName}");
        }
    }

    public void CreateNewLevel()
    {
        //TODO
    }
}
