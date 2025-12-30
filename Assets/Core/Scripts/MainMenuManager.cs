using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenuPanel;

    [SerializeField]
    private GameObject optionsMenuPanel;

    [Header("Level Selection")]
    [SerializeField] private int startLevelIndex = 0;

    private GameSceneManager sceneManager;

    private void Start()
    {
        sceneManager = GameSceneManager.Instance;
    }
    public void StartGame()
    {
        if (sceneManager != null)
        {
            sceneManager.LoadLevel(startLevelIndex);
        }
        else
        {
            Debug.LogError("GameSceneManager not found! Falling back to direct scene load.");
            SceneManager.LoadScene("level0");
        }
    }
    
    public void LoadLevel(int levelIndex)
    {
        if (sceneManager != null)
        {
            sceneManager.LoadLevel(levelIndex);
        }
        else
        {
            Debug.LogError("GameSceneManager not found!");
        }
    }

    public void OpenOptions()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);
        
        if (optionsMenuPanel != null)
            optionsMenuPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        if (optionsMenuPanel != null)
            optionsMenuPanel.SetActive(false);
        
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        EditorApplication.isPlaying = false;
    }
}

