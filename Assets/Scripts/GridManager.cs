using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;


#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
[RequireComponent(typeof(GridLayoutGroup), typeof(RectTransform))]
public class GridManager : MonoBehaviour
{
    [Range(1, 15)]
    public int gridSize;

    public bool autoResize;
    public GameObject cellPrefab;

    private GridLayoutGroup grid;
    private RectTransform rectTransform;


    public void Start()
    {
        grid = GetComponent<GridLayoutGroup>();
        rectTransform = GetComponent<RectTransform>();

        GenerateGrid();
    }

    private void OnValidate()
    {
        if (autoResize)
        {
            GridHelpers.ResizeGrid(grid, rectTransform, gridSize);
        }
        GameManager.Instance.GridTable = new GameObject[gridSize, gridSize];
    }

    public void GenerateGrid()
    {
        if (cellPrefab == null)
        {
            Debug.LogWarning("GridResizer needs a button prefab");
            return;
        }

        ClearGrid();


        grid.constraintCount = gridSize;

        for (int x = 0; x < gridSize; x++)
        {
            for(int y = 0; y <gridSize; y++)
            {
                InstantiateCell(x, y);
            }
        }

        GridHelpers.ResizeGrid(grid, rectTransform, gridSize);
    }

    public void InstantiateCell(int posX, int posY)
    {
        GameObject cellGO = Instantiate(cellPrefab, transform);
        cellGO.GetComponent<CellBehaviour>().InitializeCell(posX, posY);
        
        GameManager.Instance.GridTable[posX, posY] = cellGO;
        //cellGO.GetComponentInChildren<TextMeshProUGUI>().text = $"{posX},{posY}";
    }

    public void ClearGrid()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            DestroyImmediate(child.gameObject);
        }
        GameManager.Instance.GridTable = new GameObject[gridSize, gridSize];
    }
}
