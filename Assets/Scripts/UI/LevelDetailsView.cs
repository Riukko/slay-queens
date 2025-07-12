using System;
using TMPro;
using UnityEngine;

public class LevelDetailsView : MonoBehaviour
{
    [SerializeField] private TMP_InputField levelNameInputField;
    [SerializeField] private TextMeshProUGUI levelIdText;
    [SerializeField] private TextMeshProUGUI solvabilityText;

    public static event Action<string> OnLevelNameChangedEvent;

    private void Awake()
    {
        levelNameInputField.onEndEdit.AddListener(HandleLevelNameInput);
    }

    private void Start()
    {
        LevelEditorDataManager.Instance.OnCurrentLevelChanged += UpdateLevelDetails;
        LevelEditorDataManager.Instance.LevelSolvabilityUpdateEvent += SetSolvability;
    }

    private void HandleLevelNameInput(string newName)
    {
        OnLevelNameChangedEvent?.Invoke(newName);
    }

    private void UpdateLevelDetails(GameLevelData gameLevelData)
    {
        levelIdText.text = gameLevelData?.LevelId ?? "Level is unsaved";
        levelNameInputField.text = gameLevelData?.LevelName ?? "";
        SetSolvability(gameLevelData.IsLevelSolvable);
    }

    public void SetSolvability(bool solvable)
    {
        solvabilityText.text = solvable ? "Level is solvable" : "Level is not solvable";
        solvabilityText.color = solvable ? Color.green : Color.red;
        solvabilityText.gameObject.SetActive(true);
    }
}
