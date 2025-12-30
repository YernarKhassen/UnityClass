using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class WinMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject winMenuPanel;
    [SerializeField] private GameObject optionsMenuPanel;

    [Header("Buttons")]
    [SerializeField] private Button continueButton;
    [SerializeField] private Button reloadButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;

    [Header("Volume UI")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI volumeText;

    private GameSceneManager sceneManager;
    private MusicManager musicManager;
    private int lastLevelIndex = 0;

    private void Start()
    {
        sceneManager = GameSceneManager.Instance;
        musicManager = MusicManager.Instance;

        lastLevelIndex = PlayerPrefs.GetInt("LastLevelIndex", 0);

        SetupButtons();
        SetupOptionsPanel();
    }

    private void SetupButtons()
    {
        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinueClicked);

        if (reloadButton != null)
            reloadButton.onClick.AddListener(OnReloadClicked);

        if (optionsButton != null)
            optionsButton.onClick.AddListener(OnOptionsClicked);
        
        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitGame);
    }

    private void SetupOptionsPanel()
    {
        if (optionsMenuPanel != null)
        {
            if (volumeSlider == null)
                volumeSlider = optionsMenuPanel.GetComponentInChildren<Slider>();

            if (volumeText == null)
                volumeText = optionsMenuPanel.GetComponentInChildren<TextMeshProUGUI>();

            if (volumeSlider != null && musicManager != null)
            {
                volumeSlider.value = musicManager.Volume;
                volumeSlider.onValueChanged.RemoveAllListeners();
                volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
                UpdateVolumeText(musicManager.Volume);
            }

            optionsMenuPanel.SetActive(false);
        }
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

    public void OnContinueClicked()
    {
        if (sceneManager == null) return;

        int nextLevel = lastLevelIndex + 1;
        int maxLevel = sceneManager.GetLevelCount() - 1;

        if (nextLevel <= maxLevel)
            sceneManager.LoadLevel(nextLevel);
        else
            sceneManager.LoadLevel(0);
    }

    public void OnReloadClicked()
    {
        if (sceneManager == null) return;

        if (lastLevelIndex >= 0)
            sceneManager.LoadLevel(lastLevelIndex);
        else
            sceneManager.LoadLevel(0);
    }

    public void OnOptionsClicked()
    {
        if (winMenuPanel != null)
            winMenuPanel.SetActive(false);
        
        if (optionsMenuPanel != null)
            optionsMenuPanel.SetActive(true);
    }

    public void OnQuitGame()
    {
        EditorApplication.isPlaying = false;
    }
    
    public void CloseOptions()
    {
        if (optionsMenuPanel != null)
            optionsMenuPanel.SetActive(false);
        
        if (winMenuPanel != null)
            winMenuPanel.SetActive(true);
    }
}
