using UnityEngine;

public class LamparaCheckpoint : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Animator animador;
    [SerializeField] private GameObject luces2D;

    private bool activado = false;

    private void Start()
    {
        if (!activado)
        {
            if (animador != null)
                animador.SetBool("Enable", false);

            if (luces2D != null)
                luces2D.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activado) return;

        if (other.CompareTag("Player"))
        {
            activado = true;

            if (animador != null)
                animador.SetBool("Enable", true);

            if (luces2D != null)
                luces2D.SetActive(true);

            // Acá podrías notificar al sistema de checkpoints
            // GameManager.Instance.SetCheckpoint(transform.position);
        }
    }
}
