using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    const string gameScene = "Game";
    public static SceneLoader Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SwitchScene(string scene)
    {
        SwitchSceneRoutine(scene);
    }

    List<Scene> scenesToUnload = new List<Scene>();
    IEnumerator SwitchSceneRoutine(string sceneName)
    {
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

        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
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
