using UnityEngine;
using UnityEngine.UI;

public class BackgroundColorChanger : MonoBehaviour
{
    [SerializeField]
    private float colorChangeDelay = 1f;
    
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
        
        colors = new Color[]
        {
            new Color(1f, 0f, 0f, 1f),
            new Color(1f, 0.5f, 0f, 1f),
            new Color(1f, 1f, 0f, 1f),
            new Color(0f, 1f, 0f, 1f),
            new Color(0f, 1f, 1f, 1f),
            new Color(0f, 0f, 1f, 1f),
            new Color(0.5f, 0f, 1f, 1f),
            new Color(1f, 0.08f, 0.58f, 1f)
        };
        
        startColor = colors[0];
        targetColor = colors[1];
        backgroundImage.color = startColor;
    }
    
    void Update()
    {
        timer += Time.deltaTime;
        
        float t = timer / colorChangeDelay;
        
        if (t >= 1f)
        {
            timer = 0f;
            currentColorIndex = (currentColorIndex + 1) % colors.Length;
            startColor = backgroundImage.color;
            targetColor = colors[currentColorIndex];
            t = 0f;
        }
        
        backgroundImage.color = Color.Lerp(startColor, targetColor, t);
    }
}

