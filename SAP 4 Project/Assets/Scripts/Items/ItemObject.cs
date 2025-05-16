using UnityEngine;

public enum ItemType
{
    Material,

}
public abstract class ItemObject : ScriptableObject
{
    public GameObject prefab;
    public ItemType type;
    [TextArea(15, 20)]
    public string discription;
}
