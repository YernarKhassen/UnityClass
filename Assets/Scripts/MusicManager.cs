using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    private AudioSource audioSource;
    
    private const string VOLUME_KEY = "MusicVolume";
    private float volume = 1f;

    public static MusicManager Instance => instance;
    
    public float Volume
    {
        get => volume;
        set
        {
            volume = Mathf.Clamp01(value);
            if (audioSource != null)
            {
                audioSource.volume = volume;
            }
            PlayerPrefs.SetFloat(VOLUME_KEY, volume);
            PlayerPrefs.Save();
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            
            if (PlayerPrefs.HasKey(VOLUME_KEY))
            {
                volume = PlayerPrefs.GetFloat(VOLUME_KEY);
            }
            else
            {
                volume = 1f;
            }
            
            if (audioSource != null)
            {
                audioSource.volume = volume;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}