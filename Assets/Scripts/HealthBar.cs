using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 1f, 0);
    public Slider slider;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;

        if (slider == null)
            slider = GetComponentInChildren<Slider>();
    }

    private void LateUpdate()
    {
        if (!target || !cam) return;

        Vector3 screenPos = cam.WorldToScreenPoint(target.position + offset);
        transform.position = screenPos;
    }

    public void SetMaxValue(int value)
    {
        if (slider != null)
        {
            slider.maxValue = value;
            slider.value = value;
        }
    }

    public void SetValue(int value)
    {
        if (slider != null)
            slider.value = value;
    }
}