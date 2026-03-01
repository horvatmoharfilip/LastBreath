using UnityEngine;
using System.Collections.Generic;

public class InventoryToggle : MonoBehaviour
{
    public InventoryD woodItem;
    public InventoryD gunItem;

    public GameObject hotBarObj;
    public GameObject InventorySlotParent;

    private List<Slot> inventorySlots = new List<Slot>();
    private List<Slot> hotBarSlots = new List<Slot>();
    private List<Slot> allSlots = new List<Slot>();

    private void Awake()
    {
        inventorySlots.AddRange(InventorySlotParent.GetComponentsInChildren<Slot>());
        hotBarSlots.AddRange(hotBarObj.GetComponentsInChildren<Slot>());

        allSlots.AddRange(inventorySlots);
        allSlots.AddRange(hotBarSlots);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            AddItem(woodItem, 3);
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            AddItem(gunItem, 3);
        }
    }


    public void AddItem(InventoryD itemToAdd, int amount)
    {
        int remanining = amount;

        foreach (Slot slot in allSlots)
        {
            if(slot.HasItem() && slot.GetItem() == itemToAdd)
            {
                int currentAmount = slot.GetAmount();
                int maxStack = itemToAdd.maxStackSize;

                if(currentAmount < maxStack)
                {
                    int spaceLeft = maxStack - currentAmount;
                    int amountToAdd = Mathf.Min(spaceLeft, remanining); 

                    slot.SetItem(itemToAdd, currentAmount + amountToAdd);
                    remanining -= amountToAdd;

                    if (remanining <= 0)
                        return;
                }
            }
        }

        foreach (Slot slot in allSlots)
        {
            if (!slot.HasItem())
            {
                int amountToPlace = Mathf.Min(itemToAdd.maxStackSize, remanining);
                slot.SetItem(itemToAdd, amountToPlace);
                remanining -= amountToPlace;    

                if(remanining <= 0)
                    return;
            }
        }


        if(remanining > 0)
        {
            Debug.Log("Inventory Full");
        }
    }

}