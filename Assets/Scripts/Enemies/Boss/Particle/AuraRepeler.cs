using UnityEngine;
using System.Collections;

public class AuraRepelente : MonoBehaviour
{
    public int da�o = 5;
    public float fuerzaKnockback = 20f;

    private bool puedeHacerDa�o = true;
    public float cooldownDa�o = 0.2f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!puedeHacerDa�o || !other.CompareTag("Player")) return;

        PlayerKnockback knockback = other.GetComponent<PlayerKnockback>();
        if (knockback != null)
        {
            Vector2 direccion = (other.transform.position - transform.position).normalized;
            knockback.RecibirKnockback(direccion, fuerzaKnockback, da�o);
        }

        StartCoroutine(EsperarCooldown());
    }

    private IEnumerator EsperarCooldown()
    {
        puedeHacerDa�o = false;
        yield return new WaitForSeconds(cooldownDa�o);
        puedeHacerDa�o = true;
    }
}
