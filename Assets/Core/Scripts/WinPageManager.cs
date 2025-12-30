using UnityEngine;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class WinPageManager : MonoBehaviour
{
    private GameSceneManager sceneManager;
    private MusicManager musicManager;
    private int currentLevelIndex = -1;

    [Header("Button References")]
    [SerializeField] private Button continueButton;
    [SerializeField] private Button reloadButton;
    [SerializeField] private Button optionsButton;

    [Header("Options UI")]
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI volumeText;

    [Header("Settings")]
    [SerializeField] private bool autoFindButtons = true;

    private void Start()
    {
        Debug.Log("WinPageManager: Start() called");
        
        sceneManager = GameSceneManager.Instance;
        musicManager = MusicManager.Instance;

        if (sceneManager == null)
        {
            Debug.LogError("WinPageManager: GameSceneManager.Instance is null!");
        }
        else
        {
            Debug.Log("WinPageManager: GameSceneManager found");
        }

        if (musicManager == null)
        {
            Debug.LogWarning("WinPageManager: MusicManager.Instance is null (may not be initialized yet)");
        }

        currentLevelIndex = PlayerPrefs.GetInt("LastLevelIndex", 0);
        Debug.Log($"WinPageManager: Current level index = {currentLevelIndex}");

        SetupButtons();
        
        SetupOptionsPanel();
        
        UpdateButtonStates();
        
        Debug.Log("WinPageManager: Initialization complete");
    }

    private void SetupButtons()
    {
        if (autoFindButtons)
        {
            if (continueButton == null)
            {
                continueButton = GameObject.Find("ContinueButton")?.GetComponent<Button>();
                if (continueButton == null)
                {
                    continueButton = GameObject.Find("StartButton")?.GetComponent<Button>();
                    if (continueButton == null)
                    {
                        continueButton = GameObject.Find("NextLevelButton")?.GetComponent<Button>();
                    }
                }
            }
            if (reloadButton == null)
            {
                reloadButton = GameObject.Find("ReloadButton")?.GetComponent<Button>();
            }
            if (optionsButton == null)
            {
                optionsButton = GameObject.Find("OptionsButton")?.GetComponent<Button>();
            }
        }

        UnityEngine.EventSystems.EventSystem eventSystem = FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>();
        if (eventSystem == null)
        {
            Debug.LogError("WinPageManager: EventSystem not found! Buttons will not work. Please add EventSystem to the scene.");
        }
        else
        {
            Debug.Log("WinPageManager: EventSystem found");
        }
        
        if (continueButton != null)
        {
            int listenerCount = continueButton.onClick.GetPersistentEventCount();
            bool isInteractable = continueButton.interactable;
            Debug.Log($"WinPageManager: Continue button found. Interactable: {isInteractable}, Persistent listeners: {listenerCount}");
            
            if (!isInteractable)
            {
                Debug.LogWarning("WinPageManager: Continue button is not interactable! Enabling it...");
                continueButton.interactable = true;
            }
            
            if (listenerCount == 0)
            {
                continueButton.onClick.AddListener(OnContinueClicked);
                Debug.Log("WinPageManager: Added Continue button listener programmatically");
            }
        }
        else
        {
            Debug.LogError("WinPageManager: Continue button not found! Please assign it in Inspector or name it 'ContinueButton'");
        }

        if (reloadButton != null)
        {
            int listenerCount = reloadButton.onClick.GetPersistentEventCount();
            bool isInteractable = reloadButton.interactable;
            Debug.Log($"WinPageManager: Reload button found. Interactable: {isInteractable}, Persistent listeners: {listenerCount}");
            
            if (!isInteractable)
            {
                Debug.LogWarning("WinPageManager: Reload button is not interactable! Enabling it...");
                reloadButton.interactable = true;
            }
            
            if (listenerCount == 0)
            {
                reloadButton.onClick.AddListener(OnReloadClicked);
                Debug.Log("WinPageManager: Added Reload button listener programmatically");
            }
        }
        else
        {
            Debug.LogError("WinPageManager: Reload button not found! Please assign it in Inspector or name it 'ReloadButton'");
        }

        if (optionsButton != null)
        {
            int listenerCount = optionsButton.onClick.GetPersistentEventCount();
            bool isInteractable = optionsButton.interactable;
            Debug.Log($"WinPageManager: Options button found. Interactable: {isInteractable}, Persistent listeners: {listenerCount}");
            
            if (!isInteractable)
            {
                Debug.LogWarning("WinPageManager: Options button is not interactable! Enabling it...");
                optionsButton.interactable = true;
            }
            
            if (listenerCount == 0)
            {
                optionsButton.onClick.AddListener(OnOptionsClicked);
                Debug.Log("WinPageManager: Added Options button listener programmatically");
            }
        }
        else
        {
            Debug.LogError("WinPageManager: Options button not found! Please assign it in Inspector or name it 'OptionsButton'");
        }
    }
    
    private void SetupOptionsPanel()
    {
        if (optionsPanel == null)
        {
            optionsPanel = GameObject.Find("OptionsPanel");
        }

        if (optionsPanel != null)
        {
            if (volumeSlider == null)
            {
                volumeSlider = optionsPanel.GetComponentInChildren<Slider>();
            }
            if (volumeText == null)
            {
                volumeText = optionsPanel.GetComponentInChildren<TextMeshProUGUI>();
            }

            if (volumeSlider != null && musicManager != null)
            {
                volumeSlider.value = musicManager.Volume;
                volumeSlider.onValueChanged.RemoveAllListeners();
                volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
                UpdateVolumeText(musicManager.Volume);
            }

            optionsPanel.SetActive(false);
        }
    }
    
    private void UpdateButtonStates()
    {
        if (sceneManager == null) return;

        if (continueButton != null)
        {
            int maxLevel = sceneManager.GetLevelCount() - 1;
            if (currentLevelIndex >= maxLevel)
            {
                // Это последний уровень - можно скрыть кнопку или изменить текст
                // Для простоты оставим кнопку, но она вернет в главное меню
            }
        }
    }
    
    private void UpdateVolumeText(float volume)
    {
        if (volumeText != null)
        {
            volumeText.text = $"Volume: {Mathf.RoundToInt(volume * 100)}%";
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
    
    public void OnContinueClicked()
    {
        Debug.Log("=== WinPageManager: OnContinueClicked() called! ===");
        
        if (sceneManager == null)
        {
            sceneManager = GameSceneManager.Instance;
            if (sceneManager == null)
            {
                Debug.LogError("WinPageManager: GameSceneManager.Instance is null! Trying to find it...");
                sceneManager = FindFirstObjectByType<GameSceneManager>();
            }
        }

        if (sceneManager == null)
        {
            Debug.LogError("WinPageManager: GameSceneManager not found! Cannot load next level.");
            return;
        }

        if (currentLevelIndex < 0)
        {
            currentLevelIndex = PlayerPrefs.GetInt("LastLevelIndex", 0);
        }

        int nextLevel = currentLevelIndex + 1;
        int maxLevel = sceneManager.GetLevelCount() - 1;

        Debug.Log($"WinPageManager: Continue clicked. Current level: {currentLevelIndex}, Next level: {nextLevel}, Max level: {maxLevel}");

        if (nextLevel <= maxLevel)
        {
            Debug.Log($"WinPageManager: Loading next level: {nextLevel}");
            sceneManager.LoadLevel(nextLevel);
        }
        else
        {
            Debug.Log($"WinPageManager: All levels completed! Cycling back to level 0.");
            sceneManager.LoadLevel(0);
        }
    }
    
    public void OnReloadClicked()
    {
        Debug.Log("=== WinPageManager: OnReloadClicked() called! ===");
        
        if (sceneManager == null)
        {
            sceneManager = GameSceneManager.Instance;
            if (sceneManager == null)
            {
                Debug.LogError("WinPageManager: GameSceneManager.Instance is null! Trying to find it...");
                sceneManager = FindFirstObjectByType<GameSceneManager>();
            }
        }

        if (sceneManager == null)
        {
            Debug.LogError("WinPageManager: GameSceneManager not found! Cannot reload level.");
            return;
        }

        if (currentLevelIndex < 0)
        {
            currentLevelIndex = PlayerPrefs.GetInt("LastLevelIndex", 0);
        }

        if (currentLevelIndex >= 0)
        {
            Debug.Log($"WinPageManager: Reload clicked. Reloading level {currentLevelIndex}");
            sceneManager.LoadLevel(currentLevelIndex);
        }
        else
        {
            Debug.LogWarning("WinPageManager: Could not determine current level. Loading level 0.");
            sceneManager.LoadLevel(0);
        }
    }
    
    public void OnOptionsClicked()
    {
        Debug.Log("WinPageManager: OnOptionsClicked() called!");
        
        if (optionsPanel == null)
        {
            optionsPanel = GameObject.Find("OptionsPanel");
        }

        if (optionsPanel != null)
        {
            bool isActive = optionsPanel.activeSelf;
            optionsPanel.SetActive(!isActive);
            Debug.Log($"WinPageManager: Options clicked. Panel is now {(isActive ? "closed" : "open")}");
        }
        else
        {
            Debug.LogWarning("WinPageManager: Options panel not found!");
        }
    }
}

