using UnityEngine;

public class MedkitHoldable : MonoBehaviour
{
    public int healAmount = 25;
    public bool isActiveItem;

    public Vector3 spawnPosition;
    public Vector3 spawnRotation;

    private void Update()
    {
        if (isActiveItem && Input.GetKeyDown(KeyCode.H))
        {
            Use();
        }
    }

    private void Use()
    {
        PlayerHealth player = FindObjectOfType<PlayerHealth>();
        if (player != null)
        {
            player.Heal(healAmount);
            Destroy(gameObject);
        }
    }
}
