using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    [Header("Globos de boleadoras")]
    public GameObject globoI;
    public GameObject globoII;
    public GameObject globoIII;

    [Header("Inventario")]
    public GameObject panelInventary;
    private bool _inventaryOpen = false;

    [Header("Mapa")]
    public GameObject panelMap;
    private bool _MapOpen = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    void Update()
    {
        OpenInventory();

        OpenMap();
    }

    public void ActualizarBoleadoras(int cantidad)
    {
        // Clamp entre 0 y 3 para evitar errores
        cantidad = Mathf.Clamp(cantidad, 0, 3);

        // Apaga todos primero
        globoI.SetActive(false);
        globoII.SetActive(false);
        globoIII.SetActive(false);

        //Encender solo el que corresponda
        if (cantidad == 1) globoI.SetActive(true);
        else if (cantidad == 2) globoII.SetActive(true);
        else if (cantidad == 3) globoIII.SetActive(true);
        // Si cantidad == 0 → todos apagados
    }

    private void OpenMap()
    {
        if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown("joystick button 4"))
        {
            _MapOpen = !_MapOpen;

            panelMap.SetActive(_MapOpen);
            // Pausa o reanuda el juego
            Time.timeScale = _MapOpen ? 0f : 1f;
        }
    }

    private void OpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown("joystick button 5"))
        {
            _inventaryOpen = !_inventaryOpen;

            panelInventary.SetActive(_inventaryOpen);

            // Pausa o reanuda el juego
            Time.timeScale = _inventaryOpen ? 0f : 1f;
        }
    }
}
