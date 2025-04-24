using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
[DefaultExecutionOrder(-1000)]
public class SceneLoader : MonoBehaviour
{
    const string gameScene = "Game";
    public static SceneLoader Instance { get; private set; }

    public UnityEvent<string> onSceneChanged = new UnityEvent<string>();
    private void Awake()
    {
        Instance = this;
    }

    public void SwitchScene(string scene, string entryID)
    {
        StartCoroutine(SwitchSceneRoutine(scene, entryID));
    }

    List<Scene> scenesToUnload = new List<Scene>();
    IEnumerator SwitchSceneRoutine(string sceneName, string entryId)
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

        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        onSceneChanged.Invoke(entryId);
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
