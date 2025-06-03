using UnityEngine;
using System.Collections;
using UnityEngine.AI; // control NavMeshAgent

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


    private void Start()
    {
        _currentHealth = _maxHealth;
        _originalColor  = spriteRenderer.color;
        _navAgent = GetComponent<NavMeshAgent>();
        _controller = GetComponent<EnemyController>();
    }

    public void TakeDamage(int amount, Vector2 knockbackDirection)
    {
        if (_controller != null) _controller.isMovementBlocked = true;

        _currentHealth -= amount;
        Debug.Log("El enemigo recibe daño, vida restante:: " + _currentHealth);

        // Coroutina for flashDamage()
        StartCoroutine(FlashDamage());

        // Aplicar knockback
        StartCoroutine(SimulateKnockback(knockbackDirection.normalized, _knockbackDuration));

        if (_currentHealth <= 0)
        {
            Die();
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
        Debug.Log("ENEMY DOWN, COUNTER-TERRORIST WIN.");
        // I leave this space to add animations at a later date
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
