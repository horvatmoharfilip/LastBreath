using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    [Header("Spawning")]
    public GameObject[] enemyPrefabs;     // drag your deer, wolf etc here
    public int totalEnemies = 30;
    public float minSpawnDistance = 20f;  // min distance from player
    public float maxSpawnDistance = 80f;  // max distance from player

    [Header("Win Condition")]
    public GameObject winScreen;          // drag your win screen UI here

    private Transform player;
    private int enemiesRemaining;
    private int enemiesSpawned;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemiesRemaining = totalEnemies;
        enemiesSpawned = 0;

        if (winScreen != null)
            winScreen.SetActive(false);

        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (enemiesSpawned < totalEnemies)
        {
            SpawnEnemy();
            enemiesSpawned++;
            yield return new WaitForSeconds(0.2f); // small delay between spawns
        }
    }

    private void SpawnEnemy()
    {
        // try to find a valid spawn point
        for (int i = 0; i < 10; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle.normalized;
            float distance = Random.Range(minSpawnDistance, maxSpawnDistance);

            Vector3 spawnPos = new Vector3(
                player.position.x + randomCircle.x * distance,
                player.position.y + 10f, // start high so it falls to ground
                player.position.z + randomCircle.y * distance
            );

            // raycast down to find ground
            if (Physics.Raycast(spawnPos, Vector3.down, out RaycastHit hit, 20f))
            {
                GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                Instantiate(prefab, hit.point, Quaternion.identity);
                return;
            }
        }
    }

    // call this when an enemy dies
    public void EnemyKilled()
    {
        enemiesRemaining--;

        if (enemiesRemaining <= 0)
            Win();
    }

    private void Win()
    {
        Time.timeScale = 0f;
        if (winScreen != null)
            winScreen.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}