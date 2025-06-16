using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectableLevel : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI levelNameText;

    [SerializeField]
    Image levelPreviewImage;

    private GameLevelData levelData;

    public string LevelId => levelData.LevelId;

    public event Action<GameLevelData> OnLevelSelectedEvent;

    public void OnLevelSelected() => OnLevelSelectedEvent?.Invoke(levelData);

    public void UpdateLevelText(string text) => levelNameText.text = text;
    public void UpdateLevelPreviewImage(Image img) => levelPreviewImage = img;

    public void InitializeLevelFromData(GameLevelData levelData)
    {
        levelNameText.text = levelData.LevelName;
        this.levelData = levelData;
    }
}
