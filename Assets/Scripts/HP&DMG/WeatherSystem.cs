using UnityEngine;

public class WeatherSystem : MonoBehaviour
{
    public enum WeatherType
    {
        Sunny,
        Rain,
        Fog
    }

    public WeatherType currentWeather;

    [Header("References")]
    public Light sun;
    public ParticleSystem rainEffect;

    [Header("Fog")]
    public Color fogColorSunny;
    public Color fogColorFoggy;
    public float fogDensitySunny = 0.01f;
    public float fogDensityFoggy = 0.05f;

    [Header("Auto Weather")]
    public float weatherChangeTime = 30f; // vsakih 30 sekund
    private float timer;

    void Start()
    {
        ApplyWeather(currentWeather);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= weatherChangeTime)
        {
            timer = 0f;

            int randomWeather = Random.Range(0, 3);
            ApplyWeather((WeatherType)randomWeather);
        }
    }

    public void ApplyWeather(WeatherType weather)
    {
        currentWeather = weather;

        switch (weather)
        {
            case WeatherType.Sunny:
                RenderSettings.fog = false;
                sun.intensity = 1.2f;

                rainEffect.Stop();
                break;

            case WeatherType.Rain:
                RenderSettings.fog = true;
                RenderSettings.fogColor = fogColorSunny;
                RenderSettings.fogDensity = 0.02f;

                sun.intensity = 0.6f;

                rainEffect.Play();
                break;

            case WeatherType.Fog:
                RenderSettings.fog = true;
                RenderSettings.fogColor = fogColorFoggy;
                RenderSettings.fogDensity = fogDensityFoggy;

                sun.intensity = 0.4f;

                rainEffect.Stop();
                break;
        }
    }
}