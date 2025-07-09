using UnityEngine;

public class SparringDummy : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private GameObject damageParticlePrefab;

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
            SpawnDamageParticles();
        }
    }

    private void SpawnDamageParticles()
    {
        if (damageParticlePrefab != null)
        {
            // Instantiate the particle prefab at the enemy's position
            GameObject particles = Instantiate(damageParticlePrefab, transform.position, Quaternion.identity);
            // Optionally, you can set the particles to be destroyed after a certain time
            Destroy(particles, 1f); // Destroy after 1 second
        }
    }
}
