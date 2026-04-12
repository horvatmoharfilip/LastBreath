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
        {
            weaponSlot.SetActive(weaponSlot == activeWeaponSlot);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchActiveSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchActiveSlot(1);
    }

    public void PickupWeapon(GameObject pickedUpWeapon)
    {
        AddWeaponIntoActiveSlot(pickedUpWeapon);
    }

    public void PickupMedkit(GameObject medkit)
    {
        // drop current weapon if any
        DropCurrentWeapon(medkit);

        // place medkit into active slot like a weapon
        medkit.transform.SetParent(activeWeaponSlot.transform, false);

        MedkitHoldable holdable = medkit.GetComponent<MedkitHoldable>();
        medkit.transform.localPosition = holdable.spawnPosition;
        medkit.transform.localRotation = Quaternion.Euler(holdable.spawnRotation);

        holdable.isActiveItem = true;

        // disable rigidbody so it doesnt fall
        Rigidbody rb = medkit.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        // disable collider so it doesnt block raycasts
        Collider col = medkit.GetComponent<Collider>();
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

    private void DropCurrentWeapon(GameObject pickedUpWeapon)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;

            // handle both weapon and medkit
            Weapon w = weaponToDrop.GetComponent<Weapon>();
            if (w != null) w.isActiveWeapon = false;

            MedkitHoldable m = weaponToDrop.GetComponent<MedkitHoldable>();
            if (m != null) m.isActiveItem = false;

            weaponToDrop.transform.SetParent(pickedUpWeapon.transform.parent);
            weaponToDrop.transform.localPosition = pickedUpWeapon.transform.localPosition;
            weaponToDrop.transform.localRotation = pickedUpWeapon.transform.localRotation;
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
        }

        activeWeaponSlot = weaponSlots[slotNumber];

        if (activeWeaponSlot.transform.childCount > 0)
        {
            var next = activeWeaponSlot.transform.GetChild(0);
            Weapon w = next.GetComponent<Weapon>();
            if (w != null) w.isActiveWeapon = true;

            MedkitHoldable m = next.GetComponent<MedkitHoldable>();
            if (m != null) m.isActiveItem = true;
        }
    }
}