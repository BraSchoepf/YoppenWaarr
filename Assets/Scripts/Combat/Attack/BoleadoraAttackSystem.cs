using System.Collections;
using UnityEngine;

public class BoleadoraAttackSystem : MonoBehaviour
{
    [Header("Configuración del Ataque")]
    [SerializeField] private GameObject boleadoraPrefab;
    [SerializeField] private int cantidadBoleadoras = 3;
    [SerializeField] private float velocidad = 5f;
    [SerializeField] private float fuerzaComba = 5f;
    [SerializeField] private float rangoDeteccion = 20f;
    [SerializeField] private LayerMask layerEnemigos;
    [SerializeField] private float tiempoEntreBoleadoras = 0.1f;
    [SerializeField] private int daño = 2; // configuracion desde el inspector ;)

    [Header("Recarga")]
    [SerializeField] private int maxCargas = 3;
    private int cargasActuales;

    private void Start()
    {
        //cargasActuales = maxCargas;
        //HUDManager.Instance.ActualizarBoleadoras(cargasActuales);
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsInDialogue)
            return;

        if ((Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown("joystick button 1")) && GameManager.Instance.GetAmountBoleadoras() > 0)
        {
            LanzarBoleadoras();
            GameManager.Instance.RemoveBoleadora();
        }

        //TEST
        if (Input.GetKeyDown(KeyCode.T) || Input.GetKeyDown("joystick button 2") )
        {
            Recargar();
        }
    }

    void LanzarBoleadoras()
    {
        Collider2D[] enemigos = Physics2D.OverlapCircleAll(transform.position, rangoDeteccion, layerEnemigos);

        int boleadorasAInstanciar = Mathf.Min(cantidadBoleadoras, enemigos.Length);

        StartCoroutine(DispararSecuencialmente(enemigos, boleadorasAInstanciar));
    }

    private IEnumerator DispararSecuencialmente(Collider2D[] enemigos, int cantidad)
    {
        for (int i = 0; i < cantidad; i++)
        {
            Transform objetivo = enemigos[i].transform;

            GameObject boleadora = Instantiate(boleadoraPrefab, transform.position, Quaternion.identity);
            BoleadoraProjectile proj = boleadora.GetComponent<BoleadoraProjectile>();
            proj.Inicializar(objetivo, velocidad, fuerzaComba, daño); // aca recibe el daño pero se cambia desde el inspector, Brian n.n

            yield return new WaitForSeconds(tiempoEntreBoleadoras);
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

    //TEST
    void Recargar()
    {
        cargasActuales = maxCargas;
        HUDManager.Instance.ActualizarBoleadoras(cargasActuales);
        Debug.Log("Boleadoras recargadas");
    }
}