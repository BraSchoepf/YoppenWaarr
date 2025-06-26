using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private GameObject interactPrompt;
    private DialogueTrigger dialogueTrigger;
    private bool playerInRange;

    private void Awake()
    {
        dialogueTrigger = GetComponent<DialogueTrigger>();
        interactPrompt.SetActive(false);
    }

    private void Update()
    {
        // Si el jugador esta en rango no hay diálogo activo NO está bloqueado entonces puede interactuar
        if ((playerInRange && Input.GetKeyDown(KeyCode.E) && !DialogueSystem.Instance.IsDialogueActive() && DialogueSystem.Instance.CanStartNewDialogue() || 
            playerInRange && Input.GetKeyDown("joystick button 3")) && !DialogueSystem.Instance.IsDialogueActive() && DialogueSystem.Instance.CanStartNewDialogue())
        {
            dialogueTrigger.TriggerDialogue();
            interactPrompt.SetActive(false);
        }

        // Cuando suelta E se habilita de nuevo las interacciones
        if (Input.GetKeyUp(KeyCode.E) || Input.GetKeyDown("joystick button 3"))
        {
            DialogueSystem.Instance.ResetInteractionBlock();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactPrompt.SetActive(true);
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactPrompt.SetActive(false);
            playerInRange = false;
            DialogueSystem.Instance.ResetInteractionBlock(); // Seguridad extra por si el jugador se va del área
        }
    }
}
