using UnityEngine;
using FMODUnity;
//#TEST
public class FMODTester : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Probando sonido...");
        ////#TEST RuntimeManager.PlayOneShot("event:/sfx-steps");
        ////#TEST RuntimeManager.PlayOneShot("event:/sfx-lowLife");
        RuntimeManager.PlayOneShot("event:/music-scene_tutorial");
    }
}
