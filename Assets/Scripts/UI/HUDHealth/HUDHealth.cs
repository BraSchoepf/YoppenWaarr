using UnityEngine;

public class HUDVida : MonoBehaviour
{
    [System.Serializable]
    public class VidaSlot
    {
        public GameObject vidaCompleta;
        public GameObject vidaRota;
    }

    public VidaSlot[] slotsDeVida; // tamaño 5
    private const int vidaMaxima = 100;
    private const int vidaPorSlot = 20;

    public void ActualizarVida(int vidaActual)
    {
        for (int i = 0; i < slotsDeVida.Length; i++)
        {
            int vidaSlotMin = i * vidaPorSlot;
            int vidaSlotMax = vidaSlotMin + vidaPorSlot;

            var slot = slotsDeVida[i];

            if (vidaActual >= vidaSlotMax)
            {
                slot.vidaCompleta.SetActive(true);
                slot.vidaRota.SetActive(false);
            }
            else if (vidaActual >= vidaSlotMin + (vidaPorSlot / 2))
            {
                slot.vidaCompleta.SetActive(false);
                slot.vidaRota.SetActive(true);
            }
            else
            {
                slot.vidaCompleta.SetActive(false);
                slot.vidaRota.SetActive(false);
            }
        }
    }
}

