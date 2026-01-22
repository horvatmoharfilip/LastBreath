using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int damage = 10;  // Set the damage value for the enemy projectile

    private void OnCollisionEnter(Collision collision)
    {
        // If the enemy projectile hits the player
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);  // Apply damage to the player
            }
        }

        // Destroy the projectile on collision
        Destroy(gameObject);
    }
}
