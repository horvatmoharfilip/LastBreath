using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set; }

    [Header("Settings")]
    public float interactRange = 3f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange))
        {
            GameObject obj = hit.transform.gameObject;

            if (obj.GetComponent<Weapon>())
            {
                if (Input.GetKeyDown(KeyCode.E))
                    WeaponManager.Instance.PickupWeapon(obj);
            }
            else if (obj.GetComponent<MedkitHoldable>())
            {
                if (Input.GetKeyDown(KeyCode.E))
                    WeaponManager.Instance.PickupMedkit(obj);
            }
            else if (obj.GetComponent<FoodHoldable>())
            {
                if (Input.GetKeyDown(KeyCode.E))
                    WeaponManager.Instance.PickupFood(obj);
            }
        }
    }
}