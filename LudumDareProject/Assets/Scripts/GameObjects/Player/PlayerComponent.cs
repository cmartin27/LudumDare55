using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MovementComponent))]
[RequireComponent(typeof(InteractionComponent))]
public class PlayerComponent : MonoBehaviour
{
    [SerializeField]
    GameObject summoningDialogue_;

    [SerializeField]
    GameObject confirmButton_;

    public void ShowSummoningDialogue()
    {
        summoningDialogue_.SetActive(true);
        EventSystem.current.SetSelectedGameObject(confirmButton_);
    }

    public void HideSummoningDialogue()
    {
        summoningDialogue_.SetActive(false);
    }

    private void Start()
    {
        GameManager.Instance.player_ = gameObject;
    }

    public void ConfirmSummoning()
    {
        HideSummoningDialogue();
        GameManager.Instance.questManager_.StartSummoning();
    }

    public void CancelSummoning()
    {
        HideSummoningDialogue();
        GameManager.Instance.SetInputMode(EInputMode.InGame);
    }
}
