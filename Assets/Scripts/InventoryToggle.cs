using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryToggle : MonoBehaviour
{
    public CanvasGroup inventoryGroup;
    public KeyCode toggleKey = KeyCode.Tab;
    private FirstPersonController fpsController;
    private PlayerInput playerInput;

    private bool isOpen = false;

    void OpenInventory()
    {
        isOpen = true;
        inventoryGroup.alpha = 1f;
        inventoryGroup.interactable = true;
        inventoryGroup.blocksRaycasts = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        fpsController.enabled = false;
        playerInput.enabled = false;  // add this
    }

    void CloseInventory()
    {
        isOpen = false;
        inventoryGroup.alpha = 0f;
        inventoryGroup.interactable = false;
        inventoryGroup.blocksRaycasts = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        fpsController.enabled = true;
        playerInput.enabled = true;  // add this
    }
    void Start()
    {
        CloseInventory();
        fpsController = GameObject.FindFirstObjectByType<FirstPersonController>();
        playerInput = GameObject.FindFirstObjectByType<PlayerInput>();
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

    
}
