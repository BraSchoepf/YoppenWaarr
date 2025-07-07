using UnityEngine;
using FMODUnity;

public class SceneMusicStarter : MonoBehaviour
{
    [Tooltip("Evento de m�sica a reproducir al iniciar la escena")]
    public EventReference musicaDeEscena;

    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic(); // <--- Detiene m�sica anterior si a�n suena
            AudioManager.Instance.PlayMusic(musicaDeEscena);
        }
    }
}
