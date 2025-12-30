using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class ButtonSceneLoader : MonoBehaviour
{
    [SerializeField] private SceneAsset sceneToLoad;
    [SerializeField] private string sceneNameToLoad = "";

    public void LoadScene()
    {
        string sceneName = "";

        if (sceneToLoad != null)
        {
            string scenePath = AssetDatabase.GetAssetPath(sceneToLoad);
            sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
        }
        else if (!string.IsNullOrEmpty(sceneNameToLoad))
        {
            sceneName = sceneNameToLoad;
        }

        if (!string.IsNullOrEmpty(sceneName))
        {
            GameSceneManager sceneManager = GameSceneManager.Instance;
            if (sceneManager != null)
            {
                if (sceneName == "main" || sceneName == "MainMenu")
                {
                    sceneManager.LoadMainMenu();
                }
                else if (sceneName.StartsWith("level"))
                {
                    string levelNum = sceneName.Replace("level", "");
                    if (int.TryParse(levelNum, out int levelIndex))
                    {
                        sceneManager.LoadLevel(levelIndex);
                    }
                    else
                    {
                        SceneManager.LoadScene(sceneName);
                    }
                }
                else
                {
                    SceneManager.LoadScene(sceneName);
                }
            }
            else
            {
                SceneManager.LoadScene(sceneName);
            }
        }
        else
        {
            Debug.LogWarning("Scene name is not set! Please assign scene in inspector.");
        }
    }
}