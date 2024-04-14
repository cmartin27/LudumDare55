using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
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
    public UnityEvent pathCompleted_;

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
        Vector2 moveAmount =  direction_ * movSpeed_* Time.fixedDeltaTime;
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


    public void StartPath(List<Vector3> pathPoints)
    {
        StartCoroutine(FollowPath(pathPoints));
    }

    IEnumerator FollowPath(List<Vector3> pathPoints)
    {
        int currentPointIndex = 0;
        animator_.SetBool("IsMoving", true);
        animator_.SetFloat("Horizontal", 0.01f);


        while (currentPointIndex < pathPoints.Count)
        {
            Vector2 targetPosition = pathPoints[currentPointIndex];
            Vector2 currentPosition = rb_.position;

            // Move towards the target position
            rb_.MovePosition(Vector2.MoveTowards(currentPosition, targetPosition, movSpeed_ * Time.fixedDeltaTime));

            if (Mathf.Abs(rb_.velocity.x) > 0.0f)
            {
                Debug.Log(rb_.velocity.x);
                animator_.SetFloat("Horizontal", rb_.velocity.x);
            }

            // Check if the target position is reached
            if (Vector2.Distance(currentPosition, targetPosition) < 0.1f)
            {
                currentPointIndex++;

                // Check if all points are visited
                if (currentPointIndex == pathPoints.Count)
                {
                    animator_.SetBool("IsMoving", false);
                    pathCompleted_.Invoke();
                    yield break; // Exit the coroutine
                }
            }
            yield return null;
        }
    }


}
