using UnityEngine;

public static class Settings
{
    #region UNITS
    public const float pixelPerUnit = 16f;
    public const float tileSizePixels = 16f;
    #endregion

    #region DUNGEON BUILD SETTINGS

    public const int maxDungeonRebuildAttemptsForRoomGraph = 1000;
    public const int maxDungeonBuildAttempts = 10;

    #endregion

    #region ROOM SETTINGS
    public const float fadeInTime = 0.5f; // time to fade in the room
    [Tooltip("Max number of child corridors leading from a room.")]
    public const int maxChildCorridors = 3;

    #endregion ROOM SETTINGS

    #region ANIMATOR PARAMETERS
    public static int aimUp = Animator.StringToHash("aimUp");
    public static int aimDown = Animator.StringToHash("aimDown");
    public static int aimLeft = Animator.StringToHash("aimLeft");
    public static int aimRight = Animator.StringToHash("aimRight");
    public static int aimUpRight = Animator.StringToHash("aimUpRight");
    public static int aimUpLeft = Animator.StringToHash("aimUpLeft");
    public static int rollUp = Animator.StringToHash("rollUp");
    public static int rollDown = Animator.StringToHash("rollDown");
    public static int rollLeft = Animator.StringToHash("rollLeft");
    public static int rollRight = Animator.StringToHash("rollRight");
    public static int isIdle = Animator.StringToHash("isIdle");
    public static int isMoving = Animator.StringToHash("isMoving");

    // Animation parameters for Door
    public static int open = Animator.StringToHash("open");

    public static float baseSpeedForPlayerAnimations = 8.0f;
    public static float baseSpeedForEnemyAnimations = 3.0f;
    #endregion

    #region GAMEOBJECT TAGS
    public const string playerTag = "Player";
    public const string playerWeapon = "PlayerWeapon";
    #endregion

    #region UI PARAMETERS
    public const float uiAmmoSpacing = 4f;
    #endregion

    #region ASTART PATHFINDING PARAMETERS
    public const int defaultAStarMovementPenalty = 40;
    public const int prefferedAStarMovementPenalty = 1;
    public const int targetFrameRateToSpreadPathfindingOver = 60;
    public const float playerMovementDistanceToRebuildPath = 3f;
    public const float enemyPathRebuildCooldownTimer = 2f;
    #endregion

    #region FIRING CONTROL
    public const float useAimAngleDistance = 50f;
    #endregion
}