using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum CellStatus
{
    IDLE,
    EXCLUDED,
    QUEEN
}

public abstract class Cell : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
    #region Public Variables
    public Vector2Int Coordinates;

    public CellStatus CellStatus
    {
        get => _cellStatus;
        set
        {
            switch (value)
            {
                case CellStatus.IDLE:
                    CellText.text = "";
                    Queen = null;
                    break;
                case CellStatus.EXCLUDED:
                    CellText.text = "X";
                    break;
                case CellStatus.QUEEN:
                    CellText.text = "";
                    Queen = new Queen(Coordinates);
                    break;
            }

            _cellStatus = value;
        }
    }


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


    public bool IsCellConflicted
    {
        get => _isCellConflicted;
        set
        {
            ErrorOverlay.SetActive(value);
            _isCellConflicted = value;
        }
    }

    public CellOutlines CellOutlines;

    public CellColorGroup CellGroup = CellColorGroup.WHITE;
    #endregion

    #region Protected Variables

    protected TextMeshProUGUI CellText;
    protected Image CellImage;

    [SerializeField]
    protected Image queenSprite;

    [SerializeField]
    protected GameObject ErrorOverlay;
    #endregion

    #region Private variables

    private Queen _queen = null;
    private CellStatus _cellStatus = CellStatus.IDLE;

    #endregion


    private bool _isCellConflicted = false;
    protected void UpdateConflictStatus(bool isConflict) => IsCellConflicted = isConflict;

    public abstract void OnCellClick();
    public abstract void OnCellHoldClick();

    public void InitializeCell(Vector2Int coordinates, CellColorGroup colorGroup)
    {
        Coordinates = coordinates;
        CellGroup = colorGroup;
        CellText = GetComponentInChildren<TextMeshProUGUI>();
        CellImage = GetComponent<Image>();
        ApplyColor();
    }

    public void ApplyColor()
    {
        CellImage.color = CellGroupColorPalette.GetColor(CellGroup);
    }

    #region Pointer Events
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnCellClick();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0))
        {
            OnCellHoldClick();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            GridHelpers.HighlightCellOutlinesInGrid(GridManager.Instance.CellTable);
        }
    }
    #endregion
}
