using UnityEngine;
using UnityEngine.InputSystem; // required for new Input System

public class PickUp : MonoBehaviour
{
    public GameObject weaponPrefab;
    private bool playerInRange = false;
    private bool pickedUp = false;

    private void Update()
    {
        if (playerInRange && !pickedUp && Keyboard.current.eKey.wasPressedThisFrame)
        {
            pickedUp = true;
            Debug.Log("Picked up weapon");

            if (weaponPrefab != null)
            {
                Instantiate(weaponPrefab, transform.position, Quaternion.identity);
            }

            Collider col = GetComponent<Collider>();
            if (col != null) col.enabled = false;

            Destroy(gameObject, 0.05f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player entered pickup range");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player left pickup range");
        }
    }
}
