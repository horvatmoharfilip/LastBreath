using UnityEngine;
using UnityEngine.UI;

public class HotbarUI : MonoBehaviour
{
    public Image slot1Icon;
    public Image slot2Icon;

    public Sprite emptySprite;

    public Sprite shotgunSprite;
    public Sprite ak47Sprite;
    public Sprite sniperSprite;
    public Sprite medkitSprite;
    public Sprite appleSprite;
    public Sprite bananaSprite;

    private void Update()
    {
        UpdateSlot(WeaponManager.Instance.weaponSlots[0], slot1Icon);
        UpdateSlot(WeaponManager.Instance.weaponSlots[1], slot2Icon);
    }

    private void UpdateSlot(GameObject slot, Image icon)
    {
        if (slot.transform.childCount == 0)
        {
            icon.sprite = emptySprite;
            icon.color = new Color(1, 1, 1, 0); // prozorno èe prazno
            return;
        }

        GameObject item = slot.transform.GetChild(0).gameObject;
        icon.color = Color.white;

        Weapon w = item.GetComponent<Weapon>();
        if (w != null)
        {
            if (item.name.ToLower().Contains("ak")) icon.sprite = ak47Sprite;
            else if (item.name.ToLower().Contains("sniper")) icon.sprite = sniperSprite;
            else icon.sprite = shotgunSprite;
            return;
        }

        MedkitHoldable m = item.GetComponent<MedkitHoldable>();
        if (m != null) { icon.sprite = medkitSprite; return; }

        FoodHoldable f = item.GetComponent<FoodHoldable>();
        if (f != null)
        {
            if (f.foodName.ToLower().Contains("banana")) icon.sprite = bananaSprite;
            else icon.sprite = appleSprite;
            return;
        }

        icon.sprite = emptySprite;
        icon.color = new Color(1, 1, 1, 0);
    }
}