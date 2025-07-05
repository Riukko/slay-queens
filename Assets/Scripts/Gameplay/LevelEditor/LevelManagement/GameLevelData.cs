[System.Serializable]
public class GameLevelData
{
    public string LevelId;
    public string LevelName;
    public int[,] CellTable;
    public bool IsLevelSolvable;

    [System.NonSerialized]
    public string LastSavedFileName;

    public bool HasAlreadyBeenSaved => !string.IsNullOrEmpty(LastSavedFileName);

    public bool IsEmpty => string.IsNullOrEmpty(LevelId) || CellTable == null;

    public static GameLevelData CreateGeneric() => new GameLevelData
    {
        LevelId = null,
        LevelName = "",
        CellTable = null,
        IsLevelSolvable = false
    };
}
