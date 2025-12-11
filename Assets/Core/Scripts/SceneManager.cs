using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class ButtonSceneLoader : MonoBehaviour
{
    [SerializeField] private SceneAsset sceneToLoad;

    public void LoadScene()
    {
        if (sceneToLoad != null)
        {
            string scenePath = AssetDatabase.GetAssetPath(sceneToLoad);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("select the scene in inspector!");
        }
    }
}