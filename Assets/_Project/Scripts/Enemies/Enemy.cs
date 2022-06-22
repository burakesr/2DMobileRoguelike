using System.Collections;
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
[RequireComponent(typeof(AimWeaponEvent))]
[RequireComponent(typeof(FireWeaponEvent))]
#endregion
[DisallowMultipleComponent]
public class Enemy : MonoBehaviour
{
    private EnemyDetailsSO enemyDetails;
    private CircleCollider2D circleCollider;
    private PolygonCollider2D polygonCollider;
    private Animator animator;
    private SpriteRenderer[] spriteRenderers;

    private MaterialiseEffect materialiseEffect;
    private EnemyMovementAI enemyMovementAI;
    private MovementToPositionEvent movementToPositionEvent;
    private IdleEvent idleEvent;
    private AnimateEnemy animateEnemy;
    private AimWeaponEvent aimWeaponEvent;
    private FireWeaponEvent fireWeaponEvent;

    #region Properties
    public IdleEvent IdleEvent => idleEvent;
    public MovementToPositionEvent MovementToPositionEvent => movementToPositionEvent;
    public Animator Animator => animator;
    public CircleCollider2D CircleCollider => circleCollider;
    public EnemyDetailsSO EnemyDetails => enemyDetails;
    public AnimateEnemy AnimateEnemy => animateEnemy;
    public AimWeaponEvent AimWeaponEvent => aimWeaponEvent;
    public FireWeaponEvent FireWeaponEvent => fireWeaponEvent;
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
        materialiseEffect = GetComponent<MaterialiseEffect>();
    }

    public void InitialiseEnemy(EnemyDetailsSO enemyDetails, int enemySpawnNumber, DungeonLevelSO dungeonLevel)
    {
        this.enemyDetails = enemyDetails;

        SetEnemyMovementUpdateFrame(enemySpawnNumber);

        SetEnemyAnimationSpeed();

        StartCoroutine(Materialise());
    }


    private void SetEnemyMovementUpdateFrame(int enemySpawnNumber)
    {
        enemyMovementAI.SetUpdateFrameNumber(enemySpawnNumber % Settings.targetFrameRateToSpreadPathfindingOver);
    }

    private void SetEnemyAnimationSpeed()
    {
        animator.speed = enemyMovementAI.MoveSpeed / Settings.baseSpeedForEnemyAnimations;
    }

    private IEnumerator Materialise()
    {
        EnemyEnable(false);

        yield return StartCoroutine(materialiseEffect.MaterialiseRoutine(enemyDetails.enemyMaterialiseShader, enemyDetails.enemyMaterialiseColor,
            enemyDetails.enemyMaterialiseTime, spriteRenderers, enemyDetails.enemyStandardMaterial));

        EnemyEnable(true);
    }

    private void EnemyEnable(bool enable)
    {
        circleCollider.enabled = enable;
        polygonCollider.enabled = enable;
        enemyMovementAI.enabled = enable;
    }
}
