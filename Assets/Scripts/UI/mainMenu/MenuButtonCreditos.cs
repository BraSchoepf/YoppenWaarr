using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonCreditos : MonoBehaviour
{
    public int indexEscenaFogon = 3;

    public void IrAFogon()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
        }

        SceneManager.LoadScene(indexEscenaFogon);
    }
}
