using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public bool entregaLlave = false;
    public string npcID;  //un nombre �nico para cada NPC


    public void TriggerDialogue()
    {
        if (dialogue != null)
        {
            DialogueSystem.Instance.StartDialogue(dialogue, this);
        }
    }
}