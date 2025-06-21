using UnityEngine;

public interface IHittable
{
    void TakeDamage (int amount, Vector2 knockbackDirection);
}
