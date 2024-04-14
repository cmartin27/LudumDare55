using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class NPCComponent : MonoBehaviour, IInteractable
{
    [SerializeField]
    public int id_ {  get; private set; }
    public float dialogueBoxWidth_ { get; private set; }

    [SerializeField]
    private GameObject dialogueBox_;
    [SerializeField]
    private TMP_Text dialogueText_;

    public void Interact()
    {
        GameManager.Instance.questManager_.ActivateQuest(this);
    }

    public void ShowDialogueBox()
    {
        dialogueBox_.SetActive(true);
        SetDialogueWidth(0);
        SetDialogueText("");
    }

    public void HideDialogueBox()
    {
        dialogueBox_.SetActive(false);
    }

    public void SetDialogueText(string text)
    {
        dialogueText_.text = text;
    }

    public void IncrementDialogueWidth(float increment)
    {
        dialogueBox_.transform.localScale += new Vector3(increment, 0.0f, 0.0f);
    }

    public void RestoreDialogWidth() 
    {
        SetDialogueWidth(dialogueBoxWidth_);
    }

    private void Start()
    {
        dialogueBoxWidth_ = dialogueBox_.transform.localScale.x;
    }



    private void SetDialogueWidth(float width)
    {
        Vector3 scale = dialogueBox_.transform.localScale;
        scale.x = width;
        dialogueBox_.transform.localScale = scale;
    }

    private void StartQuest()
    {
        GameManager.Instance.questManager_.StartQuest(id_);
    }
}
