using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private SceneAsset gameScene;

    [SerializeField]
    private GameObject mainMenuPanel;

    [SerializeField]
    private GameObject optionsMenuPanel;

    public void StartGame()
    {
        if (gameScene != null)
        {
#if UNITY_EDITOR
            // В редакторе используем EditorSceneManager, который не требует Build Settings
            string scenePath = AssetDatabase.GetAssetPath(gameScene);
            EditorSceneManager.LoadSceneAsyncInPlayMode(scenePath, new LoadSceneParameters(LoadSceneMode.Single));
#else
            // В билде используем обычный SceneManager (требует Build Settings)
            SceneManager.LoadScene(gameScene.name);
#endif
        }
        else
        {
            Debug.LogWarning("Game Scene не назначена в MainMenuManager! Пожалуйста, назначьте сцену в Inspector.");
#if UNITY_EDITOR
            // Попытка загрузить level0 напрямую через путь
            string level0Path = "Assets/Scenes/level0.unity";
            if (System.IO.File.Exists(level0Path))
            {
                EditorSceneManager.LoadSceneAsyncInPlayMode(level0Path, new LoadSceneParameters(LoadSceneMode.Single));
            }
            else
            {
                Debug.LogError($"Сцена level0 не найдена по пути: {level0Path}");
            }
#else
            // В билде пытаемся загрузить по имени (требует Build Settings)
            SceneManager.LoadScene("level0");
#endif
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
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

