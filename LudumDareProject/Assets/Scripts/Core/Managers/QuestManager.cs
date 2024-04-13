using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EQuestState
{
    NotInitialized,
    InProgress,
    AfterSummoning,
    Finished
}
public class QuestManager : MonoBehaviour
{
    private List<EQuestState> questStates_;
    public EQuestState GetQuestStatus(int questId)
    {
        return questStates_[questId];
    }

    public void StartQuest(int questId)
    {
        questStates_[questId] = EQuestState.InProgress;
    }

    public void FinishQuest(int questId) 
    {
        questStates_[questId] = EQuestState.Finished;
    }
}
