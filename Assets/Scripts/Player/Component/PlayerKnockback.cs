using UnityEngine;
using System.Collections;

public class PlayerKnockback : MonoBehaviour
{
    private bool enKnockback = false;
    private PlayerHealth health;
    private PlayerController movimiento; // si ten�s un script de movimiento propio

    [SerializeField] private float duracion = 0.2f;
    [SerializeField] private float fuerza = 6f;

    private void Awake()
    {
        health = GetComponent<PlayerHealth>();
        movimiento = GetComponent<PlayerController>();
    }

    public void RecibirKnockback(Vector2 direccion, float fuerzaKnockback, int da�o)
    {
        if (!enKnockback)
        {
            StartCoroutine(DoKnockback(direccion.normalized, fuerzaKnockback, da�o));
        }
    }

    private IEnumerator DoKnockback(Vector2 direccion, float fuerzaKnockback, int da�o)
    {
        enKnockback = true;

        if (movimiento != null)
            movimiento.enabled = false;

        float elapsed = 0f;
        while (elapsed < duracion)
        {
            transform.position += (Vector3)(direccion * fuerzaKnockback * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (health != null)
            health.TakeDamage(da�o);

        if (movimiento != null)
            movimiento.enabled = true;

        enKnockback = false;
    }
}