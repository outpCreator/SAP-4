using UnityEngine;

public class UIComponent : MonoBehaviour
{
    public void OpenMenu()
    {
        UIManager.Instance.SetCallerActive(this.gameObject); 
    }
}
