using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public GameObject damagePopupPrefab; // assign prefab in inspector

    public Transform player;
    public Transform aimPoint; //  NEW

    public float health = 100f;
    public EnemyHealthBar healthBar; // drag your EnemyHealthBar prefab here in Inspector
    public int maxHealth = 100;      // optional, for scaling

    public LayerMask whatIsGround, whatIsPlayer;

    // Patrol
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange = 10f;

    // Attack
    public float timeBetweenAttacks = 1.5f;
    bool alreadyAttacked;
    public GameObject projectile;
    public float shootForce = 20f;

    // States
    public float sightRange = 15f, attackRange = 10f;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            aimPoint = player.Find("AimPoint"); // AUTO FIND
        }
    }

    private void Update()
    {
        if (player == null) return;

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        else if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        else if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        if (Vector3.Distance(transform.position, walkPoint) < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        Vector3 point = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(point, Vector3.down, 2f, whatIsGround))
        {
            walkPoint = point;
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.isStopped = true;

        Transform target = aimPoint != null ? aimPoint : player;

        transform.LookAt(target.position); //  LOOK AT REAL HEIGHT

        if (!alreadyAttacked)
        {
            Rigidbody rb = Instantiate(
                projectile,
                transform.position + transform.forward + Vector3.up,
                Quaternion.identity
            ).GetComponent<Rigidbody>();

            Vector3 dir = (target.position - rb.position).normalized;
            rb.AddForce(dir * shootForce, ForceMode.Impulse);

            Destroy(rb.gameObject, 3f);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
        agent.isStopped = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        // Update health bar
        if (healthBar != null)
            healthBar.SetHealth((int)health); // EnemyHealthBar handles fill & gradient

        // Damage popup
        if (damagePopupPrefab != null)
        {
            GameObject popup = Instantiate(damagePopupPrefab, transform.position + Vector3.up * 2f, Quaternion.identity);
            popup.GetComponent<DamagePopup>().Setup(damage, transform);
        }

        if (health <= 0)
            Invoke(nameof(DestroyEnemy), 0.5f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
