using UnityEngine;

public class MedkitHoldable : MonoBehaviour
{
    public int healAmount = 25;
    public bool isActiveItem;

    public Vector3 spawnPosition;
    public Vector3 spawnRotation;

    private PlayerHealth player;

    private void Start()
    {
        // najde player SAMO ENKRAT
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