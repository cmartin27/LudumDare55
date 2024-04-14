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
    [SerializeField]
    private int nQuests_;
    [SerializeField]
    private List<EQuestState> questStates_;

    public EQuestState GetQuestStatus(int questId)
    {
        return questStates_[questId];
    }

    public bool IsQuestInProgress(int questId)
    {
        return questStates_[questId] == EQuestState.InProgress;
    }

    public void StartQuest(int questId)
    {
        if(questStates_[questId] == EQuestState.NotInitialized)
        {
            questStates_[questId] = EQuestState.InProgress;
        }
    }

    public void FinishQuest(int questId) 
    {
        if (questStates_[questId] == EQuestState.AfterSummoning)
        {
            questStates_[questId] = EQuestState.Finished;
        }
    }

    void Start()
    {
        questStates_ = new List<EQuestState>();
        for (int i = 0; i < nQuests_; ++i) 
        {
            questStates_.Add(EQuestState.NotInitialized);
        }
    }
}
