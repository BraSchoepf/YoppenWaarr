using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections;


public class AudioManager : MonoBehaviour
{
    // singletons
    public static AudioManager Instance { get; private set; }

    // Música actual
    private EventInstance musicInstance;

    // Variables públicas para asignar en inspector
    [Header("Música")]
    public EventReference musica_menu;
    public EventReference musica_pre_escenaTutorial;
    public EventReference musica_escenaTutorial;
    public EventReference musica_escenaCreditos;
    public EventReference musica_escenaOpciones;
    //public EventReference musica_boss;
    //public EventReference musica_gameOver;
    //public EventReference musica_victory;

    [Header("SFX")]
    public EventReference sfx_botonHover;
    public EventReference sfx_play;
    public EventReference sfx_dog;
    public EventReference sfx_farm;
    public EventReference sfx_lowLife;
    public EventReference sfx_powerUp;

    private void Awake()
    {
        if(Instance != null) 
        { 
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // reproducir música
    public void PlayMusic(EventReference musicEvent)
    {
        StopMusic();

        if (musicEvent.IsNull)
        {
            Debug.LogWarning("Referencia de música nula");
            return;
        }

        musicInstance = RuntimeManager.CreateInstance(musicEvent);
        musicInstance.start();
    }

    // detener la música
    public void StopMusic()
    {
        if (musicInstance.isValid())
        {
            musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

            musicInstance.release();
            musicInstance.clearHandle(); // opcional pero recomendable
        }
    }


    public void PlaySFX(EventReference sfxEvent)
    {
        if (sfxEvent.IsNull)
        {
            Debug.LogWarning("SFX vacío o no asignado.");
            return;
        }

        RuntimeManager.PlayOneShot(sfxEvent);
    }

    public void PlaySFXAndWait(EventReference sfxEvent, System.Action onComplete)
    {
        StartCoroutine(SFXWaitCoroutine(sfxEvent, onComplete));
    }

    private IEnumerator SFXWaitCoroutine(EventReference sfxEvent, System.Action onComplete)
    {
        if (sfxEvent.IsNull)
        {
            Debug.LogWarning("SFX no asignado.");
            onComplete?.Invoke();
            yield break;
        }

        StopMusic();

        EventInstance instance = RuntimeManager.CreateInstance(sfxEvent);
        instance.start();

        PLAYBACK_STATE state;
        do
        {
            yield return null;
            instance.getPlaybackState(out state);
        } while (state == PLAYBACK_STATE.PLAYING);

        instance.release();
        instance.clearHandle();

        onComplete?.Invoke();
    }

    //

    public void PlayOneShot(EventReference soundEvent, Vector3 position)
    {
        if (soundEvent.IsNull)
        {
            Debug.LogWarning("El EventReference es nulo."); 
            return;
        }
        RuntimeManager.PlayOneShot(soundEvent, position);
    }

    public void PlayStepWithMaterial(EventReference stepEvent, float materialLabel, Vector3 position)
    {
        EventInstance instance = RuntimeManager.CreateInstance(stepEvent);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        instance.setParameterByName("Material", materialLabel); // sand, wood
        instance.start();
        instance.release();
    }


}
