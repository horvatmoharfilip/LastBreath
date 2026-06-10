using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }
    public List<GameObject> weaponSlots;
    public GameObject activeWeaponSlot;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        activeWeaponSlot = weaponSlots[0];
    }

    private void Update()
    {
        foreach (GameObject weaponSlot in weaponSlots)
            weaponSlot.SetActive(weaponSlot == activeWeaponSlot);

        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchActiveSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchActiveSlot(1);
    }

    public void PickupWeapon(GameObject pickedUpWeapon)
    {
        AddWeaponIntoActiveSlot(pickedUpWeapon);
    }

    public void PickupMedkit(GameObject medkit)
    {
        DropCurrentWeapon(medkit);
        medkit.transform.SetParent(activeWeaponSlot.transform, false);
        MedkitHoldable holdable = medkit.GetComponent<MedkitHoldable>();
        medkit.transform.localPosition = holdable.spawnPosition;
        medkit.transform.localRotation = Quaternion.Euler(holdable.spawnRotation);
        holdable.isActiveItem = true;
        Rigidbody rb = medkit.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;
        Collider col = medkit.GetComponent<Collider>();
        if (col != null) col.enabled = false;
    }

    public void PickupFood(GameObject food)
    {
        DropCurrentWeapon(food);
        food.transform.SetParent(activeWeaponSlot.transform, false);
        FoodHoldable holdable = food.GetComponent<FoodHoldable>();
        food.transform.localPosition = holdable.spawnPosition;
        food.transform.localRotation = Quaternion.Euler(holdable.spawnRotation);
        holdable.isActiveItem = true;
        Rigidbody rb = food.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;
        Collider col = food.GetComponent<Collider>();
        if (col != null) col.enabled = false;
    }

    private void AddWeaponIntoActiveSlot(GameObject pickedUpWeapon)
    {
        DropCurrentWeapon(pickedUpWeapon);
        pickedUpWeapon.transform.SetParent(activeWeaponSlot.transform, false);
        Weapon weapon = pickedUpWeapon.GetComponent<Weapon>();
        pickedUpWeapon.transform.localPosition = weapon.spawnPosition;
        pickedUpWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation);
        weapon.isActiveWeapon = true;
    }

    private void DropCurrentWeapon(GameObject incoming)
{
    if (activeWeaponSlot.transform.childCount > 0)
    {
        var toDrop = activeWeaponSlot.transform.GetChild(0).gameObject;

        Weapon w = toDrop.GetComponent<Weapon>();
        if (w != null) w.isActiveWeapon = false;

        MedkitHoldable m = toDrop.GetComponent<MedkitHoldable>();
        if (m != null) m.isActiveItem = false;

        FoodHoldable f = toDrop.GetComponent<FoodHoldable>();
        if (f != null) f.isActiveItem = false;

        // odklopi iz roke in vrzi na tla
        toDrop.transform.SetParent(null);

        Rigidbody rb = toDrop.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(Camera.main.transform.forward * 2f, ForceMode.Impulse);
        }

        Collider col = toDrop.GetComponent<Collider>();
        if (col != null) col.enabled = true;
    }
}

    public void SwitchActiveSlot(int slotNumber)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            var current = activeWeaponSlot.transform.GetChild(0);
            Weapon w = current.GetComponent<Weapon>();
            if (w != null) w.isActiveWeapon = false;
            MedkitHoldable m = current.GetComponent<MedkitHoldable>();
            if (m != null) m.isActiveItem = false;
            FoodHoldable f = current.GetComponent<FoodHoldable>();
            if (f != null) f.isActiveItem = false;
        }

        activeWeaponSlot = weaponSlots[slotNumber];

        if (activeWeaponSlot.transform.childCount > 0)
        {
            var next = activeWeaponSlot.transform.GetChild(0);
            Weapon w = next.GetComponent<Weapon>();
            if (w != null) w.isActiveWeapon = true;
            MedkitHoldable m = next.GetComponent<MedkitHoldable>();
            if (m != null) m.isActiveItem = true;
            FoodHoldable f = next.GetComponent<FoodHoldable>();
            if (f != null) f.isActiveItem = true;
        }
    }
}