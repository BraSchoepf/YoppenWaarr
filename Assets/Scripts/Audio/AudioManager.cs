using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    // singletons
    public static AudioManager Instance { get; private set; }

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
