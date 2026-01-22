using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    public CanvasGroup inventoryGroup;
    public KeyCode toggleKey = KeyCode.Tab;  

    private bool isOpen = false;

    void Start()
    {
        CloseInventory();
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            if (isOpen)
                CloseInventory();
            else
                OpenInventory();
        }
    }

    void OpenInventory()
    {
        isOpen = true;
        inventoryGroup.alpha = 1f;
        inventoryGroup.interactable = true;
        inventoryGroup.blocksRaycasts = true;
    }

    void CloseInventory()
    {
        isOpen = false;
        inventoryGroup.alpha = 0f;
        inventoryGroup.interactable = false;
        inventoryGroup.blocksRaycasts = false;
    }
}
