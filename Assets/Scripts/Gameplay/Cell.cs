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
            if (_queen != null)
                OnQueenRemoved(_queen);

            if (value != null)
                OnQueenAssigned(value);

            _queen = value;
        }
    }

    public bool HasQueen => Queen != null;

    protected virtual void OnQueenAssigned(Queen newQueen)
    {
        queenSprite.enabled = true;
        newQueen.OnConflictsChanged += UpdateConflictStatus;
        QueenManager.Instance.AddQueen(newQueen);
    }

    protected virtual void OnQueenRemoved(Queen oldQueen)
    {
        queenSprite.enabled = false;
        QueenManager.Instance.RemoveQueen(oldQueen);
        oldQueen.OnConflictsChanged -= UpdateConflictStatus;
        IsCellConflicted = false;
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

    private CellColorGroup _cellGroup = CellColorGroup.WHITE;

    public CellColorGroup CellGroup
    {
        get => _cellGroup;
        set
        {
            _cellGroup = value;
            ApplyColor();
        }
    }

    private void ApplyColor()
    {
        CellImage.color = CellGroupColorPalette.GetColor(CellGroup);
    }

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
        CellText = GetComponentInChildren<TextMeshProUGUI>();
        CellImage = GetComponent<Image>();
        CellGroup = colorGroup;
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
