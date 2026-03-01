using UnityEngine;

[CreateAssetMenu(fileName = "item", menuName = "newItem")]

public class InventoryD : ScriptableObject
{

    public string itemName;
    public Sprite icon;
    public int maxStackSize;
    public GameObject itemPrefab;
    public GameObject handItemPrefab;


}

