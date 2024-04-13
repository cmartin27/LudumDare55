using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueManager))]
[RequireComponent(typeof(AudioManager))]
[RequireComponent(typeof(InventoryManager))]
[RequireComponent(typeof(QuestManager))]
public class GameManager : MonoBehaviour
{
    private static GameManager instance_ = null;
    public static GameManager Instance
    {
        get { return instance_; }
    }

    public DialogueManager dialogueManager_;
    public AudioManager audioManager_;
    public InventoryManager inventoryManager_;
    public QuestManager questManager_;

    private void Awake()
    {
        instance_ = this;
    }

    private void Start()
    {
        dialogueManager_ = GetComponent<DialogueManager>();
        audioManager_ = GetComponent<AudioManager>();
        inventoryManager_ = GetComponent<InventoryManager>();
        questManager_ = GetComponent<QuestManager>();
    }
}
