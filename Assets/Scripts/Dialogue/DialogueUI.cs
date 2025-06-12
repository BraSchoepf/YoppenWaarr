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
            DialogueSystem.Instance.EndDialogue(); // 👈 Esta es la línea clave
        }
    }


    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
        currentDialogue = null;
    }

    public bool IsActive() => dialoguePanel.activeSelf;
}