using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(MovementByVelocityEvent))]
[DisallowMultipleComponent]
public class MovementByVelocity : MonoBehaviour
{
    private Rigidbody2D rb;
    private MovementByVelocityEvent movementByVelocityEvent;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        movementByVelocityEvent = GetComponent<MovementByVelocityEvent>();
    }

    private void OnEnable()
    {
        movementByVelocityEvent.OnMovementByVelocity += OnMovementByVelocity;
    }

    private void OnDisable()
    {
        movementByVelocityEvent.OnMovementByVelocity -= OnMovementByVelocity;
    }

    // On Movement event
    private void OnMovementByVelocity(MovementByVelocityEvent movementEvent, MovementByVelocityArgs movementArgs)
    {
        MoveRigidBody(movementArgs.moveDirection, movementArgs.moveSpeed);
    }

    private void MoveRigidBody(Vector2 moveDirection, float moveSpeed)
    {
        rb.velocity = moveDirection * moveSpeed;
    }
}