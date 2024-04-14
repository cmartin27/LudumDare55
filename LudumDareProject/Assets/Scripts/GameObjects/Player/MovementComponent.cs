using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class MovementComponent : MonoBehaviour
{

    Rigidbody2D rb_;
    Animator animator_;
    public float movSpeed_ = 5.0f;
    public bool isMoving_;
    public Vector2 direction_;

    private void Start()
    {
        rb_ = GetComponent<Rigidbody2D>();
        animator_ = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if(isMoving_)
        {
            Move();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StartMoving(context.ReadValue<Vector2>());
        }
        else {
            StopMoving();
        }
    }

    private void Move()
    {
        Vector2 moveAmount =  direction_ * movSpeed_;
        rb_.velocity = moveAmount;
        if (Mathf.Abs(rb_.velocity.x) > 0.0f)
        {
            animator_.SetFloat("Horizontal", moveAmount.x);
        }
    }

    private void StartMoving(Vector2 dir)
    {
        isMoving_ = true;
        animator_.SetBool("IsMoving", true);
        direction_ = dir;
    }

    private void StopMoving()
    {
        isMoving_ = false;
        rb_.velocity = Vector2.zero;
        direction_ = Vector2.zero;
        animator_.SetBool("IsMoving", false);
    }
}
