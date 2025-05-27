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
        // Cuanto m�s bajo est� (menor Y), mayor ser� el Order
        sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }
}

