[System.Serializable]
public class GameLevelData
{
    public string LevelId;
    public string LevelName;
    public int[,] CellTable;

    public bool IsEmpty => string.IsNullOrEmpty(LevelId) || CellTable == null;
}
