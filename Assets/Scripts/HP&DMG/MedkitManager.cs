using UnityEngine;
using TMPro;

public class MedkitManager : MonoBehaviour
{
    public static MedkitManager Instance { get; private set; }

    public int medkitCount = 0;
    public TextMeshProUGUI medkitCountText; // drag a UI Text into this in Inspector

    private PlayerHealth playerHealth;

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
        playerHealth = FindObjectOfType<PlayerHealth>();
        UpdateUI();
    }

    private void Update()
    {
        // press H to use a medkit
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

        if (playerHealth == null) return;

        medkitCount--;
        playerHealth.Heal(25);
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (medkitCountText != null)
            medkitCountText.text = "Medkits: " + medkitCount;
    }
}