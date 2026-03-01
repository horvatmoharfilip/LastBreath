using UnityEngine;
using UnityEngine.UI;
using StarterAssets;

public class SettingsManager : MonoBehaviour
{
    [Header("Audio")]
    public Slider volumeSlider;

    [Header("Mouse Sensitivity")]
    public Slider sensitivitySlider;
    public FirstPersonController fpsController;

    [Header("Display")]
    public Toggle fullscreenToggle;

    // SAFE DEFAULT VALUES
    private const float DEFAULT_VOLUME = 1f;
    private const float DEFAULT_SENSITIVITY = 1f;
    private const int DEFAULT_FULLSCREEN = 1;

    private void Start()
    {
        // MAKE SURE SLIDER CAN'T BE 0
        sensitivitySlider.minValue = 0.1f;
        sensitivitySlider.maxValue = 5f;

        // LOAD SAVED SETTINGS (SAFE DEFAULTS)
        float savedVolume = PlayerPrefs.GetFloat("Volume", DEFAULT_VOLUME);
        float savedSensitivity = PlayerPrefs.GetFloat("Sensitivity", DEFAULT_SENSITIVITY);
        bool savedFullscreen = PlayerPrefs.GetInt("Fullscreen", DEFAULT_FULLSCREEN) == 1;

        // EXTRA PROTECTION (if sensitivity somehow saved as 0)
        if (savedSensitivity <= 0f)
            savedSensitivity = DEFAULT_SENSITIVITY;

        // APPLY VALUES
        AudioListener.volume = savedVolume;
        fpsController.RotationSpeed = savedSensitivity;
        Screen.fullScreen = savedFullscreen;

        // UPDATE UI
        volumeSlider.value = savedVolume;
        sensitivitySlider.value = savedSensitivity;
        fullscreenToggle.isOn = savedFullscreen;

        // ADD LISTENERS
        volumeSlider.onValueChanged.AddListener(SetVolume);
        sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
    }

    public void SetSensitivity(float value)
    {
        // NEVER ALLOW 0
        value = Mathf.Max(0.1f, value);

        fpsController.RotationSpeed = value;
        PlayerPrefs.SetFloat("Sensitivity", value);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }

    // OPTIONAL RESET BUTTON
    public void ResetSettings()
    {
        PlayerPrefs.DeleteKey("Volume");
        PlayerPrefs.DeleteKey("Sensitivity");
        PlayerPrefs.DeleteKey("Fullscreen");

        AudioListener.volume = DEFAULT_VOLUME;
        fpsController.RotationSpeed = DEFAULT_SENSITIVITY;
        Screen.fullScreen = DEFAULT_FULLSCREEN == 1;

        volumeSlider.value = DEFAULT_VOLUME;
        sensitivitySlider.value = DEFAULT_SENSITIVITY;
        fullscreenToggle.isOn = DEFAULT_FULLSCREEN == 1;
    }
}