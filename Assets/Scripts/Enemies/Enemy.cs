using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

#region REQUIRE COMPONENTS
[RequireComponent(typeof(SortingGroup))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(AnimateEnemy))]
[RequireComponent(typeof(EnemyMovementAI))]
[RequireComponent(typeof(MovementToPosition))]
[RequireComponent(typeof(MovementToPositionEvent))]
[RequireComponent(typeof(Idle))]
[RequireComponent(typeof(IdleEvent))]
#endregion
[DisallowMultipleComponent]
public class Enemy : MonoBehaviour
{
    private EnemyDetailsSO enemyDetails;
    private CircleCollider2D circleCollider;
    private PolygonCollider2D polygonCollider;
    private Animator animator;
    private SpriteRenderer[] spriteRenderers;

    private EnemyMovementAI enemyMovementAI;
    private MovementToPositionEvent movementToPositionEvent;
    private IdleEvent idleEvent;
    private AnimateEnemy animateEnemy;

    #region Properties
    public IdleEvent IdleEvent => idleEvent;
    public MovementToPositionEvent MovementToPositionEvent => movementToPositionEvent;
    public Animator Animator => animator;
    public CircleCollider2D CircleCollider => circleCollider;
    public EnemyDetailsSO EnemyDetails => enemyDetails;
    public AnimateEnemy AnimateEnemy => animateEnemy;
    #endregion

    public SpriteRenderer[] SpriteRenderers => spriteRenderers;

    private void Awake()
    {
        enemyMovementAI = GetComponent<EnemyMovementAI>();
        movementToPositionEvent = GetComponent<MovementToPositionEvent>();
        idleEvent = GetComponent<IdleEvent>();
        circleCollider = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        polygonCollider = GetComponent<PolygonCollider2D>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        animateEnemy = GetComponent<AnimateEnemy>();
    }

    public void InitialiseEnemy(EnemyDetailsSO enemyDetails, int enemySpawnNumber, DungeonLevelSO dungeonLevel)
    {
        this.enemyDetails = enemyDetails;

        SetEnemyMovementUpdateFrame(enemySpawnNumber);

        SetEnemyAnimationSpeed();
    }

    private void SetEnemyMovementUpdateFrame(int enemySpawnNumber)
    {
        enemyMovementAI.SetUpdateFrameNumber(enemySpawnNumber % Settings.targetFrameRateToSpreadPathfindingOver);
    }

    private void SetEnemyAnimationSpeed()
    {
        animator.speed = enemyMovementAI.MoveSpeed / Settings.baseSpeedForEnemyAnimations;
    }
}
