using UnityEngine;
using TMPro;

public class MedkitManager : MonoBehaviour
{
    public static MedkitManager Instance { get; private set; }

    [Header("Medkits")]
    public int medkitCount = 0;
    public TextMeshProUGUI medkitCountText;

    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;

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
        // fallback če nisi nastavil v Inspectorju
        if (playerHealth == null)
        {
            playerHealth = FindAnyObjectByType<PlayerHealth>();
        }

        UpdateUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            UseMedkit();
        }
    }

    public void PickupMedkit(GameObject medkitObject)
    {
        MedkitItem item = medkitObject.GetComponent<MedkitItem>();

        if (item != null)
        {
            medkitCount++;

            // 🔥 poveži z inventory sistemom
            if (playerHealth != null)
            {
                playerHealth.inventory.Add("Medkit");
            }

            UpdateUI();
            Destroy(medkitObject);
        }
    }

    private void UseMedkit()
    {
        if (medkitCount <= 0)
        {
            Debug.Log("No medkits!");
            return;
        }

        if (playerHealth == null)
        {
            Debug.LogWarning("PlayerHealth ni najden!");
            return;
        }

        medkitCount--;
        playerHealth.Heal(25);

        // 🔥 odstrani iz inventory
        if (playerHealth.inventory.Contains("Medkit"))
        {
            playerHealth.inventory.Remove("Medkit");
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (medkitCountText != null)
        {
            medkitCountText.text = "Medkits: " + medkitCount;
        }
    }
}