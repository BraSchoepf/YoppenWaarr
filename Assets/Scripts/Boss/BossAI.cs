using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public float velocidadMovimiento = 2f;
    public float distanciaMinima = 1.5f;

    private bool activo = false;
    private Transform objetivoJugador;

    public void ActivarAtaque()
    {
        activo = true;
        // Acá podés activar animaciones, cambiar estado, etc.

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            objetivoJugador = player.transform;
        }
    }

    void Update()
    {
        if (!activo || objetivoJugador == null) return;

        float distancia = Vector2.Distance(transform.position, objetivoJugador.position);

        if (distancia > distanciaMinima)
        {
            Vector2 direccion = (objetivoJugador.position - transform.position).normalized;
            transform.position += (Vector3)direccion * velocidadMovimiento * Time.deltaTime; 
        }
        else
        {
            // Acá va el ataque o idle cerca
            // Debug.Log("Boss está en rango para atacar.");
        }
        // Lógica de movimiento o ataque
        // Ejemplo simple:
        //transform.position = Vector2.MoveTowards(transform.position, PlayerTarget(), 2f * Time.deltaTime);
    }

    private Vector2 PlayerTarget()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        return player != null ? player.transform.position : transform.position;
    }
}

