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

public class CellBehaviour : MonoBehaviour
{

    public Cell cell;

    public CellStatus cellStatus = CellStatus.IDLE;

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
            GetComponent<Image>().color = GameManager.Instance.CellGroupColorPalette.groupColors[UnityEngine.Random.Range(0, GameManager.Instance.CellGroupColorPalette.groupColors.Count - 1)].color;
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
                queenSprite.enabled = false;
                GameManager.Instance.RemoveQueen(cell);
                break;
            case CellStatus.EXCLUDED:
                cellText.text = "X";
                break;
            case CellStatus.QUEEN:
                cellText.text = "";
                queenSprite.enabled = true;
                GameManager.Instance.AddQueen(cell);
                break;
        }
    }
}

public record Cell(int PosX, int PosY)
{
    internal CellGroup cellGroup;
}
