using UnityEngine;

public class PanelOpciones : MonoBehaviour
{
    [Header("Referencia al panel")]
    public GameObject panelOpcionesMañas;

    public void ActivarPanel()
    {
        panelOpcionesMañas.SetActive(true);
    }

    public void DesactivarPanel()
    {
        panelOpcionesMañas.SetActive(false);
    }
}
