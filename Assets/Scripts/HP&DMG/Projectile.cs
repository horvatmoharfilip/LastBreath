using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 10;

    public enum OwnerType { Player, Enemy }
    public OwnerType owner;

    private void OnTriggerEnter(Collider other)
    {
        // Enemy bullet hits player
        if (owner == OwnerType.Enemy && other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>()?.TakeDamage(damage);
            Destroy(gameObject);
        }

        // Player bullet hits enemy
        if (owner == OwnerType.Player && other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyAI>()?.TakeDamage(damage);
            other.GetComponent<MeleeEnemyAI>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
