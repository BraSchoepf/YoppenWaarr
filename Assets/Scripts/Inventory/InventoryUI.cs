using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject panelInventario;

    private bool inventarioAbierto = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventarioAbierto = !inventarioAbierto;

            panelInventario.SetActive(inventarioAbierto);

            // Pausa o reanuda el juego
            Time.timeScale = inventarioAbierto ? 0f : 1f;
        }
    }
}
