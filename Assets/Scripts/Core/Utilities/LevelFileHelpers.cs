using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class LevelFileHelpers
{
    public static readonly string LevelsFilePath = Path.Combine(Application.streamingAssetsPath, "Levels");
    public static string LevelFileNamePrefix = "GL_";

    public static string GetLevelFilePathFromName(string levelFileName) => Path.Combine(LevelsFilePath, $"{levelFileName}.json").Replace('\\', '/');

    public static bool SaveLevelFile(GameLevelData gameLevel)
    {
        if (!Directory.Exists(LevelsFilePath))
        {
            Directory.CreateDirectory(LevelsFilePath);
            Debug.Log($"Directory created: {LevelsFilePath}");
        }

        string filePath = GetLevelFilePathFromName(gameLevel.LevelName);
        string json = JsonConvert.SerializeObject(gameLevel);

        try
        {
            File.WriteAllText(filePath, json);
        }
        catch (Exception e)
        {
            Debug.LogError($"Saving file {filePath} failed with error: {e.Message}");
            return false;
        }

        Debug.Log($"Level saved: {filePath}");

        return true;
    }

    public static bool SaveLevelFileAsNew(GameLevelData gameLevel)
    {
        gameLevel.LevelId = Guid.NewGuid().ToString();
        return SaveLevelFile(gameLevel);
    }

    public static bool OverwriteLevelFile(GameLevelData gameLevel, string oldName = "")
    {
        bool saveResult = SaveLevelFile(gameLevel);

        if (!string.IsNullOrEmpty(oldName) && saveResult)
        {
            TryDeleteLevel(oldName);
        }

        return saveResult;
    }

    public static bool TryDeleteLevel(string levelFileName)
    {
        string levelFilePath = GetLevelFilePathFromName(levelFileName);
        if (File.Exists(levelFilePath))
        {

            try
            {
                File.Delete(levelFilePath);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Couldn't delete file {levelFilePath}, because of exception : {ex.Message}");
                return false;
            }

            //Also delete .meta file
            string metaFilePath = levelFilePath + ".meta";
            if (File.Exists(metaFilePath))
            {
                try
                {
                    File.Delete(metaFilePath);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Couldn't delete file meta {metaFilePath}, because of exception : {ex.Message}");
                    return false;
                }

                Debug.Log($"Deleted level file and meta: {levelFilePath}");
                return true;
            }
            else
            {
                Debug.LogError($"Couldn't find file meta file {levelFilePath}, couldn't delete it");
                return true;
            }
        }
        else
        {
            Debug.LogError($"Couldn't find file {levelFileName}, couldn't delete it");
            return false;
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

    public static GameLevelData DeserializeLevelFromFileName(string levelFileName)
    {
        string levelFilePath = GetLevelFilePathFromName(levelFileName);
        if (!File.Exists(levelFilePath))
        {
            Debug.LogError($"Couldn't find file {levelFilePath} to load it");
            return null;
        }

        try
        {
            string levelJson = File.ReadAllText(levelFilePath);
            return JsonConvert.DeserializeObject<GameLevelData>(levelJson); ;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to deserialize level: {ex.Message}");
            return null;
        }
    }

    public static GameLevelData DeserializeLevelFromPath(string levelFilePath)
    {
        if (!File.Exists(levelFilePath))
        {
            Debug.LogError($"Couldn't find file {levelFilePath} to load it");
            return null;
        }

        try
        {
            string levelJson = File.ReadAllText(levelFilePath);
            return JsonConvert.DeserializeObject<GameLevelData>(levelJson); ;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to deserialize level {levelFilePath}: {ex.Message}");
            return null;
        }
    }

    public static List<GameLevelData> LoadAllFoundLevels(string overridePath = null)
    {
        List<GameLevelData> foundGameLevels = new();
        foreach (string filePath in Directory.GetFiles(overridePath == null ? LevelsFilePath : overridePath, "*.json"))
        {
            try
            {
                foundGameLevels.Add(DeserializeLevelFromPath(filePath));
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Error when trying to load {filePath} : {ex.Message}");
                continue;
            }
        }
        return foundGameLevels;
    }
}
