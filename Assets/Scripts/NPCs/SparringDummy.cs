using UnityEngine;

public class SparringDummy : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnHit()
    {
        _animator.ResetTrigger("Hit"); // Limpia el trigger en caso de que esté activo
        _animator.SetTrigger("Hit");   // Vuelve a activarlo, reinicia la animación
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerAttack") || other.CompareTag("Projectile"))
        {
            OnHit();
        }
    }
}
