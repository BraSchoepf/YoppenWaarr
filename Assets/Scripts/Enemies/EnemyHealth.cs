using UnityEngine;
using System.Collections;
using UnityEngine.AI; // control NavMeshAgent
using FMODUnity;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 6;
    private int _currentHealth;

    //effect for damage recive - for flashDamage()
    public SpriteRenderer spriteRenderer;
    private Color _originalColor;

    [SerializeField] private float _knockbackForce = 5f;
    [SerializeField] private float _knockbackDuration = 0.15f;
    private NavMeshAgent _navAgent;

    private EnemyController _controller;

    // Reference to the damage particle prefab
    [SerializeField] private GameObject damageParticlePrefab;

    private Animator _animator;

    private bool isDead = false; // Flag to ensure death animation only plays once

    public bool IsDead => isDead; // Expose isDead as a public property

    [SerializeField] private EventReference _damageSFX;

    [SerializeField] private GameObject _healthBarPrefab;
    private EnemyHealthBar _healthBarInstance;


    private void Start()
    {
        _currentHealth = _maxHealth;
        _originalColor  = spriteRenderer.color;
        _navAgent = GetComponent<NavMeshAgent>();
        _controller = GetComponent<EnemyController>();
        _animator = GetComponent<Animator>();

        _healthBarInstance = Instantiate(_healthBarPrefab, transform.position, Quaternion.identity).GetComponent<EnemyHealthBar>();
        _healthBarInstance.SetTarget(transform);
        _healthBarInstance.SetHealth(_currentHealth, _maxHealth);

    }

    public void TakeDamage(int amount, Vector2 knockbackDirection)
    {
        if (_controller != null) _controller.isMovementBlocked = true;

        _currentHealth -= amount;
        _healthBarInstance.SetHealth(_currentHealth, _maxHealth);
        Debug.Log("El enemigo recibe daño, vida restante:: " + _currentHealth);

        // Coroutina for flashDamage()
        StartCoroutine(FlashDamage());

        // Spawn damage particles
        SpawnDamageParticles();

        // Sonido de daño
        AudioManager.Instance.PlayOneShot(_damageSFX, transform.position);

        // Aplicar knockback
        StartCoroutine(SimulateKnockback(knockbackDirection.normalized, _knockbackDuration));

        if (_currentHealth <= 0 && !isDead)
        {
            Die();
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

    private IEnumerator SimulateKnockback(Vector2 direction, float duration)
    {
        if (_navAgent != null)
        {
            _navAgent.enabled = false; // Evita conflicto de movimiento
        }

        if (_controller != null)
        {
            _controller.StopAttack(); // Detener la animación de ataque
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            transform.position += (Vector3)(direction * _knockbackForce * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (_navAgent != null)
        {
            _navAgent.enabled = true;
        }

        if (_controller != null)
        {
            _controller.isMovementBlocked = false;
            _controller.ResumeAttack(); // Reanudar la animación de ataque
        }
    }
    private void Die()
    {
        if (isDead) return;

        Debug.Log("Death animation triggered.");
        isDead = true; // PRIMERO: detener toda lógica futura

        //if (_controller != null)
        //{
        //    _controller.StopAttack();
        //    _controller.StopAllCoroutines();
        //    _controller.enabled = false; // ESTA LÍNEA es la que soluciona TODO
        //}

        //if (_navAgent != null)
        //    _navAgent.enabled = false;

        //_animator.ResetTrigger("Attack");
        //_animator.SetBool("isAttacking", false);
        //_animator.SetBool("isWalking", false);

        _animator.SetTrigger("Die");

        StartCoroutine(DestroyAfterAnimation());

        if (_healthBarInstance != null)
            Destroy(_healthBarInstance.gameObject);

    }


    private IEnumerator DestroyAfterAnimation()
    {
        Debug.Log("Waiting for animation to finish.");

        // Wait for the animation to finish (adjust the time as needed)
        yield return new WaitForSeconds(0.7f); // Adjust the duration based on your animation length

        Debug.Log("Destroying enemy.");

        Destroy(gameObject);
    }

    //Coroutina for effect damage recive
    IEnumerator FlashDamage()
    {
        int flashCount = 2;
        float flashDuration = 0.1f;


        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = _originalColor;
            yield return new WaitForSeconds(flashDuration);
        }
    }
}
