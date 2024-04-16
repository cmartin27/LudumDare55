using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionComponent : MonoBehaviour
{
    [SerializeField]
    float interactionRadius_ = 10.0f;
    [SerializeField]
    bool showInteractionRadius_ = false;
    [SerializeField]
    LayerMask interactionMask_;

    public void OnInteract(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, interactionRadius_, transform.forward, 0.0f, interactionMask_);

            if (hit)
            {
                IInteractable interactable = hit.transform.gameObject.GetComponent<IInteractable>();
                if (interactable != null) interactable.Interact();
            }
        }
    }
    
    public void OnDebugSummoningMenu(InputAction.CallbackContext context)
    {

        GameManager.Instance.summoningManager_.EnableSummoningMenu(GameManager.Instance.questManager_.GetQuest(0));
        GameManager.Instance.SetInputMode(EInputMode.UI);
    }

    public void OnInteractSummoning(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.Instance.summoningManager_.AddResource();
        }
    }

    public void OnMenuOpened(InputAction.CallbackContext context)
    {
        if (context.performed)
        { 
            GameManager.Instance.uiManager_.OpenPauseMenu();
        }
    }

}
