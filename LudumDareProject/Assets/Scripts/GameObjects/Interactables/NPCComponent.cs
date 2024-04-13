using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class NPCComponent : MonoBehaviour, IInteractable
{
    [SerializeField]
    private int id_;

    [SerializeField]
    private SpriteRenderer dialogueBox_;
    [SerializeField]
    private TMP_Text dialogueText_;

    public void Interact()
    {
        StartDialog();
    }

    public void ShowDialogueBox()
    {
        dialogueBox_.enabled = true;
        dialogueText_.enabled = true;
    }

    public void HideDialogueBox()
    {
        dialogueBox_.enabled = false;
        dialogueText_.enabled = false;
    }

    public void SetDialogText(string text)
    {
        dialogueText_.text = text;
    }

    private void Start()
    { 
        dialogueBox_.enabled = false;
        dialogueText_.enabled = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) {
            StartDialog();
        }
    }

    private void StartDialog()
    {
        EQuestState questState = GameManager.Instance.questManager_.GetQuestStatus(id_);
        GameManager.Instance.dialogueManager_.StartDialogue(this, id_, questState);
    }
}
