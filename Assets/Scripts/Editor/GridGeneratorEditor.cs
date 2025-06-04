using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridGenerator))]
public class GridGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridGenerator gridGenerator = (GridGenerator)target;

        if (GUILayout.Button("Generate grid"))
        {
            gridGenerator.GenerateGrid();
        }

        if (GUILayout.Button("Delete grid"))
        {
            gridGenerator.ClearGrid();
        }

        if(GUILayout.Button("Generate cell outlines"))
        {
            GridHelpers.HighlightCellOutlinesInGrid();
        }

        if (GUILayout.Button("Generate grid outlines"))
        {
            GridHelpers.HighlightGridOuterLines();
        }
    }
}
