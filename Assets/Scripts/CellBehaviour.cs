using NUnit.Framework;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum CellStatus
{
    IDLE,
    EXCLUDED,
    QUEEN
}

public class CellBehaviour : MonoBehaviour
{

    public Cell cell;

    public CellStatus cellStatus = CellStatus.IDLE;

    public Queen Queen
    {
        get => _queen;
        set
        {
            if(value == null)
            {
                queenSprite.enabled = false;
                GameManager.Instance.RemoveQueen(_queen);
                _queen.OnConflictsChanged -= UpdateConflictStatus;
                IsCellConflicted = false;
            }
            else
            {
                queenSprite.enabled = true;
                value.OnConflictsChanged += UpdateConflictStatus;
                GameManager.Instance.AddQueen(value);
            }
            _queen = value;
        }
    }
    private Queen _queen = null;

    public bool IsCellConflicted
    {
        get => _isCellConflicted;
        set
        {
            ErrorOverlay.SetActive(value);
            _isCellConflicted = value;
        }
    }
    private bool _isCellConflicted = false;
    private void UpdateConflictStatus(bool isConflict) => IsCellConflicted = isConflict;

    public Image queenSprite;
    public GameObject ErrorOverlay;

    private void Start()
    {
        queenSprite.enabled = false;
        ErrorOverlay.SetActive(false);
    }

    public void InitializeCell(int posX, int posY)
    {
        Cell cell = new(posX, posY);
        cell.cellGroup = (CellGroup)UnityEngine.Random.Range(0, Enum.GetValues(typeof(CellGroup)).Length);

        if(GameManager.Instance.CellGroupColorPalette != null)
        {
            GetComponent<Image>().color = GameManager.Instance.CellGroupColorPalette.GetRandomColor();
        }

        this.cell = cell;
    }

    public void OnCellClick()
    {
        int nextStatus = ((int)cellStatus + 1) % Enum.GetValues(typeof(CellStatus)).Length;

        cellStatus = (CellStatus)nextStatus;
        ApplyStatus(cellStatus);

    }

    private void ApplyStatus(CellStatus status)
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
                Queen = new Queen(cell.PosX, cell.PosY);
                break;
        }
    }

}

public record Cell(int PosX, int PosY)
{
    internal CellGroup cellGroup;
}
