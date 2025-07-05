using System;
using UnityEngine;

public class LevelDataManager : Singleton<LevelDataManager>
{
    private GameLevelData _currentLevel = GameLevelData.CreateGeneric();
    public GameLevelData CurrentLevel
    {
        get => _currentLevel;
        set
        {
            OnCurrentLevelChanged?.Invoke(value);
            _currentLevel = value;
        }
    }

    public event Action<GameLevelData> OnCurrentLevelChanged;
    public event Action<bool> LevelSolvabilityUpdateEvent;

    public bool HasUnsavedChanges = true;
    private void HandleGridChanged() => HasUnsavedChanges = true;

    public void Start()
    {
        EditorCell.OnEditorCellChangedEvent += HandleGridChanged;
        LevelDetailsView.OnLevelNameChangedEvent += HandleLevelNameChanged;
    }

    public void SaveCurrentLevel()
    {
        if (!GridManager.HasInstance)
            return;

        if (string.IsNullOrEmpty(CurrentLevel.LevelName))
        {
            UIManager.Instance.GetUIElement<InformationPopup>(AccessibleUIElementTag.InformationPopup)
                .Show("Please enter a name for the level", null);
            return;
        }

        if (!HasUnsavedChanges)
        {
            UIManager.Instance.GetUIElement<InformationPopup>(AccessibleUIElementTag.InformationPopup)
            .Show("No changes to save", null);
            return;
        }

        int[,] savedLevelTable = LevelFileHelpers.ExtractGridDataTable(GridManager.Instance.CellTable);
        bool levelSolvability = GameSolver.IsSolvable(savedLevelTable);

        bool saveResult;

        CurrentLevel.LevelName = LevelFileHelpers.GetValidatedLevelFileName(CurrentLevel.LevelName);
        CurrentLevel.CellTable = savedLevelTable;
        CurrentLevel.IsLevelSolvable = levelSolvability;

        if (!CurrentLevel.IsEmpty)
        {
            saveResult = ChooseOverwriteOrSaveAsNew();
        }
        else
        {
            saveResult = LevelFileHelpers.SaveLevelFileAsNew(CurrentLevel);
        }

        HasUnsavedChanges = !saveResult;
    }

    private bool ChooseOverwriteOrSaveAsNew()
    {
        bool saveResult = false;
        UIManager.Instance
            .GetUIElement<ConfirmationPopup>(AccessibleUIElementTag.ConfirmationPopup)
            .Show(
                "Would you like to overwrite current level or save it as a new one?",
                () => saveResult = HandleLevelOverWriting(),
                () => saveResult = SaveLevelFileAsNew(),
                new PopupStyle
                {
                    ConfirmButtonColor = Color.green,
                    CancelButtonColor = Color.blue,
                    ConfirmButtonText = "Overwrite",
                    CancelButtonText = "Save As New"
                }
            );

        return saveResult;
    }

    private bool SaveLevelFileAsNew()
    {
        if (LevelFileHelpers.SaveLevelFileAsNew(CurrentLevel))
        {
            CurrentLevel.LastSavedFileName = CurrentLevel.LevelName;
            return true;
        }
        return false;
    }

    private bool HandleLevelOverWriting()
    {
        if (string.Equals(CurrentLevel.LevelName, CurrentLevel.LastSavedFileName, StringComparison.OrdinalIgnoreCase))
        {
            return LevelFileHelpers.OverwriteLevelFile(CurrentLevel);
        }
        else
        {
            bool result = LevelFileHelpers.OverwriteLevelFile(CurrentLevel, CurrentLevel.LastSavedFileName);
            if (result)
                CurrentLevel.LastSavedFileName = CurrentLevel.LevelName;
            return result;
        }
    }

    private void HandleLevelNameChanged(string levelName)
    {
        CurrentLevel.LevelName = levelName;
    }

    public void LoadLevelFromFile(string levelFileName)
    {
        GameLevelData level = LevelFileHelpers.DeserializeLevelFromFileName(levelFileName);
        if (level != null)
        {
            LoadLevelFromData(level);
        }
        else
        {
            Debug.LogError($"Couldn't deserialize the Level {levelFileName}");
        }
    }

    public void LoadLevelFromData(GameLevelData levelData)
    {
        CurrentLevel = levelData;
        CurrentLevel.LastSavedFileName = levelData.LevelName;
        GridManager.Instance.GenerateGridFromTable(levelData.CellTable);
        HasUnsavedChanges = false;
    }

    public void CreateNewLevel()
    {
        UIManager.Instance
            .GetUIElement<ConfirmationPopup>(AccessibleUIElementTag.ConfirmationPopup)
            .Show(
                "Do you want to create a new level? This will clear the current grid." + (HasUnsavedChanges ? "\n(You currently have unsaved changes)" : ""),
                () =>
                {
                    GridManager.Instance.GenerateEmptyGrid();
                    CurrentLevel = GameLevelData.CreateGeneric();
                },
                null,
                new PopupStyle
                {
                    ConfirmButtonText = "Create New",
                    CancelButtonText = "Cancel"
                }
            );
    }

    public void CheckCurrentLevelSolvability()
    {
        if (!GridManager.HasInstance)
            throw new Exception("Grid Manager is not available");

        int[,] savedLevelTable = LevelFileHelpers.ExtractGridDataTable(GridManager.Instance.CellTable);
        LevelSolvabilityUpdateEvent?.Invoke(GameSolver.IsSolvable(savedLevelTable));

        QueenManager.Instance.UpdateAllQueensConflicts();
        GridManager.Instance.RefreshAllCellsConflict();
    }
}
