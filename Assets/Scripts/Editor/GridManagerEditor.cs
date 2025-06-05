using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridManager))]
public class GridManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridManager gridManager = (GridManager)target;

        if (GUILayout.Button("Generate grid"))
        {
            gridManager.GenerateGrid();
        }

        if (GUILayout.Button("Delete grid"))
        {
            gridManager.ClearGrid();
        }

        if(GUILayout.Button("Generate cell outlines"))
        {
            GridHelpers.HighlightCellOutlinesInGrid(gridManager.CellTable);
        }

        if (GUILayout.Button("Generate grid outlines"))
        {
            GridHelpers.HighlightGridOuterLines(gridManager.CellTable);
        }
    }
}
