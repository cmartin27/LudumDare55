using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class MovementComponent : MonoBehaviour
{

    Rigidbody2D rb_;
    Animator animator_;
    public float movSpeed_ = 5.0f;

    private void Start()
    {
        rb_ = GetComponent<Rigidbody2D>();
        animator_ = GetComponent<Animator>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveAmount = context.ReadValue<Vector2>() * movSpeed_;
        rb_.velocity = moveAmount;
    
        // Set animation control variables
        bool isMoving = rb_.velocity.magnitude > 0.0f;
        animator_.SetBool("IsMoving", isMoving);
        if (Mathf.Abs(rb_.velocity.x) > 0.0f)
        { 
            animator_.SetFloat("Horizontal", moveAmount.x);
        }
    }

}
