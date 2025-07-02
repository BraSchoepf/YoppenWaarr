using System.Collections.Generic;
using UnityEngine;

public class ConditionNPC : MonoBehaviour
{
    public List<string> npcsRequeridos = new List<string>(); // asignar desde inspector
    private HashSet<string> npcsHablados = new HashSet<string>();
    private bool llaveRecibida = false;

    public PuertaBossController puertaBoss;

    public void NPCVisitado(string npcID)
    {
        if (!npcsHablados.Contains(npcID))
        {
            npcsHablados.Add(npcID);
            Debug.Log($"Visitado: {npcID}");
            VerificarCondicion();
        }
    }

    public void RecibirLlave()
    {
        llaveRecibida = true;
        Debug.Log("Llave recibida");
        VerificarCondicion();
    }

    private void VerificarCondicion()
    {
        if (llaveRecibida && npcsRequeridos.TrueForAll(id => npcsHablados.Contains(id)))
        {
            puertaBoss?.AbrirPorton();
            Debug.Log("Se cumplen todas las condiciones → abrir portón");

            // Notify QuestManager
            QuestManager questManager = FindFirstObjectByType<QuestManager>();
            if (questManager != null)
            {
                questManager.CompleteObjective("door");
            }
        }
    }
}