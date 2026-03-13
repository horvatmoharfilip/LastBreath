using UnityEngine;

public class DayNightSystem : MonoBehaviour
{
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
        // Rotate sun
        sun.transform.Rotate(Vector3.right * daySpeed * Time.deltaTime);

        // Get sun height
        float sunHeight = Mathf.Clamp01(sun.transform.forward.y);

        // Sun intensity
        sun.intensity = Mathf.Lerp(0f, 1f, sunHeight);

        // Fog color change
        RenderSettings.fogColor = Color.Lerp(nightFogColor, dayFogColor, sunHeight);

        // Fog density change
        RenderSettings.fogDensity = Mathf.Lerp(nightFogDensity, dayFogDensity, sunHeight);
    }
}