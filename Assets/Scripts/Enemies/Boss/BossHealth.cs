using FMODUnity;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BossHealth : MonoBehaviour, IHittable
{
    public int vidaMaxima = 100;
    private int vidaActual;
    public bool esInvulnerable = false;

    [SerializeField] private EventReference _damageSFX;
    [SerializeField] private GameObject damageParticlePrefab;
    [SerializeField] private float _knockbackForce = 5f;
    [SerializeField] private float _knockbackDuration = 0.15f;

    public SpriteRenderer spriteRenderer;
    private Color _originalColor;
    private NavMeshAgent _navAgent;

    //private BossManager bossManager;
    //private BossAI bossAI;
    //private bool bossActivado = false;
    void Start()
    {
        vidaActual = vidaMaxima;
        _navAgent = GetComponent<NavMeshAgent>();
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
       if (vidaActual <= 0 || esInvulnerable) return;

        StartCoroutine(FlashDamage());
        SpawnDamageParticles();
        AudioManager.Instance.PlayOneShot(_damageSFX, transform.position);
        StartCoroutine(SimulateKnockback(knockbackDirection, _knockbackDuration));

        vidaActual = Mathf.Max(vidaActual - amount, 0);
        BossManager.Instance?.ActualizarBarra(vidaActual, vidaMaxima);

        if (vidaActual <= 0)
        {
            Muerte();
        }
    }

    public void TakePureDamage(int amount)
    {
        if (vidaActual <= 0 || esInvulnerable) return;

        vidaActual = Mathf.Max(vidaActual - amount, 0);
        BossManager.Instance?.ActualizarBarra(vidaActual, vidaMaxima);

        if (vidaActual <= 0)
        {
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

        // Notify QuestManager
        QuestManager questManager = FindFirstObjectByType<QuestManager>();
        if (questManager != null)
        {
            questManager.CompleteObjective("xalpen");
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.Victory();
        }

        StartCoroutine(DestruirDespuesDe(0.2f)); // Esperar 2 segundos
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

    public void EnableDamage() => esInvulnerable = false;
    public void DisableDamage() => esInvulnerable = true;
}