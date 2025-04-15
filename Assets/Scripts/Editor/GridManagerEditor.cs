using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridManager))]
public class GridManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridManager resizer = (GridManager)target;

        if (GUILayout.Button("Generate grid"))
        {
            resizer.GenerateGrid();
        }

        if (GUILayout.Button("Delete grid"))
        {
            resizer.ClearGrid();
        }
    }
}
