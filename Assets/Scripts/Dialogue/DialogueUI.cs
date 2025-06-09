using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button skipButton;
    [SerializeField] private Button exitButton;

    private Dialogue currentDialogue;
    private int dialogueIndex;

    private void Awake()
    {
        nextButton.onClick.AddListener(NextLine);
        skipButton.onClick.AddListener(SkipDialogue);
        exitButton.onClick.AddListener(HideDialogue);
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
            HideDialogue();
        }
    }

    private void SkipDialogue()
    {
        dialogueText.text = currentDialogue.lines[^1]; // Última línea
        dialogueIndex = currentDialogue.lines.Length - 1;
    }

    private void HideDialogue()
    {
        dialoguePanel.SetActive(false);
        currentDialogue = null;
    }

    public bool IsActive() => dialoguePanel.activeSelf;
}