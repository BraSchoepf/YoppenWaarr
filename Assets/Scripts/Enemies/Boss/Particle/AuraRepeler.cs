using UnityEngine;
using System.Collections;

public class AuraRepelente : MonoBehaviour
{
    public int daño = 5;
    public float fuerzaKnockback = 20f;

    private bool puedeHacerDaño = true;
    public float cooldownDaño = 0.2f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!puedeHacerDaño || !other.CompareTag("Player")) return;

        PlayerKnockback knockback = other.GetComponent<PlayerKnockback>();
        if (knockback != null)
        {
            Vector2 direccion = (other.transform.position - transform.position).normalized;
            knockback.RecibirKnockback(direccion, fuerzaKnockback, daño);
        }

        StartCoroutine(EsperarCooldown());
    }

    private IEnumerator EsperarCooldown()
    {
        puedeHacerDaño = false;
        yield return new WaitForSeconds(cooldownDaño);
        puedeHacerDaño = true;
    }
}
