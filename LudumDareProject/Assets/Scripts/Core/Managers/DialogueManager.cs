using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private float linePrintDuration;    
    [SerializeField]
    private float maxCooldownBetweenChars;
    [SerializeField]
    private List<DialogueEntry> dialogues_;

    private string[] currentDialogue_;
    private string currentLine_;
    private int currentDialogueLine_;
    private NPCComponent npc_;

    public void StartDialogue(NPCComponent npc, int dialogId, EQuestState questState)
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

        npc_ = npc;
        currentDialogue_ = dialog.Split('\n');
        currentDialogueLine_ = 0;
        npc_.ShowDialogueBox();

        // TODO: Show dialogue bubble and start animation for first line
        ShowDialogueLine(currentDialogue_[currentDialogueLine_]);
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
            ShowDialogueLine(currentLine);

            // TODO: Start animation for next line
        }
    }

    private void ShowDialogueLine(string line)
    {
        Debug.Log(line);
        currentLine_ = line;
        StartCoroutine(DisplayLine());
    }

    private void EndDialogue()
    {
        Debug.Log("Finished dialog!");
        // TODO: Remove dialogue bubble
        npc_.HideDialogueBox();
        // TODO: Return input to player
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)) {
            OnNextLine();
        }
    }

    IEnumerator DisplayLine()
    {
        int nChars = currentLine_.Length;
        float timeBetweenChars = Mathf.Min(linePrintDuration / nChars, maxCooldownBetweenChars);

        string lineInConstruction = "";
        for(int i = 0; i < nChars; i++)
        {
            lineInConstruction += currentLine_[i];
            npc_.SetDialogText(lineInConstruction);
            yield return new WaitForSeconds(timeBetweenChars);
        }
    }
}
