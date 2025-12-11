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
            string scenePath = AssetDatabase.GetAssetPath(gameScene);
            EditorSceneManager.LoadSceneAsyncInPlayMode(scenePath, new LoadSceneParameters(LoadSceneMode.Single));
        }
        else
        {
            Debug.LogWarning("Game Scene is not assigned in MainMenuManager! Please assign the scene in Inspector.");

            string level0Path = "Assets/Scenes/level0.unity";
            if (System.IO.File.Exists(level0Path))
            {
                EditorSceneManager.LoadSceneAsyncInPlayMode(level0Path, new LoadSceneParameters(LoadSceneMode.Single));
            }
            else
            {
                Debug.LogError($"Scene level0 not found on the path: {level0Path}");
            }
            SceneManager.LoadScene("level0");
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

