using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 1.5f, 0); // смещение над головой
    public Slider slider;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        if (!target || !cam) return;

        // Переводим мировую позицию игрока в экранную
        Vector3 screenPos = cam.WorldToScreenPoint(target.position + offset);

        // Обновляем позицию UI
        transform.position = screenPos;
    }

    public void SetMaxValue(int value)
    {
        slider.maxValue = value;
        slider.value = value;
    }

    public void SetValue(int value)
    {
        slider.value = value;
    }
}