using UnityEngine;
using UnityEngine.InputSystem; // pomembno za PlayerInput

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public HealthBar healthBar;
    public GameObject deathScreen;
    public Transform respawnPoint;

    private CharacterController controller;
    private PlayerInput playerInput;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(currentHealth);

        deathScreen.SetActive(false);

        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Time.timeScale = 0f;
        deathScreen.SetActive(true);

        // Odkleni miško
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Izklopi movement/input
        playerInput.enabled = false;
    }

    public void Respawn()
    {
        Time.timeScale = 1f;
        deathScreen.SetActive(false);

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(currentHealth);

        // Teleport (pomembno za CharacterController)
        controller.enabled = false;
        transform.position = respawnPoint.position;
        controller.enabled = true;

        // Zakleni miško nazaj
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Vklopi input
        playerInput.enabled = true;
    }
}