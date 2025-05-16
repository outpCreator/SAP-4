using UnityEngine;

public class DontDetroyOnLoadScript : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
