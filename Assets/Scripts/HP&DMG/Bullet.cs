using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10;  // Add a damage value to the bullet

    private void OnCollisionEnter(Collision collision)
    {
        // If the bullet hits an enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // Apply damage to the enemy
            }
            Destroy(gameObject);  // Destroy the bullet
        }
        // If the bullet hits the player
        else if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);  // Apply damage to the player
            }
            Destroy(gameObject);  // Destroy the bullet
        }

        // If the bullet hits a wall or something else, just destroy it
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
