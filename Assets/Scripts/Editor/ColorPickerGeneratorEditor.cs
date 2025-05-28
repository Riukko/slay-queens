using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ColorPickerGenerator))]
public class ColorPickerGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ColorPickerGenerator generator = (ColorPickerGenerator)target;

        if (GUILayout.Button("Generate colors"))
        {
            generator.ClearColors();
            generator.InitializeColorPicker();
        }
    }
}
