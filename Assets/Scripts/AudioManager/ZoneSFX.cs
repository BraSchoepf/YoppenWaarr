using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;

public class ZoneSFX : MonoBehaviour
{
    //cada ZoneSFX se registre automáticamente en una lista al activarse.
    public static List<ZoneSFX> zonasActivas = new List<ZoneSFX>(); 
    public enum TipoSFX { Granja, Perro, Buo, Fogata}
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

            zonasActivas.Add(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (sonidoActivo && collision.CompareTag("Player"))
        {
            instancia.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            instancia.release();
            sonidoActivo = false;

            zonasActivas.Remove(this);
        }
    }

    private EventReference GetReferencia()
    {
        if (tipo == TipoSFX.Granja) return AudioManager.Instance.sfx_farm;
        if (tipo == TipoSFX.Perro) return AudioManager.Instance.sfx_dog;
        if (tipo == TipoSFX.Buo) return AudioManager.Instance.sfx_buo;
        if (tipo == TipoSFX.Fogata) return AudioManager.Instance.sfx_fogata;
        return default;
    }

    public void PausarSfx()
    {
        if (sonidoActivo && instancia.isValid())
        {
            instancia.setPaused(true);
        }
    }

    public void ReanudarSfx()
    {
        if (sonidoActivo && instancia.isValid())
        {
            instancia.setPaused(false);
        }
    }
}