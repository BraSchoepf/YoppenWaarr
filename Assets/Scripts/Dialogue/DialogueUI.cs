using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button exitButton;

    private Dialogue currentDialogue;
    private int dialogueIndex;

    private void Awake()
    {
        nextButton.onClick.AddListener(NextLine);
        exitButton.onClick.AddListener(() => DialogueSystem.Instance.EndDialogue());
    }

    private void Update()
    {
        if (IsActive() && Input.GetKeyDown(KeyCode.E) || IsActive() && Input.GetKeyDown("joystick button 3"))
        {
            NextLine();
        }
    }

    public void ShowDialogue(Dialogue dialogue)
    {
        currentDialogue = dialogue;
        dialogueIndex = 0;
        dialoguePanel.SetActive(true);
        nameText.text = currentDialogue.npcName;
        dialogueText.text = currentDialogue.lines[dialogueIndex];
    }

    private void NextLine()
    {
        dialogueIndex++;
        if (dialogueIndex < currentDialogue.lines.Length)
        {
            dialogueText.text = currentDialogue.lines[dialogueIndex];
        }
        else
        {
            DialogueTrigger trigger = DialogueSystem.Instance.npcActual;
            if (trigger != null)
            {
                var quest = FindFirstObjectByType<ConditionNPC>();
                if (quest != null)
                {
                    quest.NPCVisitado(trigger.npcID);
                    if (trigger.entregaLlave)
                    {
                        quest.RecibirLlave();
                    }
                }
            }

            DialogueSystem.Instance.EndDialogue();
            DialogueSystem.Instance.BlockNextInteractionUntilKeyReleased();
        }
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
        currentDialogue = null;
    }

    public bool IsActive() => dialoguePanel.activeSelf;
}