using UnityEngine;

public class YSort : MonoBehaviour
{
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        // Cuanto más bajo esté (menor Y), mayor será el Order
        sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }
}

