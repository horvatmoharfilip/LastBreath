using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    private int currentHealth;
    public HealthBar healthBar;

    [Header("Stamina")]
    public float maxStamina = 100f;
    public float drainRate = 15f;
    public float regenRate = 10f;
    public float regenDelay = 1.5f;
    public HealthBar staminaBar;

    private float currentStamina;
    private float regenDelayTimer = 0f;
    private bool isExhausted = false;

    [Header("Other")]
    public GameObject deathScreen;
    public Transform respawnPoint;

    [Header("Water")]
    public float waterDamagePerSecond = 50f;
    public bool waterInstantKill = false;

    private CharacterController controller;
    private PlayerInput playerInput;
    private StarterAssetsInputs _input;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        _input = GetComponent<StarterAssetsInputs>();

        // HEALTH
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(currentHealth);
        deathScreen.SetActive(false);

        // STAMINA
        currentStamina = maxStamina;
        staminaBar.SetMaxHealth((int)maxStamina);
    }

    void Update()
    {
        HandleStamina();
    }

    // ---------------- STAMINA ----------------

    private void HandleStamina()
    {
        bool wantsToSprint = _input.sprint && _input.move != Vector2.zero;

        if (isExhausted && currentStamina >= maxStamina * 0.3f)
            isExhausted = false;

        bool isSprinting = wantsToSprint && !isExhausted;

        if (isSprinting)
        {
            currentStamina -= drainRate * Time.deltaTime;
            regenDelayTimer = regenDelay;

            if (currentStamina <= 0f)
            {
                currentStamina = 0f;
                isExhausted = true;
            }
        }
        else
        {
            if (regenDelayTimer > 0f)
                regenDelayTimer -= Time.deltaTime;
            else if (currentStamina < maxStamina)
            {
                currentStamina += regenRate * Time.deltaTime;
                currentStamina = Mathf.Min(currentStamina, maxStamina);
            }
        }

        staminaBar.SetHealth((int)currentStamina);
    }

    public bool CanSprint() => !isExhausted && currentStamina > 0f;

    // ---------------- HEALTH ----------------

    public void TakeDamage(float damage)
    {
        currentHealth -= Mathf.RoundToInt(damage);
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Time.timeScale = 0f;
        deathScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerInput.enabled = false;
    }

    public void Respawn()
    {
        Time.timeScale = 1f;
        deathScreen.SetActive(false);

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(currentHealth);

        currentStamina = maxStamina;
        staminaBar.SetMaxHealth((int)maxStamina);
        isExhausted = false;

        controller.enabled = false;
        transform.position = respawnPoint.position;
        controller.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerInput.enabled = true;
    }

    // ---------------- WATER ----------------

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            if (waterInstantKill)
            {
                TakeDamage(maxHealth);
            }
            else
            {
                TakeDamage(waterDamagePerSecond * Time.deltaTime);
            }
        }
    }
}