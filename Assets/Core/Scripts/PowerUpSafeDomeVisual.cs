using UnityEngine;

public class SafeDomePulse : MonoBehaviour
{
    public float pulseDuration = 0.5f; // скорость пульсации
    public float minAlpha = 0.2f; // минимальная прозрачность
    public float maxAlpha = 0.5f; // максимальная прозрачность
    public float lifeTime = 3f; // длительность купола

    private SpriteRenderer sr;
    private float timer = 0f;
    private float fadeTimer = 0f;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // уменьшаем lifeTime
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            // Пульсация перед исчезновением
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Lerp(minAlpha, maxAlpha, Mathf.PingPong(fadeTimer * 2f, 1f));
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;

            // Когда прошло еще полсекунды, уничтожаем купол
            if (fadeTimer >= pulseDuration)
                Destroy(gameObject);
        }
    }
}