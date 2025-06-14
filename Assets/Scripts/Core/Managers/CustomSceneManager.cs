using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : PersistentSingleton<CustomSceneManager>
{
    [SerializeField] private SceneDatabase sceneDatabase;

    private Dictionary<SceneType, string> nameToPathMap = new();

    private void Start()
    {
        foreach (var sceneRef in sceneDatabase.scenes)
        {
            nameToPathMap[sceneRef.sceneType] = sceneRef.GetSceneName();
        }
    }

    public void LoadScene(SceneType sceneType, bool async = false)
    {
        if (!nameToPathMap.ContainsKey(sceneType))
        {
            Debug.LogError($"Scene '{sceneType}' not found in database!");
            return;
        }

        if (async)
            StartCoroutine(LoadSceneAsync(nameToPathMap[sceneType]));
        else
            SceneManager.LoadScene(nameToPathMap[sceneType]);
    }

    private IEnumerator LoadSceneAsync(string path)
    {
        var op = SceneManager.LoadSceneAsync(path);
        while (!op.isDone)
        {
            yield return null;
        }
    }
}
