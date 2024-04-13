using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private List<DialogueEntry> dialogues_;

    private string[] currentDialogue_;
    private int currentDialogueLine_;

    public void StartDialogue(int dialogId, EQuestState questState)
    {
        // TODO: change player input
        // ...

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

        currentDialogue_ = dialog.Split('\n');
        currentDialogueLine_ = 0;

        // TODO: Show dialogue bubble and start animation for first line
    }

    public void OnNextLine()
    {
        // finished dialogue, end dialogue
        if(++currentDialogueLine_ >= currentDialogue_.Length) 
        {
            EndDialogue();
        }
        else
        {
            string currentLine = currentDialogue_[currentDialogueLine_];
            // TODO: Start animation for next line
        }
    }

    private void EndDialogue()
    {
        // TODO: Remove dialogue bubble

        // TODO: Return input to player
    }
}
