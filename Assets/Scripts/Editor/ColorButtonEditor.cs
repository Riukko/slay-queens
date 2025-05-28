using UnityEditor;

[CustomEditor(typeof(LevelEditorButton))]
public class ColorButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty colorStatusProp = serializedObject.FindProperty("ClickingAction");
        SerializedProperty colorProp = serializedObject.FindProperty("AppliedColor");

        EditorGUILayout.PropertyField(colorStatusProp);

        if ((ClickActionStatus)colorStatusProp.enumValueIndex == ClickActionStatus.COLOR)
        {
            EditorGUILayout.PropertyField(colorProp);
        }

        serializedObject.ApplyModifiedProperties();
    }
}