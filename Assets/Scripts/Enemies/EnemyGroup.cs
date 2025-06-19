using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    private int enemigosTotales;
    private int enemigosMuertos;
    private bool grupoDesactivado = false;

    void Start()
    {
        enemigosTotales = transform.childCount;
        Debug.Log($"{gameObject.name} tiene {enemigosTotales} enemigos."); // DEBUG inicial
    }

    public void NotificarMuerte()
    {
        if (grupoDesactivado) return;

        enemigosMuertos++;

        Debug.Log($"{gameObject.name}: enemigo muerto. Total: {enemigosMuertos}/{enemigosTotales}");

        if (enemigosMuertos >= enemigosTotales)
        {
            grupoDesactivado = true;
            Debug.Log($"{gameObject.name} eliminado. Se daña al boss.");

            BossManager.Instance?.ReducirVida(20); // o el valor que uses
        }
    }
}
