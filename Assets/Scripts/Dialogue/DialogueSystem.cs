using UnityEngine;
using static Unity.VisualScripting.Member;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance;

    [SerializeField] private DialogueUI dialogueUI;

    public DialogueTrigger npcActual; // <- nuevo campo


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartDialogue(Dialogue dialogue, DialogueTrigger source)
    {
        npcActual = source; // guardamos quién inició el diálogo
        GameManager.Instance?.SetDialogueState(true);
        dialogueUI.ShowDialogue(dialogue);
    }

    public void EndDialogue()
    {
        GameManager.Instance?.SetDialogueState(false);
        dialogueUI.HideDialogue();
    }

    public bool IsDialogueActive() => dialogueUI.IsActive();
}