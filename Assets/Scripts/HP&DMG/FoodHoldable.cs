using UnityEngine;

public class FoodHoldable : MonoBehaviour
{
    public string foodName = "Apple";
    public int healAmount = 20;
    public bool isActiveItem;
    public Vector3 spawnPosition;
    public Vector3 spawnRotation;

    private PlayerHealth player;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerHealth>();
    }

    private void Update()
    {
        if (isActiveItem && Input.GetKeyDown(KeyCode.H))
        {
            Use();
        }
    }

    private void Use()
    {
        if (player != null)
        {
            player.Heal(healAmount);
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("PlayerHealth ni najden!");
        }
    }
}