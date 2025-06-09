using UnityEngine;

public class MenuMusicStarter : MonoBehaviour
{
    void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.musica_menu);
        }
    }
}

