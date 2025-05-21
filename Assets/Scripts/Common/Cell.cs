using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum CellStatus
{
    IDLE,
    EXCLUDED,
    QUEEN
}

public class Cell : MonoBehaviour
{
    public Vector2Int Coordinates;

    public CellStatus CellStatus = CellStatus.IDLE;

    public CellGroup CellGroup = CellGroup.WHITE;

    public Queen Queen
    {
        get => _queen;
        set
        {
            if (value == null)
            {
                queenSprite.enabled = false;
                QueenManager.Instance.RemoveQueen(_queen);
                _queen.OnConflictsChanged -= UpdateConflictStatus;
                IsCellConflicted = false;
            }
            else
            {
                queenSprite.enabled = true;
                value.OnConflictsChanged += UpdateConflictStatus;
                QueenManager.Instance.AddQueen(value);
            }
            _queen = value;
        }
    }
    protected Queen _queen = null;

    public bool IsCellConflicted
    {
        get => _isCellConflicted;
        set
        {
            ErrorOverlay.SetActive(value);
            _isCellConflicted = value;
        }
    }
    protected bool _isCellConflicted = false;
    protected void UpdateConflictStatus(bool isConflict) => IsCellConflicted = isConflict;

    public Image queenSprite;
    public GameObject ErrorOverlay;

    private void Start()
    {
        queenSprite.enabled = false;
        ErrorOverlay.SetActive(false);
    }

    public void InitializeCell(Vector2Int coordinates)
    {
        Coordinates = coordinates;
        CellGroup = (CellGroup)UnityEngine.Random.Range(0, Enum.GetValues(typeof(CellGroup)).Length);

        if (GridGenerator.CellGroupColorPalette != null)
        {
            GetComponent<Image>().color = GridGenerator.CellGroupColorPalette.GetRandomColor();
        }
    }

    public void OnCellClick()
    {
        int nextStatus = ((int)CellStatus + 1) % Enum.GetValues(typeof(CellStatus)).Length;

        CellStatus = (CellStatus)nextStatus;
        ApplyStatus(CellStatus);

    }

    protected void ApplyStatus(CellStatus status)
    {
        TextMeshProUGUI cellText = GetComponentInChildren<TextMeshProUGUI>();
        switch (status)
        {
            case CellStatus.IDLE:
                cellText.text = "";
                Queen = null;
                break;
            case CellStatus.EXCLUDED:
                cellText.text = "X";
                break;
            case CellStatus.QUEEN:
                cellText.text = "";
                Queen = new Queen(Coordinates);
                break;
        }
    }

}
