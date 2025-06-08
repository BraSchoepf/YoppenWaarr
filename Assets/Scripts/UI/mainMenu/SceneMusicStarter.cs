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
            AudioManager.Instance.PlayMusic(musicaDeEscena);
        }
    }
}
