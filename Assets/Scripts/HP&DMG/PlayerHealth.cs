using StarterAssets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    private int currentHealth;
    public HealthBar healthBar;

    [Header("Inventory")]
    public List<string> inventory = new List<string>();

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

    private string savePath;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        _input = GetComponent<StarterAssetsInputs>();

        savePath = Application.persistentDataPath + "/save.json";

        // HEALTH INIT
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(currentHealth);
        deathScreen.SetActive(false);

        // STAMINA INIT
        currentStamina = maxStamina;
        staminaBar.SetMaxHealth((int)maxStamina);

        // AUTO LOAD
        LoadGame();
    }

    void Update()
    {
        HandleStamina();

        if (Input.GetKeyDown(KeyCode.F5))
        {
            SaveGame();
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            LoadGame();
        }
    }

    void OnApplicationQuit()
    {
        SaveGame();
    }

    // ---------------- SAVE / LOAD ----------------

    [System.Serializable]
    public class SaveData
    {
        public int health;
        public float posX;
        public float posY;
        public float posZ;
        public List<string> inventory;
    }

    public void SaveGame()
    {
        SaveData data = new SaveData();

        data.health = currentHealth;
        data.posX = transform.position.x;
        data.posY = transform.position.y;
        data.posZ = transform.position.z;
        data.inventory = inventory;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

        Debug.Log("GAME SAVED");
    }

    public void LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("Ni save file-a!");
            return;
        }

        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        // HEALTH
        currentHealth = Mathf.Clamp(data.health, 0, maxHealth);
        healthBar.SetHealth(currentHealth);

        // POSITION
        controller.enabled = false;
        transform.position = new Vector3(data.posX, data.posY, data.posZ);
        controller.enabled = true;

        // INVENTORY
        inventory = data.inventory;

        // MEDKIT SYNC
        if (MedkitManager.Instance != null)
        {
            MedkitManager.Instance.medkitCount = inventory.Count;
        }

        Debug.Log("GAME LOADED");
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

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        healthBar.SetHealth(currentHealth);
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
                TakeDamage(maxHealth);
            else
                TakeDamage(waterDamagePerSecond * Time.deltaTime);
        }
    }
}