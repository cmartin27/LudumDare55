using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum EInputMode
{
    InGame,
    UI,
    Dialogue,
    Summoning
    
}

[RequireComponent(typeof(DialogueManager))]
[RequireComponent(typeof(AudioManager))]
[RequireComponent(typeof(InventoryManager))]
[RequireComponent(typeof(QuestManager))]
[RequireComponent(typeof(ResourceManager))]
public class GameManager : MonoBehaviour
{
    private static GameManager instance_ = null;
    public static GameManager Instance
    {
        get { return instance_; }
    }

    public DialogueManager dialogueManager_ { get; private set; }
    public AudioManager audioManager_ { get; private set; }
    public InventoryManager inventoryManager_ { get; private set; }
    public QuestManager questManager_ { get; private set; }
    public ResourceManager resourceManager_ { get; private set; }
    public SummoningManager summoningManager_;
    public GameObject player_;
    public GameObject mainCamera_;

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
        resourceManager_ = GetComponent<ResourceManager>();

    }

    public void SetInputMode(EInputMode mode)
    {
        PlayerInput playerInput = player_.GetComponent<PlayerInput>();
        switch(mode)
        {
            case EInputMode.UI:
                playerInput.SwitchCurrentActionMap("UI");
                break;
            case EInputMode.Summoning:
                playerInput.SwitchCurrentActionMap("Summoning");
                break;
            case EInputMode.Dialogue:
                playerInput.SwitchCurrentActionMap("Dialogue");
                break;
            case EInputMode.InGame:
            default:
                playerInput.SwitchCurrentActionMap("InGame");
                break;
        }
    }

    public void DisableInput()
    {
        player_.GetComponent<PlayerInput>().DeactivateInput();
        
    }

    public void EnableInput()
    {
        player_.GetComponent<PlayerInput>().ActivateInput();
    }


}
