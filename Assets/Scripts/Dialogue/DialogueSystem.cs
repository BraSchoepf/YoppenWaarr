using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance;

    [SerializeField] private DialogueUI dialogueUI;
    public DialogueTrigger npcActual;

    private bool blockNextInteraction = false;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartDialogue(Dialogue dialogue, DialogueTrigger source)
    {
        npcActual = source;
        GameManager.Instance?.SetDialogueState(true);
        dialogueUI.ShowDialogue(dialogue);
    }

    public void EndDialogue()
    {
        GameManager.Instance?.SetDialogueState(false);
        dialogueUI.HideDialogue();
    }

    public bool IsDialogueActive() => dialogueUI.IsActive();

    public void BlockNextInteractionUntilKeyReleased()
    {
        blockNextInteraction = true;
    }

    public bool CanStartNewDialogue()
    {
        return !blockNextInteraction;
    }

    public void ResetInteractionBlock()
    {
        blockNextInteraction = false;
    }
}