using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void StartDialogue(NPCComponent npc, int dialogId, EQuestState questState)
    {
        GameManager.Instance.SetInputMode(EInputMode.Dialogue);

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
        currentDialogueLine_ = 0;
        StartCoroutine(ShowDialogueBubble());
    }

    public void OnNextLine()
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
        Debug.Log(line);
        currentLine_ = line;
        StartCoroutine(DisplayLine());
    }

    // Ends the dialogue, returns input to player
    private void EndDialogue()
    {
        StartCoroutine(HideDialogueBubble());
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)) {
            OnNextLine();
        }
    }

    // Displays the dialogue bubble of the npc, increasing its width over a period of time
    IEnumerator ShowDialogueBubble()
    {
        npc_.ShowDialogueBox();

        const int nIncrements = 100;
        float dialogueBoxWidth = npc_.dialogueBoxWidth_;
        float widthIncrement = dialogueBoxWidth / (float)nIncrements;
        float timeIncrement = dialogueAppearDuration_ / (float)nIncrements;

        for(int i = 0; i < nIncrements; ++i)
        {
            npc_.IncrementDialogueWidth(widthIncrement);
            yield return new WaitForSeconds(timeIncrement);
        }

        npc_.RestoreDialogWidth();
        ShowDialogueLine(currentDialogue_[currentDialogueLine_]);
    }

    // Hides the dialogue bubble of the npc, reducing its width over a period of time

    IEnumerator HideDialogueBubble()
    {
        npc_.SetDialogueText("");

        const int nIncrements = 100;
        float dialogueBoxWidth = npc_.dialogueBoxWidth_;
        float widthIncrement = dialogueBoxWidth / (float)nIncrements;
        float timeIncrement = dialogueAppearDuration_ / (float)nIncrements;

        for (int i = 0; i < nIncrements; ++i)
        {
            npc_.IncrementDialogueWidth(-widthIncrement);
            yield return new WaitForSeconds(timeIncrement);
        }

        npc_.HideDialogueBox();
        GameManager.Instance.SetInputMode(EInputMode.InGame);
    }

    // Displays a line letter by letter over a period of time
    IEnumerator DisplayLine()
    {
        int nChars = currentLine_.Length;
        float timeBetweenChars = Mathf.Min(linePrintDuration_ / nChars, maxCooldownBetweenChars_);

        string lineInConstruction = "";
        for(int i = 0; i < nChars; i++)
        {
            lineInConstruction += currentLine_[i];
            npc_.SetDialogueText(lineInConstruction);
            yield return new WaitForSeconds(timeBetweenChars);
        }
    }
}
