using System;
using System.Threading.Tasks;
using UnityEngine;

public class LevelEditorDataManager : Singleton<LevelEditorDataManager>
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

    public async void SaveCurrentLevel()
    {
        if (!GridManager.HasInstance)
            return;

        if (string.IsNullOrEmpty(CurrentLevel.LevelName))
        {
            UIManager.Instance.ShowInfo("Please enter a name for the level");
            return;
        }

        if (!HasUnsavedChanges)
        {
            UIManager.Instance.ShowInfo("No changes to save");
            return;
        }

        int[,] savedLevelTable = LevelFileHelpers.ExtractGridDataTable(GridManager.Instance.CellTable);
        bool levelSolvability = GameSolver.IsSolvable(savedLevelTable);

        bool saveResult;

        CurrentLevel.LevelName = LevelFileHelpers.GetValidatedLevelFileName(CurrentLevel.LevelName);
        CurrentLevel.CellTable = savedLevelTable;
        CurrentLevel.IsLevelSolvable = levelSolvability;

        try
        {
            saveResult = CurrentLevel.IsEmpty
                ? SaveLevelFileAsNew()
                : await ChooseOverwriteOrSaveAsNew();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error while saving level: {ex.Message}");
            UIManager.Instance.ShowInfo("An unexpected error occurred while saving.");
            return;
        }

        UIManager.Instance
            .GetUIElement<InformationPopup>(AccessibleUIElementTag.InformationPopup)
            .Show(saveResult ? $"Level '{CurrentLevel.LevelName}' saved successfully" : $"Unable to save '{CurrentLevel.LevelName}", null);

        HasUnsavedChanges = !saveResult;
    }

    private async Task<bool> ChooseOverwriteOrSaveAsNew()
    {
        bool saveResult = false;
        bool overwrite = await UIManager.Instance
            .GetUIElement<ConfirmationPopup>(AccessibleUIElementTag.ConfirmationPopup)
            .ShowAsync(
                "Would you like to overwrite current level or save it as a new one?",
                new PopupStyle
                {
                    ConfirmButtonColor = Color.green,
                    CancelButtonColor = Color.blue,
                    ConfirmButtonText = "Overwrite",
                    CancelButtonText = "Save As New"
                }
            );

        if (overwrite)
        {
            saveResult = TryOverwriteLevel();
        }
        else
        {
            saveResult = SaveLevelFileAsNew();
        }

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

    private bool TryOverwriteLevel()
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
    public void DeleteCurrentLevel()
    {
        UIManager.Instance
            .GetUIElement<ConfirmationPopup>(AccessibleUIElementTag.ConfirmationPopup)
            .Show(
                "Are you sure you want to delete the current level? This action cannot be undone.",
                () =>
                {
                    if (LevelFileHelpers.TryDeleteLevel(CurrentLevel.LevelName))
                    {
                        CurrentLevel = GameLevelData.CreateGeneric();
                        GridManager.Instance.GenerateEmptyGrid();
                        HasUnsavedChanges = false;

                        UIManager.Instance
                            .GetUIElement<InformationPopup>(AccessibleUIElementTag.InformationPopup)
                            .Show("Level deleted successfully", null);
                    }
                    else
                    {
                        Debug.LogError("Failed to delete the level file.");
                    }
                },
                null,
                new PopupStyle
                {
                    ConfirmButtonText = "Delete",
                    CancelButtonText = "Cancel"
                }
            );
    }
}
