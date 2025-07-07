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
        _animator.ResetTrigger("Hit"); // Limpia el trigger en caso de que est� activo
        _animator.SetTrigger("Hit");   // Vuelve a activarlo, reinicia la animaci�n
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerAttack") || other.CompareTag("Projectile"))
        {
            OnHit();
        }
    }
}
