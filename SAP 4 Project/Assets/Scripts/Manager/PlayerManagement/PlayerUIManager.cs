using TMPro;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager Instance;

    public int fixedRoomRessource;
    public TextMeshProUGUI fixedRoomRessourceCount;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        fixedRoomRessource = Mathf.Clamp(fixedRoomRessource, 0, 2);
        fixedRoomRessourceCount.text = $"Fixed Room: {fixedRoomRessource}";
    }

    public void SetFixedRoomCount(int count)
    {
        if (fixedRoomRessource <= 0)
        {
            fixedRoomRessource = 0;
            fixedRoomRessourceCount.text = $"Fixed Room: {fixedRoomRessource}";

            Debug.Log("Cant fix any more rooms!");
            return;
        }

        if (LevelManager.Instance.IsRoomFixed(LevelManager.Instance.CurrentRoomCoord))
        {
            Debug.Log("Room already fixed!");
            return;
        }

        LevelManager.Instance.FixateRoom();

        fixedRoomRessource -= count;
        fixedRoomRessourceCount.text = $"Fixed Room: {fixedRoomRessource}";
    }
}
