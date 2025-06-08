using UnityEngine;
using FMODUnity;
using FMOD.Studio;


public class AudioManager : MonoBehaviour
{
    // singletons
    public static AudioManager Instance { get; private set; }

    // M�sica actual
    private EventInstance musicInstance;

    // Variables p�blicas para asignar en inspector
    [Header("M�sica")]
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

    // reproducir m�sica
    public void PlayMusic(EventReference musicEvent)
    {
        StopMusic();

        if (musicEvent.IsNull)
        {
            Debug.LogWarning("Referencia de m�sica nula");
            return;
        }

        musicInstance = RuntimeManager.CreateInstance(musicEvent);
        musicInstance.start();
    }

    // detener la m�sica
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
            Debug.LogWarning("SFX vac�o o no asignado.");
            return;
        }

        RuntimeManager.PlayOneShot(sfxEvent);
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
