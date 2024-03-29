using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Slider sensivitySlider;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private CameraConfig cameraConfig;
    [SerializeField] private TextMeshProUGUI sensivityValueTextPro;
    [SerializeField] private TextMeshProUGUI volumeValueTextPro;

    private AudioSource _audioSource;
    
    Resolution[] resolutions;

    void Start()
    {
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height + " " + resolutions[i].refreshRate + "Hz";
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();
        _audioSource = Camera.main.GetComponent<AudioSource>();
        LoadSettings(currentResolutionIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void ApplySettings()
    {
        PlayerPrefs.SetInt("ResolutionSettingsPreference", resolutionDropdown.value);
        PlayerPrefs.SetInt("FullScreenPreference", System.Convert.ToInt32(Screen.fullScreen));
    }

    public void LoadSettings(int currentResolutionIndex)
    {
        if (PlayerPrefs.HasKey("ResolutionSettingsPreference"))
            resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionSettingsPreference");
        else
            resolutionDropdown.value = currentResolutionIndex;
        
        if (PlayerPrefs.HasKey("FullScreenPreference"))
            Screen.fullScreen = System.Convert.ToBoolean(PlayerPrefs.GetInt("FullScreenPreference"));
        else
            Screen.fullScreen = true;
    }

    public void UpdateSensivity() 
    {
        cameraConfig.sensivity = sensivitySlider.value;
        sensivityValueTextPro.text = sensivitySlider.value.ToString("0");
    }

    public void UpdateVolume()
    {
        PlayerPrefs.SetInt("PlayerVolume", (int)volumeSlider.value);
        _audioSource.volume = PlayerPrefs.GetInt("PlayerVolume") / volumeSlider.maxValue;
        volumeValueTextPro.text = volumeSlider.value.ToString("0");
    }

    public void StartVolume()
    {
        if (_audioSource == null)
            _audioSource = Camera.main.GetComponent<AudioSource>();

        if (!PlayerPrefs.HasKey("PlayerVolume"))
        {
            PlayerPrefs.SetInt("PlayerVolume", (int)volumeSlider.maxValue / 2);
            _audioSource.volume = PlayerPrefs.GetInt("PlayerVolume") / volumeSlider.maxValue;
            volumeSlider.value = PlayerPrefs.GetInt("PlayerVolume");
            volumeValueTextPro.text = volumeSlider.value.ToString("0");
        }
        else
        {
            _audioSource.volume = PlayerPrefs.GetInt("PlayerVolume") / volumeSlider.maxValue;
            volumeSlider.value = PlayerPrefs.GetInt("PlayerVolume");
            volumeValueTextPro.text = volumeSlider.value.ToString("0");
        }
    }
}
