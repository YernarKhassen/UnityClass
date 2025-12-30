using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    public static OptionsMenu Instance { get; private set; }

    [Header("UI Elements")]
    [SerializeField] private GameObject panel;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI volumeText;

    private MusicManager musicManager;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        musicManager = MusicManager.Instance;
        if (musicManager == null)
        {
            Debug.LogError("OptionsMenu: MusicManager not found!");
            return;
        }

        if (panel == null)
            panel = transform.Find("OptionsPanel")?.gameObject;

        if (volumeSlider == null)
            volumeSlider = GetComponentInChildren<Slider>();

        if (volumeText == null)
            volumeText = GetComponentInChildren<TextMeshProUGUI>();

        if (volumeSlider != null)
        {
            volumeSlider.value = musicManager.Volume;
            volumeSlider.onValueChanged.RemoveAllListeners();
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }

        UpdateVolumeText(musicManager.Volume);

        if (panel != null)
            panel.SetActive(false);
    }

    private void OnVolumeChanged(float value)
    {
        if (musicManager != null)
        {
            musicManager.Volume = value;
            UpdateVolumeText(value);
        }
    }

    private void UpdateVolumeText(float value)
    {
        if (volumeText != null)
            volumeText.text = $"Volume: {Mathf.RoundToInt(value * 100)}%";
    }
}
