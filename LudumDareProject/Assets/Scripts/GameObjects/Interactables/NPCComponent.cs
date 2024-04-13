using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCComponent : MonoBehaviour, IInteractable
{
    [SerializeField]
    private int id_;

    public void Interact()
    {
        StartDialog();
    }

    private void Start()
    {
/*        string[] questNotInitializedDialogArray = questNotInitializedDialog.Split('\n');

        foreach(string s in questNotInitializedDialogArray)
        {
            Debug.Log(s);
        }*/

        if(Input.GetKeyDown(KeyCode.Space)) {
            StartDialog();
        }
    }

    private void StartDialog()
    {
        EQuestState questState = GameManager.Instance.questManager_.GetQuestStatus(id_);
        GameManager.Instance.dialogueManager_.StartDialogue(id_, questState);
    }
}
