using UnityEngine;
using FMODUnity;

public class PuertaBossController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D bloqueoCollider;
    [SerializeField] private GameObject healthBoss;

    private bool activado = false;

    private void Start()
    {
        if (healthBoss != null) healthBoss.SetActive(false);
    }
    public void AbrirPorton()
    {
        if (activado) return;
        activado = true;

        if (bloqueoCollider != null)
        {
            bloqueoCollider.enabled = false;
            //  bloqueoCollider.gameObject.SetActive(false); <- apaga el objeto entero
        }

        AudioManager.Instance.StopMusic();

        //RuntimeManager.PlayOneShot(sfxCañon, transform.position);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.sfx_canon);
        animator.SetTrigger("Abrir");

        

        AudioManager.Instance.PlayMusic(AudioManager.Instance.musica_boss);
        if (healthBoss != null) healthBoss.SetActive(true);

    }
}

