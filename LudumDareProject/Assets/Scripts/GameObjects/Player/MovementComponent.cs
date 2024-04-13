using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementComponent : MonoBehaviour
{

    public Rigidbody2D rb;
public Animator animator;
    public float movSpeed = 10.0f;
    
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveAmount = context.ReadValue<Vector2>() * movSpeed;
        rb.velocity = moveAmount;
        bool isMoving = rb.velocity.magnitude > 0.0f;
        animator.SetBool("IsMoving", isMoving);
        if (Mathf.Abs(rb.velocity.x) > 0.0f)
        { 
            animator.SetFloat("Horizontal", moveAmount.x);
        }


    }

}
