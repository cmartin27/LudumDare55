using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementComponent))]
[RequireComponent(typeof(InteractionComponent))]
public class PlayerComponent : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.player_ = gameObject;
    }
}
