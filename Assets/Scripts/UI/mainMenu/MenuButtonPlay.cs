using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public int indexEscena = 1;

    public void OnPlayClick()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFXAndWait(AudioManager.Instance.sfx_play, () =>
            {
                SceneManager.LoadScene(indexEscena);
            });
        }
        else
        {
            SceneManager.LoadScene(indexEscena);
        }
    }
}

