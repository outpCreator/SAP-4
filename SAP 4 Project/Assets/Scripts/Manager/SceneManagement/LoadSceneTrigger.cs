using UnityEngine;

public class LoadSceneTrigger : MonoBehaviour
{
    public string sceneToLoad;
    public string entryID;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneLoader.Instance.SwitchScene(sceneToLoad, entryID);
        }
    }
}
