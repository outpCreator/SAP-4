using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    const string gameScene = "Game";
    public static SceneLoader Instance { get; private set; }

    string pendingEntryID;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SwitchScene(string scene, string entryID)
    {
        pendingEntryID = entryID;
        StartCoroutine(SwitchSceneRoutine(scene));
    }

    List<Scene> scenesToUnload = new List<Scene>();
    IEnumerator SwitchSceneRoutine(string sceneName)
    {
        yield return ScreenFader.Instance != null? 
            ScreenFader.Instance.FadeToBlack() : null;

        scenesToUnload ??= new List<Scene>(2);
        scenesToUnload.Clear();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            if (scene.name != gameScene)
            {
                scenesToUnload.Add(scene);
            }
        }

        foreach (Scene scene in scenesToUnload)
        {
            yield return SceneManager.UnloadSceneAsync(scene);
        }

        var loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return loadOperation;

        Scene loadedScene = SceneManager.GetSceneByName(sceneName);
        if (loadedScene.IsValid())
            SceneManager.SetActiveScene(loadedScene);

        yield return new WaitForEndOfFrame();

        if (ScreenFader.Instance != null)
            yield return ScreenFader.Instance.FadeFromBlack();
    }

    [RuntimeInitializeOnLoadMethod]
    static void LoadGameScene()
    {
        if (!SceneManager.GetSceneByName(gameScene).IsValid())
        {
            SceneManager.LoadScene(gameScene, LoadSceneMode.Additive);
        }
    }
}
