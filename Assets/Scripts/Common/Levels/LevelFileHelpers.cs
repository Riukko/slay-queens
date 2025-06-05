using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

public static class LevelFileHelpers
{
    public static readonly string LevelFilePath = Path.Combine(Application.streamingAssetsPath, "Levels");
    public static string LevelFileNamePrefix = "GL_";

    public static string GetLevelFilePathFromName(string levelFileName) => Path.Combine(LevelFilePath, $"{levelFileName}.json").Replace('\\', '/');

    public static void SaveLevelFileAsNew(GameLevel gameLevel)
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
        catch (Exception e)
        {
            Debug.LogError($"Saving file {filePath} has failed with error : {e.Message}");
            return;
        }

        Debug.Log($"Level saved: {filePath}");
    }

    public static void OverwriteLevelFile(GameLevel gameLevel, string oldName = "")
    {

        //TODO : Save the actual new level

        if (!string.IsNullOrEmpty(oldName))
        {
            string oldFilePath = GetLevelFilePathFromName(oldName);
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

    public static int[,] ExtractGridDataTable(Cell[,] cellTable)
    {
        int size = cellTable.GetLength(0);
        var cells = cellTable;
        var data = new int[size, size];

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                data[x, y] = cells[x, y].CellGroup.GetColorIndexFromGroup();
            }
        }

        return data;
    }

    public static string GetValidatedLevelFileName(string levelFileName)
    {
        string name = levelFileName;
        return name.StartsWith(LevelFileNamePrefix) ? name : $"{LevelFileNamePrefix}{name}";
    }

    public static GameLevel DeserializeLevelFromFile(string levelFileName)
    {
        string levelFilePath = LevelFileHelpers.GetLevelFilePathFromName(levelFileName);
        if (!File.Exists(levelFilePath))
        {
            Debug.LogError($"Couldn't find file {levelFilePath} to load it");
            return null;
        }

        string levelJson = File.ReadAllText(levelFilePath);
        GameLevel deserializedLevel = JsonConvert.DeserializeObject<GameLevel>(levelJson);

        return deserializedLevel;
    }
}
