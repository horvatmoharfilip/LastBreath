using UnityEngine;

public class WaterDamage : MonoBehaviour
{
    public float damagePerSecond = 50f;
    public bool instantKill = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();

            if (health != null)
            {
                if (instantKill)
                {
                    health.TakeDamage(9999);
                }
                else
                {
                    health.TakeDamage(damagePerSecond * Time.deltaTime);
                }
            }
        }
    }
}