using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : SingletonMonoBehaviour<GameManager>
{
    #region Header DUNGEON LEVELS
    [Header("DUNGEON LEVELS")]
    [Space(10)]
    #endregion Header DUNGEON LEVELS
    #region Tooltip

    [Tooltip("Populate with the dungeon level scriptable objects.")]

    #endregion Tooltip
    [SerializeField] private List<DungeonLevelSO> dungeonLevelList;

    private Room currentRoom;
    private Room previousRoom;
    private PlayerDetailsSO playerDetails;
    private Player player;

    #region Tooltip

    [Tooltip("Populate with the starting dungeon level for testing, first level = 0")]

    #endregion Tooltip
    [SerializeField] private int currentDungeonLevelListIndex = 0;

    private GameStates currentGameState;
    private GameStates previousGameState;
    public GameStates CurrentGameState
    {
        get { return currentGameState; }
        set { currentGameState = value; }
    }
    public GameStates PreviousGameState
    {
        get
        {
            return previousGameState;
        }
        set
        {
            previousGameState = value;
        }
    }


    protected override void Awake()
    {
        base.Awake();

#if PLATFORM_ANDROID
        Application.targetFrameRate = 60;
#endif
#if UNITY_ANDROID
        Application.targetFrameRate = 60;
#endif
#if UNITY_EDITOR
        Application.targetFrameRate = -1;
#endif
        playerDetails = GameResources.Instance.currentPlayer.playerDetails;

        InstantiatePlayer();
    }

    private void OnEnable()
    {
        StaticEventHandler.OnRoomChanged += OnRoomChanged;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnRoomChanged -= OnRoomChanged;
    }

    private void Start()
    {
        previousGameState = GameStates.GameStarted;
        currentGameState = GameStates.GameStarted;
    }

    private void Update()
    {
        HandleGameState();

        // For debugging
        //if (Keyboard.current.pKey.wasPressedThisFrame)
        //{
        //    gameState = GameStates.GameStarted;
        //}
    }

    private void InstantiatePlayer()
    {
        GameObject playerInstance = Instantiate(playerDetails.playerPrefab);

        player = playerInstance.GetComponent<Player>();
        player.Initalise(playerDetails);
    }

    private void HandleGameState()
    {
        switch (currentGameState)
        {
            case GameStates.GameStarted:

                PlayDungeonLevel(currentDungeonLevelListIndex);
                currentGameState = GameStates.PlayingLevel;

                break;

            case GameStates.PlayingLevel:
                break;

            case GameStates.EngagingEnemies:
                break;

            case GameStates.BossStage:
                break;

            case GameStates.EngagingBoss:
                break;

            case GameStates.LevelCompleted:
                break;

            case GameStates.GameWon:
                break;

            case GameStates.GameLost:
                break;

            case GameStates.GamePaused:
                break;

            case GameStates.DungeonOverviewMap:
                break;

            case GameStates.RestartGame:
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Set the current room the player in in
    /// </summary>
    public void SetCurrentRoom(Room room)
    {
        previousRoom = currentRoom;
        currentRoom = room;
    }

    private void PlayDungeonLevel(int dungeonLevelListIndex)
    {
        // Build dungeon for level
        bool dungeonBuiltSuccessfully = DungeonBuilder.Instance.GenerateDungeon(dungeonLevelList[dungeonLevelListIndex]);

        if (!dungeonBuiltSuccessfully)
        {
            Debug.LogError("Couldn't build dungeon from specified rooms and node graphs.");
        }

        // Call static event that room has changed
        StaticEventHandler.CallRoomChangedEvent(currentRoom);

        // Set player position roughly middle of the room
        player.transform.position = new Vector3((currentRoom.lowerBounds.x + currentRoom.upperBounds.x) / 2.0f,
            (currentRoom.lowerBounds.y + currentRoom.upperBounds.y) / 2.0f, 0.0f);

        // Get spawn position in room that is nearest to player
        player.transform.position = HelperUtilities.GetSpawnPositionNearestToPlayer(player.transform.position);
    }

    private void OnRoomChanged(RoomChangedEventArgs roomChangedEventArgs)
    {
        SetCurrentRoom(roomChangedEventArgs.room);
    }

    public Room GetCurrentRoom() => currentRoom;

    public Player GetPlayer() => player;

    public DungeonLevelSO GetCurrentDungeonLevel()
    {
        return dungeonLevelList[currentDungeonLevelListIndex];
    }

    public Sprite GetPlayerMinimapIcon()
    {
        return playerDetails.playerMinimapIcon;
    }
    #region Validation

#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(dungeonLevelList), dungeonLevelList);
    }

#endif

    #endregion Validation
}