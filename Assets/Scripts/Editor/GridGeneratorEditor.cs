using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridGenerator))]
public class GridGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridGenerator resizer = (GridGenerator)target;

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
