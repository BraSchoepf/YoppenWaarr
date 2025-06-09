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
        dialogueUI.ShowDialogue(dialogue);
    }

    public bool IsDialogueActive() => dialogueUI.IsActive();
}