using UnityEngine;
using UnityEngine.UI;

public class BackgroundColorChanger : MonoBehaviour
{
    [SerializeField]
    private float colorChangeDelay = 1f; // Задержка между сменами цветов в секундах
    
    private Image backgroundImage;
    private Color[] colors;
    private int currentColorIndex = 0;
    private float timer = 0f;
    private Color startColor;
    private Color targetColor;
    
    void Start()
    {
        backgroundImage = GetComponent<Image>();
        
        if (backgroundImage == null)
        {
            Debug.LogError("BackgroundColorChanger: Image component not found!");
            enabled = false;
            return;
        }
        
        // Цвета радуги + ярко-розовый
        colors = new Color[]
        {
            new Color(1f, 0f, 0f, 1f),        // Красный
            new Color(1f, 0.5f, 0f, 1f),      // Оранжевый
            new Color(1f, 1f, 0f, 1f),        // Желтый
            new Color(0f, 1f, 0f, 1f),       // Зеленый
            new Color(0f, 1f, 1f, 1f),       // Голубой (Cyan)
            new Color(0f, 0f, 1f, 1f),       // Синий
            new Color(0.5f, 0f, 1f, 1f),     // Фиолетовый
            new Color(1f, 0.08f, 0.58f, 1f)  // Ярко-розовый (Hot Pink)
        };
        
        // Устанавливаем начальный цвет
        startColor = colors[0];
        targetColor = colors[1];
        backgroundImage.color = startColor;
    }
    
    void Update()
    {
        timer += Time.deltaTime;
        
        // Плавный переход между цветами
        float t = timer / colorChangeDelay;
        
        if (t >= 1f)
        {
            // Переход завершен, переходим к следующему цвету
            timer = 0f;
            currentColorIndex = (currentColorIndex + 1) % colors.Length;
            startColor = backgroundImage.color;
            targetColor = colors[currentColorIndex];
            t = 0f;
        }
        
        // Плавная интерполяция между цветами
        backgroundImage.color = Color.Lerp(startColor, targetColor, t);
    }
}

