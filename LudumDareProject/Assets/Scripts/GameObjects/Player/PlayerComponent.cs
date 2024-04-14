using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MovementComponent))]
[RequireComponent(typeof(InteractionComponent))]
public class PlayerComponent : MonoBehaviour
{
    [SerializeField]
    GameObject summoningDialogue_;

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && summoningDialogue_.activeSelf)
        {
            StartSummoning();
            summoningDialogue_.SetActive(false);
        }
    }

    public void ShowSummoningDialogue()
    {
        summoningDialogue_.SetActive(true);
    }

    private void Start()
    {
        GameManager.Instance.player_ = gameObject;
    }

    private void StartSummoning()
    {
        // start summon manager

    }
}
