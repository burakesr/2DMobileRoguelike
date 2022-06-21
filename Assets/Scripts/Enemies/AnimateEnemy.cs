using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
[DisallowMultipleComponent]
public class AnimateEnemy : MonoBehaviour
{
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        enemy.MovementToPositionEvent.OnMovementToPosition += OnMovementToPosition;
        enemy.IdleEvent.OnIdle += OnIdle;
    }

    private void OnDisable()
    {
        enemy.MovementToPositionEvent.OnMovementToPosition -= OnMovementToPosition;
        enemy.IdleEvent.OnIdle -= OnIdle;
    }

    private void OnMovementToPosition(MovementToPositionEvent movementToPositionEvent, MovementToPositionArgs movementToPositionArgs)
    {
        if (enemy.transform.position.x < GameManager.Instance.GetPlayer().GetPlayerPosition().x)
        {
            SetAimWeaponAnimationParameters(AimDirection.Right);
        }
        else
        {
            SetAimWeaponAnimationParameters(AimDirection.Left);
        }

        SetMovementAnimationParameters();
    }


    private void OnIdle(IdleEvent idleEvent)
    {
        SetIdleAnimationParameters();
    }

    private void InitialiseAimAnimationParameters()
    {
        enemy.Animator.SetBool(Settings.aimUp, false);
        enemy.Animator.SetBool(Settings.aimDown, false);
        enemy.Animator.SetBool(Settings.aimLeft, false);
        enemy.Animator.SetBool(Settings.aimRight, false);
        enemy.Animator.SetBool(Settings.aimUpLeft, false);
        enemy.Animator.SetBool(Settings.aimUpRight, false);
    }

    private void SetMovementAnimationParameters()
    {
        enemy.Animator.SetBool(Settings.isIdle, false);
        enemy.Animator.SetBool(Settings.isMoving, true);
    }

    private void SetIdleAnimationParameters()
    {
        enemy.Animator.SetBool(Settings.isIdle, true);
        enemy.Animator.SetBool(Settings.isMoving, false);
    }

    private void SetAimWeaponAnimationParameters(AimDirection aimDirection)
    {
        InitialiseAimAnimationParameters();

        switch (aimDirection)
        {   
            case AimDirection.Up:
                enemy.Animator.SetBool(Settings.aimUp, true);
                break;
            case AimDirection.UpRight:
                enemy.Animator.SetBool(Settings.aimUpRight, true);
                break;
            case AimDirection.UpLeft:
                enemy.Animator.SetBool(Settings.aimUpLeft, true);
                break;
            case AimDirection.Right:
                enemy.Animator.SetBool(Settings.aimRight, true);
                break;
            case AimDirection.Left:
                enemy.Animator.SetBool(Settings.aimLeft, true);
                break;
            case AimDirection.Down:
                enemy.Animator.SetBool(Settings.aimDown, true);
                break;
        }
    }
}
