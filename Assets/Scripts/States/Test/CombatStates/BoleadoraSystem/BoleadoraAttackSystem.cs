using UnityEngine;

public class BoleadoraAttackSystem : MonoBehaviour
{
    [Header("Configuración del Ataque")]
    [SerializeField] private GameObject boleadoraPrefab;
    [SerializeField] private int cantidadBoleadoras = 3;
    [SerializeField] private float velocidad = 7f;
    [SerializeField] private float fuerzaComba = 1f;
    [SerializeField] private float rangoDeteccion = 10f;
    [SerializeField] private LayerMask layerEnemigos;

    [Header("Recarga")]
    [SerializeField] private int maxCargas = 3;
    private int cargasActuales;
    
    private void Start()
    {
        cargasActuales = maxCargas;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && cargasActuales > 0)
        {
            LanzarBoleadoras();
            cargasActuales--;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Recargar();
        }
    }

    void LanzarBoleadoras()
    {
        Collider2D[] enemigos = Physics2D.OverlapCircleAll(transform.position, rangoDeteccion, layerEnemigos);

        for (int i = 0; i < cantidadBoleadoras; i++)
        {
            Transform objetivo = ObtenerEnemigoMasCercano(enemigos);

            if (objetivo != null)
            {
                GameObject boleadora = Instantiate(boleadoraPrefab, transform.position, Quaternion.identity);
                BoleadoraProjectile proj = boleadora.GetComponent<BoleadoraProjectile>();
                proj.Inicializar(objetivo, velocidad, fuerzaComba);
            }
        }
    }

    Transform ObtenerEnemigoMasCercano(Collider2D[] enemigos)
    {
        float distanciaMasCorta = Mathf.Infinity;
        Transform enemigoCercano = null;

        foreach (Collider2D enemigo in enemigos)
        {
            float dist = Vector2.Distance(transform.position, enemigo.transform.position);
            if (dist < distanciaMasCorta)
            {
                distanciaMasCorta = dist;
                enemigoCercano = enemigo.transform;
            }
        }
        return enemigoCercano;
    }

    void Recargar()
    {
        cargasActuales = maxCargas;
        Debug.Log("Boleadoras recargadas");
    }
}