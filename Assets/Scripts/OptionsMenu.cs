using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private MainMenuManager mainMenuManager;

    [SerializeField]
    private Slider volumeSlider;

    [SerializeField]
    private TextMeshProUGUI volumeText;

    private MusicManager musicManager;

    void Start()
    {
        musicManager = MusicManager.Instance;
        
        if (volumeSlider == null)
        {
            volumeSlider = GetComponentInChildren<Slider>();
        }
        
        if (volumeText == null)
        {
            volumeText = GetComponentInChildren<TextMeshProUGUI>();
        }
        
        if (volumeSlider != null && musicManager != null)
        {
            volumeSlider.value = musicManager.Volume;
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
            UpdateVolumeText(musicManager.Volume);
        }
        else if (volumeSlider == null)
        {
            Debug.LogWarning("OptionsMenu: Volume Slider not found! Please assign it in the Inspector.");
        }
    }

    public void OnVolumeChanged(float value)
    {
        if (musicManager != null)
        {
            musicManager.Volume = value;
            UpdateVolumeText(value);
        }
    }

    private void UpdateVolumeText(float volume)
    {
        if (volumeText != null)
        {
            volumeText.text = $"Volume: {Mathf.RoundToInt(volume * 100)}%";
        }
    }

    public void SetVolume(float volume)
    {
        if (audioMixer != null)
        {
            audioMixer.SetFloat("MasterVolume", volume);
        }
        else if (musicManager != null)
        {
            musicManager.Volume = Mathf.Clamp01(volume);
        }
        else
        {
            AudioListener.volume = Mathf.Clamp01(volume / 80f + 1f);
        }
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void BackToMainMenu()
    {
        if (mainMenuManager != null)
        {
            mainMenuManager.CloseOptions();
        }
    }
}

