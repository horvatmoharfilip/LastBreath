using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set; }

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

        if (Physics.Raycast(ray, out hit))
        {
            GameObject objectHitByRaycast = hit.transform.gameObject;

            // weapon pickup
            if (objectHitByRaycast.GetComponent<Weapon>())
            {
                if (Input.GetKeyDown(KeyCode.E))
                    WeaponManager.Instance.PickupWeapon(objectHitByRaycast.gameObject);
                Debug.Log("Weapon Selected");
            }

            // medkit pickup
            if (objectHitByRaycast.GetComponent<MedkitHoldable>())
            {
                if (Input.GetKeyDown(KeyCode.E))
                    WeaponManager.Instance.PickupMedkit(objectHitByRaycast.gameObject);
                Debug.Log("Medkit Selected");
            }
        }
    }
}