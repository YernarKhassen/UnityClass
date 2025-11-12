using UnityEngine;

public class BombManager : MonoBehaviour
{
    public static BombManager Instance;
    public GameObject bombPrefab;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PlaceBomb(Vector3 position)
    {
        Instantiate(bombPrefab, position, Quaternion.identity);
    }
}