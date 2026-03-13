using UnityEngine;

public class DayNightSky : MonoBehaviour
{
    public Material daySky;
    public Material nightSky;
    public Light sun;

    void Update()
    {
        if (sun.transform.forward.y < 0)
        {
            RenderSettings.skybox = nightSky;
        }
        else
        {
            RenderSettings.skybox = daySky;
        }
    }
}