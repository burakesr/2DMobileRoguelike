using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
[DisallowMultipleComponent]
public class EnemyMovementAI : MonoBehaviour
{
    [SerializeField] private MovementDetailsSO movementDetails;
    
    private Enemy enemy;
    
    private Stack<Vector3> movementSteps = new Stack<Vector3>();
    private Vector3 playerReferencePos;
    private WaitForFixedUpdate waitForFixedUpdate;
    private Coroutine moveEnemyRoutine;

    private float currentPathRebuildCooldown;
    private float moveSpeed;
    private bool chasePlayer;

    [HideInInspector] public int UpdateFrameRate = 1;
    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }
        set
        {
            moveSpeed = value;
        }
    }

    private void Awake()
    {
        enemy = GetComponent<Enemy>();

        moveSpeed = movementDetails.GetMoveSpeed();
    }

    private void Start()
    {
        waitForFixedUpdate = new WaitForFixedUpdate();

        playerReferencePos = GameManager.Instance.GetPlayer().GetPlayerPosition();
    }

    private void Update()
    {
        MoveEnemy();
    }

    private void MoveEnemy()
    {
        currentPathRebuildCooldown -= Time.deltaTime;

        if (!chasePlayer && Vector3.Distance(transform.position, GameManager.Instance.GetPlayer().GetPlayerPosition()) < enemy.EnemyDetails.chaseDistance)
        {
            chasePlayer = true;
        }

        if (!chasePlayer) return;

        // Only process A Star path rebuild on certain frames to spread the load between enemies
        if (Time.frameCount % Settings.targetFrameRateToSpreadPathfindingOver != UpdateFrameRate) return;

        if (currentPathRebuildCooldown <= 0f || (Vector3.Distance(playerReferencePos, GameManager.Instance.GetPlayer().GetPlayerPosition()) 
            > Settings.playerMovementDistanceToRebuildPath))
        {
            currentPathRebuildCooldown = Settings.enemyPathRebuildCooldownTimer;

            playerReferencePos = GameManager.Instance.GetPlayer().GetPlayerPosition();

            CreatePath();

            if (movementSteps != null)
            {
                if (moveEnemyRoutine != null)
                {
                    enemy.IdleEvent.CallIdleEvent();
                    StopCoroutine(moveEnemyRoutine);
                }

                moveEnemyRoutine = StartCoroutine(MoveEnemyRoutine(movementSteps));
            }
        }
    }

    private void CreatePath()
    {
        Room currentRoom = GameManager.Instance.GetCurrentRoom();

        Grid grid = currentRoom.instantiatedRoom.grid;

        Vector3Int targetGridPos = GetNearestNonObstalcePlayerPos(currentRoom);

        Vector3Int enemyGridPos = grid.WorldToCell(transform.position);

        movementSteps = AStar.BuildPath(currentRoom, enemyGridPos, targetGridPos);

        if (movementSteps != null)
        {
            movementSteps.Pop();
        }
        else
        {
            enemy.IdleEvent.CallIdleEvent();
        }
    }

    public void SetUpdateFrameNumber(int updateFrameNumber)
    {
        UpdateFrameRate = updateFrameNumber;
    }

    /// <summary>
    /// Get the nearest position to the player that isn't on an obstacle 
    /// </summary>
    public Vector3Int GetNearestNonObstalcePlayerPos(Room currentRoom)
    {
        Vector3 playerPos = GameManager.Instance.GetPlayer().GetPlayerPosition();

        Vector3Int playerCellPos = currentRoom.instantiatedRoom.grid.WorldToCell(playerPos);

        Vector2Int adjustedPlayerCellPos = new Vector2Int(playerCellPos.x - currentRoom.templateLowerBounds.x, 
            playerCellPos.y - currentRoom.templateLowerBounds.y);

        int obstacle = currentRoom.instantiatedRoom.aStarMovementPenalty[adjustedPlayerCellPos.x, adjustedPlayerCellPos.y];

        // If player isn't on a cell square that marked as a obstacle the return that position.
        if (obstacle != 0)
        {
            return playerCellPos;
        }
        // Find a surrounding cell that isn't an obstalce - required because with the 'half collision' tiles
        // the player can be on a grid square that is marked as an obstacle
        else
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;

                    try
                    {
                        obstacle = currentRoom.instantiatedRoom.aStarMovementPenalty[adjustedPlayerCellPos.x + i, adjustedPlayerCellPos.y + j];
                        if (obstacle != 0) return new Vector3Int(playerCellPos.x + i, playerCellPos.y + j, 0);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }

            return playerCellPos;
        }
    }

    private IEnumerator MoveEnemyRoutine(Stack<Vector3> steps)
    {
        while (steps.Count > 0)
        {
            Vector3 nextPos = steps.Pop();

            // while not very close to target continue to move - when close onto the next step
            while (Vector3.Distance(nextPos, transform.position) > 0.2f)
            {
                enemy.MovementToPositionEvent.CallMovementToPositionEvent(nextPos, transform.position, (nextPos - transform.position).normalized, moveSpeed, false);

                yield return waitForFixedUpdate;
            }
            yield return waitForFixedUpdate;
        }

        enemy.IdleEvent.CallIdleEvent();
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(movementDetails), movementDetails);
    }
#endif
    #endregion
}
