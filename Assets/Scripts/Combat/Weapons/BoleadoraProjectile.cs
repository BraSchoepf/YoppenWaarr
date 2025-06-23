using UnityEngine;

public class BoleadoraProjectile : MonoBehaviour
{
    private Transform objetivo;
    private float velocidad;
    private float fuerzaComba;
    private float tiempo = 0f;
    private Vector3 posicionInicial;
    private float direccionComba;

    private int daño; // daño que viene desde el attacksystem

    // iniciador recibe el parametro de daño
    public void Inicializar(Transform objetivo, float velocidad, float fuerzaCombaBase, int daño)
    {
        this.objetivo = objetivo;
        this.velocidad = velocidad;
        this.daño = daño; // aca mismo
        posicionInicial = transform.position;

        direccionComba = Random.Range(0, 2) == 0 ? 1f : -1f;
        fuerzaComba = fuerzaCombaBase * Random.Range(0.5f, 1.5f);
    }

    private void Update()
    {
        if (objetivo == null)
        {
            Destroy(gameObject);
            return;
        }

        tiempo += Time.deltaTime * velocidad;

        Vector3 trayectoriaRecta = Vector3.Lerp(posicionInicial, objetivo.position, tiempo);
        Vector3 perpendicular = Vector3.Cross((objetivo.position - posicionInicial).normalized, Vector3.forward);
        Vector3 trayectoriaCurva = trayectoriaRecta + perpendicular * Mathf.Sin(tiempo * Mathf.PI) * fuerzaComba * direccionComba;

        transform.position = trayectoriaCurva;

        if (Vector2.Distance(transform.position, objetivo.position) < 0.4f)
        {
            Impacto();
        }
    }

    void Impacto()
    {
        ParticleSystem particulas = GetComponentInChildren<ParticleSystem>();
        if (particulas != null)
        {
            particulas.transform.parent = null;
            particulas.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            Destroy(particulas.gameObject, particulas.main.startLifetime.constantMax);
        }

        EnemyHealth enemyHealth = objetivo.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(daño, (objetivo.position - transform.position).normalized);
        }
        Destroy(gameObject);
    }
}