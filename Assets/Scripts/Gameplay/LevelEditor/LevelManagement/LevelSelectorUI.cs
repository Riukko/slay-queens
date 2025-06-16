using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelSelectorUI : MonoBehaviour
{

    [SerializeField]
    private Transform levelListGridContainer;

    [SerializeField]
    private GameObject selectableLevelPrefab;

    [SerializeField]
    private GameObject confirmationTab;

    private List<GameObject> selectableLevelList = new();

    private void OnEnable()
    {
        PopulateLevelList();
    }

    private void PopulateLevelList(List<GameLevelData> levelDatalist = null)
    {
        if (levelDatalist == null)
        {
            levelDatalist = LevelFileHelpers.LoadAllFoundLevels();
        }

        ClearSelectableLevelList();

        foreach (GameLevelData levelData in levelDatalist)
        {
            selectableLevelList.Add(CreateSelectableLevelObject(levelData));
        }
    }

    public GameObject CreateSelectableLevelObject(GameLevelData levelData)
    {
        GameObject selectableLevelGO = Instantiate(selectableLevelPrefab, levelListGridContainer);
        SelectableLevel selectableLevel = selectableLevelGO.GetComponent<SelectableLevel>();

        selectableLevel.InitializeLevelFromData(levelData);
        selectableLevel.OnLevelSelectedEvent += HandleLevelSelected;
        return selectableLevelGO;
    }

    public void RemoveSelectableLevelObjectById(string levelId)
    {
        GameObject selectableLevelGO = selectableLevelList.FirstOrDefault(g => g.GetComponent<SelectableLevel>().LevelId == levelId);
        if (selectableLevelGO != null)
        {
            selectableLevelGO.GetComponent<SelectableLevel>().OnLevelSelectedEvent -= HandleLevelSelected;
            selectableLevelList.Remove(selectableLevelGO);
            Destroy(selectableLevelGO);
        }
    }

    private void ClearSelectableLevelList()
    {
        foreach (GameObject levelObject in selectableLevelList)
        {
            Destroy(levelObject);
        }

        selectableLevelList = new();
    }

    private void HandleLevelSelected(GameLevelData levelData)
    {
        Debug.Log($"LEVEL {levelData.LevelName} SELECTED");
    }

    //public Texture2D GeneratePreviewTexture(GameLevel level, CellGroupColorPalette palette, int pixelPerCell = 4)
    //{
    //    int width = level.CellTable.GetLength(0);
    //    int height = level.CellTable.GetLength(1);

    //    Texture2D tex = new Texture2D(width * pixelPerCell, height * pixelPerCell);
    //    tex.filterMode = FilterMode.Point;

    //    for (int x = 0; x < width; x++)
    //    {
    //        for (int y = 0; y < height; y++)
    //        {
    //            Color color = palette.GetColorGroupAtIndex(level.CellTable[x, y]);

    //            for (int dx = 0; dx < pixelPerCell; dx++)
    //            {
    //                for (int dy = 0; dy < pixelPerCell; dy++)
    //                {
    //                    tex.SetPixel(x * pixelPerCell + dx, y * pixelPerCell + dy, color);
    //                }
    //            }
    //        }
    //    }

    //    tex.Apply();
    //    return tex;
    //}

}

