using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UIElements;

public enum EQuestState
{
    NotInitialized,
    InProgress,
    AfterSuccessfulAttempt,
    AfterFailedAttempt,
    Finished
}

[Serializable]
public class QuestInfo
{
    public int id_;
    public EQuestState state_;
    public Vector3 summoningPosition_;
    public SummoningInfo summoningInfo_;
    public DialogueEntry dialogueEntry_;
}

public class QuestManager : MonoBehaviour
{
    public List<QuestInfo> quests_;

    public int activeQuestId_ { get; private set; }
    private NPCComponent activeNPC_;

    public QuestInfo GetQuest(int questId)
    {
        return quests_[questId];
    }

    public EQuestState GetQuestStatus(int questId)
    {
        return quests_[questId].state_;
    }

    public bool IsQuestInProgress(int questId)
    {
        return quests_[questId].state_ == EQuestState.InProgress;
    }

    public void StartQuest(int questId)
    {
        if(quests_[questId].state_ == EQuestState.NotInitialized)
        {
            quests_[questId].state_ = EQuestState.InProgress;
        }
    }

    public void FinishQuest(int questId) 
    {
        if (quests_[questId].state_ == EQuestState.InProgress)
        {
            quests_[questId].state_ = EQuestState.Finished;
        }
    }

    public Vector3 GetQuestPosition(int questId)
    {
        return quests_[questId].summoningPosition_;

    }

    public void ActivateQuest(NPCComponent npc)
    {
        activeQuestId_ = npc.id_;
        activeNPC_ = npc;
        GameManager.Instance.SetInputMode(EInputMode.Dialogue);
        GameManager.Instance.dialogueManager_.StartDialogue(npc, quests_[activeQuestId_].dialogueEntry_, quests_[activeQuestId_].state_);
    }

    public void EndDialogue()
    {
         GameManager.Instance.SetInputMode(EInputMode.InGame);
        QuestInfo quest = GetQuest(activeQuestId_);
        switch (quest.state_)
        {
            case EQuestState.NotInitialized:
                quest.state_ = EQuestState.InProgress;
                break;
            case EQuestState.InProgress:
                GameManager.Instance.SetInputMode(EInputMode.Dialogue);
                GameManager.Instance.player_.GetComponent<PlayerComponent>().ShowSummoningDialogue();
                break;
            case EQuestState.AfterSuccessfulAttempt:
                quest.state_ = EQuestState.Finished;
                break;
            case EQuestState.AfterFailedAttempt:
                quest.state_ = EQuestState.InProgress;
                break;
            case EQuestState.Finished:
            default:
                break;
        }
    }

    public void StartSummoning()
    {
        GameManager.Instance.summoningManager_.EnableSummoningMenu(quests_[activeQuestId_]);
    }

    public void QuestSuccessful()
    {
        QuestInfo quest = GetQuest(activeQuestId_);
        quest.state_ = EQuestState.AfterSuccessfulAttempt;
        GameManager.Instance.dialogueManager_.StartDialogue(activeNPC_, quests_[activeQuestId_].dialogueEntry_, quest.state_);
    }

    public void QuestFailed()
    {
        QuestInfo quest = GetQuest(activeQuestId_);
        quest.state_ = EQuestState.AfterFailedAttempt;
        GameManager.Instance.dialogueManager_.StartDialogue(activeNPC_, quests_[activeQuestId_].dialogueEntry_, quest.state_);
    }
}
