using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class ZoneSFX : MonoBehaviour
{
    public enum TipoSFX { Granja, Perro }
    public TipoSFX tipo;

    private EventInstance instancia;
    private bool sonidoActivo = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!sonidoActivo && collision.CompareTag("Player"))
        {
            EventReference referencia = GetReferencia();
            instancia = RuntimeManager.CreateInstance(referencia);
            instancia.start();
            sonidoActivo = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (sonidoActivo && collision.CompareTag("Player"))
        {
            instancia.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            instancia.release();
            sonidoActivo = false;
        }
    }

    private EventReference GetReferencia()
    {
        if (tipo == TipoSFX.Granja) return AudioManager.Instance.sfx_farm;
        if (tipo == TipoSFX.Perro) return AudioManager.Instance.sfx_dog;
        return default;
    }
}