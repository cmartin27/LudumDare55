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

    public void StartQuest(int questId)
    {
        questStates_[questId] = EQuestState.InProgress;
    }

    public void FinishQuest(int questId) 
    {
        questStates_[questId] = EQuestState.Finished;
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
