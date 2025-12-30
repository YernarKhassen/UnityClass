using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameSceneManager : MonoBehaviour
{
    private static GameSceneManager _instance;
    public static GameSceneManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<GameSceneManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("GameSceneManager");
                    _instance = go.AddComponent<GameSceneManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

    [Header("Scene Names")]
    [SerializeField] private string mainMenuSceneName = "main";
    [SerializeField] private string winSceneName = "win";
    [SerializeField] private string[] levelSceneNames = { "level0", "level1", "level2" };

    [Header("Loading Settings")]
    [SerializeField] private bool useAsyncLoading = true;
    [SerializeField] private float minLoadingTime = 0.5f;

    private bool isLoading = false;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    public void LoadMainMenu()
    {
        if (isLoading) return;
        LoadScene(mainMenuSceneName);
    }
    
    public void LoadLevel(int levelIndex)
    {
        if (isLoading) return;

        if (levelIndex < 0 || levelIndex >= levelSceneNames.Length)
        {
            Debug.LogError($"Invalid level index: {levelIndex}. Available levels: 0-{levelSceneNames.Length - 1}");
            return;
        }

        LoadScene(levelSceneNames[levelIndex]);
    }
    
    public void LoadNextLevel()
    {
        if (isLoading) return;

        string currentSceneName = SceneManager.GetActiveScene().name;
        int currentLevelIndex = GetCurrentLevelIndex();

        if (currentLevelIndex >= 0)
        {
            int nextLevelIndex = currentLevelIndex + 1;
            if (nextLevelIndex < levelSceneNames.Length)
            {
                LoadLevel(nextLevelIndex);
            }
            else
            {
                Debug.Log("All levels completed! Returning to main menu.");
                LoadMainMenu();
            }
        }
        else
        {
            Debug.LogWarning("Current scene is not a level. Loading level 0.");
            LoadLevel(0);
        }
    }
    
    public void ReloadCurrentScene()
    {
        if (isLoading) return;
        LoadScene(SceneManager.GetActiveScene().name);
    }
    
    private void LoadScene(string sceneName)
    {
        if (isLoading)
        {
            Debug.LogWarning("Scene is already loading!");
            return;
        }

        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Scene name is null or empty!");
            return;
        }

        if (!SceneExists(sceneName))
        {
            Debug.LogError($"Scene '{sceneName}' not found in build settings!");
            return;
        }

        isLoading = true;

        if (useAsyncLoading)
        {
            StartCoroutine(LoadSceneAsync(sceneName));
        }
        else
        {
            SceneManager.LoadScene(sceneName);
            isLoading = false;
        }
    }
    
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        float startTime = Time.time;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        float elapsedTime = Time.time - startTime;
        if (elapsedTime < minLoadingTime)
        {
            yield return new WaitForSeconds(minLoadingTime - elapsedTime);
        }

        asyncLoad.allowSceneActivation = true;

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        if (!IsMainMenu())
        {
            yield return new WaitForSeconds(0.1f);
            PlayerManager playerManager = PlayerManager.Instance;
            if (playerManager != null)
            {
                playerManager.FindAllPlayers();
            }
        }

        isLoading = false;
    }
    
    private bool SceneExists(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneNameInBuild = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (sceneNameInBuild == sceneName)
            {
                return true;
            }
        }
        return false;
    }
    
    private int GetCurrentLevelIndex()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        for (int i = 0; i < levelSceneNames.Length; i++)
        {
            if (levelSceneNames[i] == currentSceneName)
            {
                return i;
            }
        }
        return -1;
    }

    public bool IsMainMenu()
    {
        return SceneManager.GetActiveScene().name == mainMenuSceneName;
    }
    
    public bool IsLevel()
    {
        return GetCurrentLevelIndex() >= 0;
    }
    
    public int GetCurrentLevel()
    {
        return GetCurrentLevelIndex();
    }
    
    public int GetLevelCount()
    {
        return levelSceneNames.Length;
    }
    
    public void LoadWinScene()
    {
        if (isLoading) return;
        LoadScene(winSceneName);
    }
    
    public bool IsWinScene()
    {
        return SceneManager.GetActiveScene().name == winSceneName;
    }
}

