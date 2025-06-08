using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{
    public void OnReturnToMenu()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic(); // detenemos música de créditos si hay
        }

        SceneManager.LoadScene("MainMenu");
    }
}
