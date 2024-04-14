using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public PlayerInput playerInput_;


    public void Start()
    {
        playerInput_ = GameManager.Instance.player_.GetComponent<PlayerInput>();
    }

    public void ActivateWorldInput()
    {
        playerInput_.actions.FindActionMap("Default").Enable();
        playerInput_.actions.FindActionMap("Summoing").Disable();
        playerInput_.actions.FindActionMap("UI").Disable();
    }


    public void ActivateUIInput()
    {
        playerInput_.actions.FindActionMap("UI").Enable();
        playerInput_.actions.FindActionMap("Default").Disable();
        playerInput_.actions.FindActionMap("Summoing").Disable();
    }

    public void ActivateSummoningInput()
    {
        playerInput_.actions.FindActionMap("Summoing").Enable();
        playerInput_.actions.FindActionMap("Default").Disable();
        playerInput_.actions.FindActionMap("UI").Disable();
    }


}
