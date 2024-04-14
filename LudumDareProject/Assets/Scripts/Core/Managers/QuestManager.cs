using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum EQuestState
{
    NotInitialized,
    InProgress,
    AfterSummoning,
    Finished
}

[Serializable]
public class QuestInfo
{
    public EQuestState state_;
    public Vector3 summoningPosition_;
}

public class QuestManager : MonoBehaviour
{
    [SerializeField]
    private int nQuests_;
    [SerializeField]
    private List<QuestInfo> questStates_;

    public EQuestState GetQuestStatus(int questId)
    {
        return questStates_[questId].state_;
    }

    public bool IsQuestInProgress(int questId)
    {
        return questStates_[questId].state_ == EQuestState.InProgress;
    }

    public void StartQuest(int questId)
    {
        if(questStates_[questId].state_ == EQuestState.NotInitialized)
        {
            questStates_[questId].state_ = EQuestState.InProgress;
        }
    }

    public void FinishQuest(int questId) 
    {
        if (questStates_[questId].state_ == EQuestState.AfterSummoning)
        {
            questStates_[questId].state_ = EQuestState.Finished;
        }
    }

    public Vector3 GetQuestPosition(int questId)
    {
        return questStates_[questId].summoningPosition_;

    }

}
