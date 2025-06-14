#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(fileName = "SceneDatabase", menuName = "Game/Scene Database")]
public class SceneDatabase : ScriptableObject
{
    public SceneReference[] scenes;

#if UNITY_EDITOR
    private void OnValidate()
    {
        foreach (var scene in scenes)
        {
            scene?.UpdateSceneName();
        }
        EditorUtility.SetDirty(this);
    }
#endif
}