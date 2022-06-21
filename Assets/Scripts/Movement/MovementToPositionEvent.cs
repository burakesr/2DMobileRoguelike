using UnityEngine;
using System;

[DisallowMultipleComponent]
public class MovementToPositionEvent : MonoBehaviour
{
    public event Action<MovementToPositionEvent, MovementToPositionArgs > OnMovementToPosition;

    public void CallMovementToPositionEvent(Vector3 movePosition, Vector3 currentPosition, Vector2 moveDirection,
        float moveSpeed, bool isRolling)
    {
        OnMovementToPosition?.Invoke(this, new MovementToPositionArgs
        {
            movePosition = movePosition,
            currentPosition = currentPosition,
            moveDirection = moveDirection,
            moveSpeed = moveSpeed,
            isRolling = isRolling
        });
    }
}
public class MovementToPositionArgs : EventArgs
{
    public Vector3 movePosition;
    public Vector2 moveDirection;
    public Vector3 currentPosition;
    public float moveSpeed;
    public bool isRolling;
}