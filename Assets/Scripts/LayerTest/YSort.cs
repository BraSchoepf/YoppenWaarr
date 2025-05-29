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
        // The lower it is (minor Y), the higher the Order
        sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }
}

