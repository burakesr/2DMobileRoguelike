using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(IdleEvent))]
[DisallowMultipleComponent]
public class Idle : MonoBehaviour
{
    private Rigidbody2D rb;
    private IdleEvent idleEvent;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        idleEvent = GetComponent<IdleEvent>();
    }

    private void OnEnable()
    {
        idleEvent.OnIdle += OnIdle;
    }

    private void OnDisable()
    {
        idleEvent.OnIdle -= OnIdle;
    }

    private void OnIdle(IdleEvent idleEvent)
    {
        MoveRigidbody();
    }

    private void MoveRigidbody()
    {
        // Ensure the rb collision detection is set to continous
        rb.velocity = Vector2.zero;
    }
}