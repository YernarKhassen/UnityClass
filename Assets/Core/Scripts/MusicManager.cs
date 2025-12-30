using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    public static MusicManager Instance => instance;

    private AudioSource audioSource;

    private const string VOLUME_KEY = "MusicVolume";
    private float volume = 1f;

    public float Volume
    {
        get => volume;
        set
        {
            volume = Mathf.Clamp01(value);
            ApplyVolume();
            PlayerPrefs.SetFloat(VOLUME_KEY, volume);
            PlayerPrefs.Save();
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();

        volume = PlayerPrefs.GetFloat(VOLUME_KEY, 1f);
        ApplyVolume();
    }

    private void ApplyVolume()
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }
}