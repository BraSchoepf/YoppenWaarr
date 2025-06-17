using UnityEngine;

public class BoleadoraProjectile : MonoBehaviour
{
    private Transform objetivo;
    private float velocidad;
    private float fuerzaComba;
    private float tiempo = 0f;
    private Vector3 posicionInicial;

    public void Inicializar(Transform objetivo, float velocidad, float fuerzaComba)
    {
        this.objetivo = objetivo;
        this.velocidad = velocidad;
        this.fuerzaComba = fuerzaComba;
        posicionInicial = transform.position;
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
        Vector3 trayectoriaCurva = trayectoriaRecta + perpendicular * Mathf.Sin(tiempo * Mathf.PI) * fuerzaComba;

        transform.position = trayectoriaCurva;

        if (Vector2.Distance(transform.position, objetivo.position) < 0.4f)
        {
            Impacto();
        }
    }

    void Impacto()
    {
        EnemyHealth enemyHealth = objetivo.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(2, (objetivo.position - transform.position).normalized);
        }
        Destroy(gameObject);
    }
}