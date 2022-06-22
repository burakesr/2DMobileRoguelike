using System;
using UnityEngine;

[RequireComponent(typeof(Player))]
[DisallowMultipleComponent]
public class AnimatePlayer : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        player.movementByVelocityEvent.OnMovementByVelocity += OnMovementByVelocity;
        player.movementToPositionEvent.OnMovementToPosition += OnMovemenToPosition;
        player.idleEvent.OnIdle += OnIdle;
        player.aimWeaponEvent.OnWeaponAim += OnWeaponAim;
    }

    private void OnDisable()
    {
        player.movementByVelocityEvent.OnMovementByVelocity -= OnMovementByVelocity;
        player.movementToPositionEvent.OnMovementToPosition -= OnMovemenToPosition;
        player.idleEvent.OnIdle -= OnIdle;
        player.aimWeaponEvent.OnWeaponAim -= OnWeaponAim;
    }

    private void OnMovemenToPosition(MovementToPositionEvent movementEvent, MovementToPositionArgs movementArgs)
    {
        InitialiseAimAnimationParameters();
        InitialiseRollAnimationParamters();
        SetMovementAnimationParameters(movementArgs);
    }

    /// <summary>
    /// On Movement by Velocity event handler
    /// </summary>
    private void OnMovementByVelocity(MovementByVelocityEvent movementEvent, MovementByVelocityArgs movementArgs)
    {
        InitialiseRollAnimationParamters();
        SetMovementAnimationParameters();
    }

    /// <summary>
    /// On Weapon Aim event handler
    /// </summary>
    private void OnWeaponAim(AimWeaponEvent aimWeaponEvent, AimWeaponEventArgs aimWeaponEventArgs)
    {
        InitialiseRollAnimationParamters();
        InitialiseAimAnimationParameters();
        SetAimWeaponAnimationParameters(aimWeaponEventArgs.aimDirection);
    }

    /// <summary>
    /// On Idle event handler
    /// </summary>
    private void OnIdle(IdleEvent idleEvent)
    {
        InitialiseRollAnimationParamters();
        SetIdleAnimationParameters();
    }

    private void InitialiseRollAnimationParamters()
    {
        player.animator.SetBool(Settings.rollDown, false);
        player.animator.SetBool(Settings.rollUp, false);
        player.animator.SetBool(Settings.rollRight, false);
        player.animator.SetBool(Settings.rollLeft, false);
    }

    private void SetMovementAnimationParameters()
    {
        player.animator.SetBool(Settings.isMoving, true);
        player.animator.SetBool(Settings.isIdle, false);
    }

    private void SetMovementAnimationParameters(MovementToPositionArgs movementArgs)
    {
        if (movementArgs.isRolling)
        {
            if (movementArgs.moveDirection.x > 0f)
            {
                player.animator.SetBool(Settings.rollRight, true);
            }
            else if (movementArgs.moveDirection.x < 0f)
            {
                player.animator.SetBool(Settings.rollLeft, true);
            }
            else if (movementArgs.moveDirection.y > 0f)
            {
                player.animator.SetBool(Settings.rollUp, true);
            }
            else if (movementArgs.moveDirection.y < 0f)
            {
                player.animator.SetBool(Settings.rollDown, true);
            }
        }
    }

    private void SetIdleAnimationParameters()
    {
        player.animator.SetBool(Settings.isMoving, false);
        player.animator.SetBool(Settings.isIdle, true);
    }

    private void SetAimWeaponAnimationParameters(AimDirection aimDirection)
    {
        switch (aimDirection)
        {
            case AimDirection.Up:
                player.animator.SetBool(Settings.aimUp, true);
                break;
            case AimDirection.UpRight:
                player.animator.SetBool(Settings.aimUpRight, true);
                break;
            case AimDirection.UpLeft:
                player.animator.SetBool(Settings.aimUpLeft, true);
                break;
            case AimDirection.Right:
                player.animator.SetBool(Settings.aimRight, true);
                break;
            case AimDirection.Left:
                player.animator.SetBool(Settings.aimLeft, true);
                break;
            case AimDirection.Down:
                player.animator.SetBool(Settings.aimDown, true);
                break;
            default:
                break;
        }
    }

    private void InitialiseAimAnimationParameters()
    {
        player.animator.SetBool(Settings.aimUp, false);
        player.animator.SetBool(Settings.aimDown, false);
        player.animator.SetBool(Settings.aimLeft, false);
        player.animator.SetBool(Settings.aimRight, false);
        player.animator.SetBool(Settings.aimUpLeft, false);
        player.animator.SetBool(Settings.aimUpRight, false);
    }
}