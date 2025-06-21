
using System.Security.Cryptography;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class PlayerSlashHitbox : MonoBehaviour
{
    [Header("Attack Hitboxes")]
    [SerializeField] private int damage = 1;
    [SerializeField] private Collider2D hitboxCollider;

    [SerializeField] private FMODUnity.EventReference _swordSFX;

    private void Awake()
    {
        if (hitboxCollider != null)
            hitboxCollider.enabled = false;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hitboxCollider == null || !hitboxCollider.enabled) return;

        if (((1 << other.gameObject.layer) & LayerMask.GetMask("Enemys")) != 0)
        {
            Vector2 knockbackDir = ((Vector2)other.transform.position - (Vector2)transform.position).normalized;

            if (other.TryGetComponent<IHittable>(out var hittable))
            {
                hittable.TakeDamage(damage, knockbackDir);
            }
            else if (other.TryGetComponent<BossHealth>(out var boss))
            {
                boss.RecibirDaño(damage, knockbackDir);
            }
        }
    }

    public void EnableHitbox()
    {
        if (hitboxCollider != null)
            hitboxCollider.enabled = true;
        AudioManager.Instance.PlayOneShot(_swordSFX, transform.position);
    }

    //Evento: se llama al finalizar el golpe
    public void DisableHitbox()
    {

       
        if (hitboxCollider != null)
            hitboxCollider.enabled = false;
    }
}

