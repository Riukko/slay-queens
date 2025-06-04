using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CellOutlines))]
public class CellOutlinesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CellOutlines cellOutlines = (CellOutlines)target;

        if (GUILayout.Button("Apply outline thickness"))
        {
            cellOutlines.ApplyOutlineThickness(cellOutlines.OutlineThickness);
        }

        if(GUILayout.Button("Set all visible"))
        {
            cellOutlines.SetAllVisibility(true);
        }

        if (GUILayout.Button("Set all invisible"))
        {
            cellOutlines.SetAllVisibility(false);
        }
    }
}
