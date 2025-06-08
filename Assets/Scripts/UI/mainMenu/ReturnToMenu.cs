using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{
    public void OnReturnToMenu()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic(); // detenemos m�sica de cr�ditos si hay
        }

        SceneManager.LoadScene("MainMenu");
    }
}
