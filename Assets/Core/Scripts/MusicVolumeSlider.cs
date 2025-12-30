using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void Start()
    {
        if (MusicManager.Instance == null)
        {
            Debug.LogError("MusicManager not found in scene!");
            return;
        }

        slider.value = MusicManager.Instance.Volume;
        slider.onValueChanged.AddListener(OnVolumeChanged);
    }

    private void OnVolumeChanged(float value)
    {
        MusicManager.Instance.Volume = value;
    }
}