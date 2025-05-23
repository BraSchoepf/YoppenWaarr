using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public void ExitGame()
    {
        Debug.Log("Saliendo del juego");
        Application.Quit();
    }
}
