using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance;

    [SerializeField] private DialogueUI dialogueUI;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartDialogue(Dialogue dialogue)
    {
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