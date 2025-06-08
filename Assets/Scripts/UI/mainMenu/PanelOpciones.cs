using UnityEngine;

public class PanelOpciones : MonoBehaviour
{
    [Header("Referencia al panel")]
    public GameObject panelOpcionesMa�as;

    public void ActivarPanel()
    {
        panelOpcionesMa�as.SetActive(true);
    }

    public void DesactivarPanel()
    {
        panelOpcionesMa�as.SetActive(false);
    }
}
