using FMODUnity;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BossHealth : MonoBehaviour, IHittable
{
    public int vidaMaxima = 100;
    private int vidaActual;

    private BossManager bossManager;

    [SerializeField] private EventReference _damageSFX;

    // Reference to the damage particle prefab
    [SerializeField] private GameObject damageParticlePrefab;
    
    //effect for damage recive - for flashDamage()
    public SpriteRenderer spriteRenderer;
    private Color _originalColor;

    [SerializeField] private float _knockbackForce = 5f;
    [SerializeField] private float _knockbackDuration = 0.15f;
    private NavMeshAgent _navAgent;

    void Start()
    {
        vidaActual = vidaMaxima;
        bossManager = GetComponent<BossManager>();
        BossManager.Instance?.ActualizarBarra(vidaActual, vidaMaxima);
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white; // Valor seguro inicial

            // Guardamos color original en BossAI
            BossAI bossAI = GetComponent<BossAI>();
            if (bossAI != null)
            {
                bossAI.spriteRenderer = spriteRenderer;
                bossAI.originalColor = spriteRenderer.color;
            }

            _originalColor = spriteRenderer.color;
        }
    }

    public void TakeDamage(int amount, Vector2 knockbackDirection)
    {
        RecibirDaño(amount, knockbackDirection);
    }

    public void RecibirDaño(int cantidad, Vector2 knockbackDirection)
    {
        if (vidaActual <= 0) return;

        StartCoroutine(FlashDamage());
        SpawnDamageParticles();

        vidaActual = Mathf.Max(vidaActual - cantidad, 0);
        BossManager.Instance?.ActualizarBarra(vidaActual, vidaMaxima);

        // Sonido
        AudioManager.Instance.PlayOneShot(_damageSFX, transform.position);

        // Knockback
        StartCoroutine(SimulateKnockback(knockbackDirection, _knockbackDuration));

        Debug.Log("Boss recibe daño: " + cantidad + " → Vida actual: " + vidaActual);

        if (vidaActual <= 0)
        {
            Debug.Log("Llamando a Muerte() del boss");
            Muerte();
        }
    }

    private IEnumerator SimulateKnockback(Vector2 direction, float duration)
    {
        if (_navAgent != null)
            _navAgent.enabled = false;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            transform.position += (Vector3)(direction * _knockbackForce * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (_navAgent != null)
            _navAgent.enabled = true;
    }

    private void Muerte()
    {
        Debug.Log("¡Boss muerto!");
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Victory();
        }

        StartCoroutine(DestruirDespuesDe(2f)); // Esperar 2 segundos
    }

    private IEnumerator DestruirDespuesDe(float tiempo)
    {
        yield return new WaitForSecondsRealtime(tiempo);
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

