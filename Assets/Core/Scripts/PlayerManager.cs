using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager _instance;
    public static PlayerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<PlayerManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("PlayerManager");
                    _instance = go.AddComponent<PlayerManager>();
                }
            }
            return _instance;
        }
    }

    private HashSet<PlayerHealth> alivePlayers = new HashSet<PlayerHealth>();
    private bool isTransitioning = false;

    [Header("Settings")]
    [SerializeField] private float transitionDelay = 1.5f;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        FindAllPlayers();
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isTransitioning = false;
        alivePlayers.Clear();
        
        GameSceneManager sceneManager = GameSceneManager.Instance;
        if (sceneManager != null && !sceneManager.IsMainMenu())
        {
            Invoke(nameof(FindAllPlayers), 0.1f);
        }
    }

    public void FindAllPlayers()
    {
        alivePlayers.Clear();
        PlayerHealth[] players = FindObjectsByType<PlayerHealth>(FindObjectsSortMode.None);
        
        foreach (PlayerHealth player in players)
        {
            RegisterPlayer(player);
        }

        Debug.Log($"PlayerManager: Found {alivePlayers.Count} player(s) on scene");
    }
    
    public void RegisterPlayer(PlayerHealth player)
    {
        if (player != null && !alivePlayers.Contains(player))
        {
            alivePlayers.Add(player);
            Debug.Log($"PlayerManager: Registered player {player.gameObject.name}");
        }
    }

    public void OnPlayerDied(PlayerHealth deadPlayer)
    {
        if (isTransitioning) return;

        GameSceneManager sceneManager = GameSceneManager.Instance;
        if (sceneManager != null && sceneManager.IsMainMenu())
        {
            return;
        }

        if (alivePlayers.Contains(deadPlayer))
        {
            alivePlayers.Remove(deadPlayer);
            Debug.Log($"PlayerManager: Player {deadPlayer.gameObject.name} died. Remaining players: {alivePlayers.Count}");
        }

        StartCoroutine(TransitionToNextLevel());
    }
    
    private System.Collections.IEnumerator TransitionToNextLevel()
    {
        if (isTransitioning) yield break;
        
        isTransitioning = true;
        Debug.Log("PlayerManager: Player died, loading win scene...");

        GameSceneManager sceneManager = GameSceneManager.Instance;
        if (sceneManager != null)
        {
            int currentLevel = sceneManager.GetCurrentLevel();
            if (currentLevel >= 0)
            {
                PlayerPrefs.SetInt("LastLevelIndex", currentLevel);
                PlayerPrefs.Save();
            }
        }

        yield return new WaitForSeconds(transitionDelay);

        if (sceneManager != null)
        {
            sceneManager.LoadWinScene();
        }
        else
        {
            Debug.LogError("PlayerManager: GameSceneManager not found!");
        }

        isTransitioning = false;
    }
    
    public int GetAlivePlayerCount()
    {
        alivePlayers.RemoveWhere(player => player == null);
        return alivePlayers.Count;
    }
    
    public bool HasAlivePlayers()
    {
        return GetAlivePlayerCount() > 0;
    }
}

