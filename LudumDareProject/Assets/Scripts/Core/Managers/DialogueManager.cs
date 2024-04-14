using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private float dialogueAppearDuration_;
    [SerializeField]
    private float linePrintDuration_;    
    [SerializeField]
    private float maxCooldownBetweenChars_;
    [SerializeField]
    private List<DialogueEntry> dialogues_;

    private string[] currentDialogue_;
    private string currentLine_;
    private int currentDialogueLine_;
    private NPCComponent npc_;

    private bool inDialogue_;
    private bool isDisplayingAnimation_;
    private bool wantToCutAnimation_;

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!inDialogue_) return;

        if(context.performed)
        {
            if (!isDisplayingAnimation_)
            {
                ShowNextLine();
            }
            else
            {
                wantToCutAnimation_ = true;
            }
        }
    }

    public void StartDialogue(NPCComponent npc, int dialogId, EQuestState questState)
    {
        inDialogue_ = true;
        isDisplayingAnimation_ = false;

        DialogueEntry entry = dialogues_[dialogId];
        string dialog = "";
        switch(questState) 
        {
            case EQuestState.NotInitialized:
                dialog = entry.questNotInitialized;
                break;
            case EQuestState.InProgress:
                dialog = entry.questInProgress;
                break;
            case EQuestState.AfterSummoning:
                dialog = entry.questAfterSummoning;
                break;
            case EQuestState.Finished:
                dialog = entry.questFinished;
                break;
        }

        npc_ = npc;
        currentDialogue_ = dialog.Split('\n');
        currentDialogueLine_ = -1;
        StartCoroutine(ShowDialogueBubble());
    }

    private void Clean()
    {
        currentDialogue_ = null;
        currentLine_ = null;
        currentDialogueLine_ = -1;
        npc_ = null;

        isDisplayingAnimation_ = false;
        wantToCutAnimation_ = false;
        inDialogue_ = false;

    }

    private void ShowNextLine()
    {
        // finished dialogue, end dialogue
        if(++currentDialogueLine_ >= currentDialogue_.Length) 
        {
            EndDialogue();
        }
        else // show next dialog line
        {
            string currentLine = currentDialogue_[currentDialogueLine_];
            ShowDialogueLine(currentLine);
        }
    }

    // Starts the coroutine to show a dialogue line
    private void ShowDialogueLine(string line)
    {
        currentLine_ = line;
        StartCoroutine(DisplayLine());
    }

    // Ends the dialogue, returns input to player
    private void EndDialogue()
    {
        StartCoroutine(HideDialogueBubble());
    }

    // Displays the dialogue bubble of the npc, increasing its width over a period of time
    IEnumerator ShowDialogueBubble()
    {
        npc_.ShowDialogueBox();
        isDisplayingAnimation_ = true;
        const int nIncrements = 100;
        float dialogueBoxWidth = npc_.dialogueBoxWidth_;
        float widthIncrement = dialogueBoxWidth / (float)nIncrements;
        float timeIncrement = dialogueAppearDuration_ / (float)nIncrements;

        for(int i = 0; i < nIncrements; ++i)
        {
            if(wantToCutAnimation_)
            {
                wantToCutAnimation_ = false;
                break;
            }
            npc_.IncrementDialogueWidth(widthIncrement);
            yield return new WaitForSeconds(timeIncrement);
        }

        isDisplayingAnimation_ = false;
        npc_.RestoreDialogWidth();
        ShowNextLine();
    }

    // Hides the dialogue bubble of the npc, reducing its width over a period of time

    IEnumerator HideDialogueBubble()
    {
        npc_.SetDialogueText("");
        isDisplayingAnimation_ = true;
        const int nIncrements = 100;
        float dialogueBoxWidth = npc_.dialogueBoxWidth_;
        float widthIncrement = dialogueBoxWidth / (float)nIncrements;
        float timeIncrement = dialogueAppearDuration_ / (float)nIncrements;

        for (int i = 0; i < nIncrements; ++i)
        {
            if (wantToCutAnimation_)
            {
                wantToCutAnimation_ = false;
                break;
            }
            npc_.IncrementDialogueWidth(-widthIncrement);
            yield return new WaitForSeconds(timeIncrement);
        }

        isDisplayingAnimation_ = false;
        npc_.HideDialogueBox();
        GameManager.Instance.questManager_.EndDialogue();
        Clean();
    }

    // Displays a line letter by letter over a period of time
    IEnumerator DisplayLine()
    {
        int nChars = currentLine_.Length;
        float timeBetweenChars = Mathf.Min(linePrintDuration_ / nChars, maxCooldownBetweenChars_);
        isDisplayingAnimation_ = true;

        string lineInConstruction = "";
        for(int i = 0; i < nChars; i++)
        {
            if (wantToCutAnimation_)
            {
                npc_.SetDialogueText(currentLine_);
                wantToCutAnimation_ = false;
                break;
            }
            lineInConstruction += currentLine_[i];
            npc_.SetDialogueText(lineInConstruction);
            yield return new WaitForSeconds(timeBetweenChars);
        }

        isDisplayingAnimation_ = false;
    }
}
