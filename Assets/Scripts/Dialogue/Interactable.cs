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
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            dialogueTrigger.TriggerDialogue();
            interactPrompt.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Entró {other.name} al trigger");

        if (other.CompareTag("Player"))
        {
            Debug.Log("Es el jugador!");
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
        }
    }
}