using UnityEngine;

[System.Serializable]
public class SceneReference
{
    public string sceneName;

    public Object sceneAsset;

    public SceneType sceneType;

#if UNITY_EDITOR
    public void UpdateSceneName()
    {
        sceneName = GetSceneName();
    }
#endif

    public string GetSceneName()
    {
#if UNITY_EDITOR
        if (sceneAsset != null)
        {
            var path = UnityEditor.AssetDatabase.GetAssetPath(sceneAsset);
            return System.IO.Path.GetFileNameWithoutExtension(path);
        }
        else
        {
            return string.Empty;
        }
#else
        return sceneName;
#endif
    }
}


public enum SceneType
{
    MainMenu,
    Game,
    LevelEditor
}