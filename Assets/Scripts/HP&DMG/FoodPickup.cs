using UnityEngine;
using TMPro;

public class FoodPickup : MonoBehaviour
{
    public float pickupRange = 2.5f;
    public LayerMask foodLayer;
    public KeyCode pickupKey = KeyCode.E;
    public TextMeshProUGUI promptText;

    private FoodHoldable nearestFood = null;

    private void Update()
    {
        FindNearestFood();

        if (nearestFood != null && Input.GetKeyDown(pickupKey))
        {
            WeaponManager.Instance.PickupFood(nearestFood.gameObject);
            nearestFood = null;
            HidePrompt();
        }
    }

    private void FindNearestFood()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, pickupRange, foodLayer);
        float minDist = float.MaxValue;
        FoodHoldable closest = null;

        foreach (Collider hit in hits)
        {
            FoodHoldable food = hit.GetComponent<FoodHoldable>();
            if (food == null) continue;
            float dist = Vector3.Distance(transform.position, hit.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = food;
            }
        }

        nearestFood = closest;
        if (nearestFood != null)
            ShowPrompt("[E] Poberi " + nearestFood.foodName);
        else
            HidePrompt();
    }

    private void ShowPrompt(string msg)
    {
        if (promptText != null)
        {
            promptText.text = msg;
            promptText.gameObject.SetActive(true);
        }
    }

    private void HidePrompt()
    {
        if (promptText != null)
            promptText.gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}