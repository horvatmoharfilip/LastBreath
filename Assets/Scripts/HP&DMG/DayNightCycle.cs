using UnityEngine;

public class DayNightSystem : MonoBehaviour
{
    [Header("Sun")]
    public Light sun;
    public float daySpeed = 10f;

    [Header("Fog Colors")]
    public Color dayFogColor = new Color(0.8f, 0.9f, 1f);
    public Color nightFogColor = new Color(0.05f, 0.05f, 0.1f);

    [Header("Fog Density")]
    public float dayFogDensity = 0.01f;
    public float nightFogDensity = 0.03f;

    void Update()
    {
        RotateSun();
        UpdateLightingAndFog();
    }

    void RotateSun()
    {
        // Vrti sonce (dan/noč cikel)
        sun.transform.Rotate(Vector3.right * daySpeed * Time.deltaTime);
    }

    void UpdateLightingAndFog()
    {
        // --- SONCE (osnovni faktor) ---
        float sunDot = sun.transform.forward.y;

        // Zamaknjen prehod (noč traja dlje)
        float sunHeight = Mathf.InverseLerp(-0.2f, 0.3f, sunDot);

        // Zgladi prehod
        sunHeight = Mathf.SmoothStep(0f, 1f, sunHeight);

        // --- INTENZITETA SONCA ---
        sun.intensity = Mathf.Lerp(0f, 1f, sunHeight);

        // --- MEGLA (ločeno počasnejši prehod) ---
        float fogFactor = Mathf.InverseLerp(-0.1f, 0.4f, sunDot);
        fogFactor = Mathf.SmoothStep(0f, 1f, fogFactor);

        // Barva megle
        RenderSettings.fogColor = Color.Lerp(nightFogColor, dayFogColor, fogFactor);

        // Gostota megle
        RenderSettings.fogDensity = Mathf.Lerp(nightFogDensity, dayFogDensity, fogFactor);
    }
}