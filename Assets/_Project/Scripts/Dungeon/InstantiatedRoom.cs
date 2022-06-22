using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[DisallowMultipleComponent]
[RequireComponent(typeof(BoxCollider2D))]
public class InstantiatedRoom : MonoBehaviour
{
    public Room room;
    public Grid grid;
    public Tilemap tilemap;
    public Tilemap groundTilemap;
    public Tilemap decoration1Tilemap;
    public Tilemap decoration2Tilemap;
    public Tilemap frontTilemap;
    public Tilemap collisionTilemap;
    public Tilemap minimapTilemap;
    public Bounds roomCollisionBounds;
    public int[,] aStarMovementPenalty;
    public int[,] aStarItemObstacles;

    private BoxCollider2D boxCollider2D;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();

        // Save room collider bounds
        roomCollisionBounds = boxCollider2D.bounds;
    }

    // Trigger room changed event when player enters the room
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && room != GameManager.Instance.GetCurrentRoom())
        {
            this.room.isPreviouslyVisited = true;

            StaticEventHandler.CallRoomChangedEvent(room);
        }   
    }

    public void Initialise(GameObject roomGameObject)
    {
        PopulateTilemapMemberVariables(roomGameObject);

        BlockoffUnusedDoorways();

        AddObstaclesAndPrefferedPaths();

        AddDoorsToRooms();

        DisableCollisionTilemapRenderer();
    }

    private void AddDoorsToRooms()
    {
        // If the room is corridor then return.
        if (room.roomNodeType.isCorridorEW || room.roomNodeType.isCorridorNS)
        {
            return;
        }

        foreach (Doorway doorway in room.doorwayList)
        {
            if (doorway.doorPrefab != null && doorway.isConnected)
            {
                float tileDistance = Settings.tileSizePixels / Settings.pixelPerUnit;

                GameObject door = null;

                if (doorway.orientation == Orientation.North)
                {
                    door = Instantiate(doorway.doorPrefab, gameObject.transform);
                    door.transform.localPosition = new Vector3(doorway.position.x + tileDistance / 2f, doorway.position.y + tileDistance, 0f);
                }
                else if (doorway.orientation == Orientation.South)
                {
                    door = Instantiate(doorway.doorPrefab, gameObject.transform);
                    door.transform.localPosition = new Vector3(doorway.position.x + tileDistance / 2f, doorway.position.y, 0f);
                }
                else if (doorway.orientation == Orientation.East)
                {
                    door = Instantiate(doorway.doorPrefab, gameObject.transform);
                    door.transform.localPosition = new Vector3(doorway.position.x + tileDistance, doorway.position.y + tileDistance * 1.25f, 0f);
                }
                else if (doorway.orientation == Orientation.West)
                {
                    door = Instantiate(doorway.doorPrefab, gameObject.transform);
                    door.transform.localPosition = new Vector3(doorway.position.x, doorway.position.y + tileDistance * 1.25f, 0f);
                }

                Door doorComponent = door.GetComponent<Door>();

                // If parent room is boss room
                if (room.roomNodeType.isBossRoom)
                {
                    doorComponent.isBossRoomDoor = true;

                    // Lock the door to prevent the access to the room
                    doorComponent.LockDoor();
                }
            }
        }
    }

    private void PopulateTilemapMemberVariables(GameObject roomGameObject)
    {
        // Get the grid component
        grid = GetComponentInChildren<Grid>();

        // Get tilemaps in the children
        Tilemap[] tilemaps = roomGameObject.GetComponentsInChildren<Tilemap>();

        foreach (Tilemap tilemap in tilemaps)
        {
            if (tilemap.gameObject.CompareTag("groundTilemap"))
            {
                groundTilemap = tilemap;
            }
            else if (tilemap.gameObject.CompareTag("decoration1Tilemap"))
            {
                decoration1Tilemap = tilemap;
            }
            else if (tilemap.gameObject.CompareTag("decoration2Tilemap"))
            {
                decoration2Tilemap = tilemap;
            }
            else if (tilemap.gameObject.CompareTag("frontTilemap"))
            {
                frontTilemap = tilemap;
            }
            else if (tilemap.gameObject.CompareTag("collisionTilemap"))
            {
                collisionTilemap = tilemap;
            }
            else if (tilemap.gameObject.CompareTag("minimapTilemap"))
            {
                minimapTilemap = tilemap;
            }
        }
    }

    private void DisableCollisionTilemapRenderer()
    {
        collisionTilemap.gameObject.GetComponent<TilemapRenderer>().enabled = false;
    }

    private void BlockoffUnusedDoorways()
    {
        foreach (Doorway doorway in room.doorwayList)
        {
            if (doorway.isConnected) continue;

            if (collisionTilemap != null)
            {
                BlockAllDoorwayOnTilemapLayer(collisionTilemap, doorway);
            }

            if (minimapTilemap != null)
            {
                BlockAllDoorwayOnTilemapLayer(minimapTilemap, doorway);
            }

            if (groundTilemap != null)
            {
                BlockAllDoorwayOnTilemapLayer(groundTilemap, doorway);
            }

            if (decoration1Tilemap != null)
            {
                BlockAllDoorwayOnTilemapLayer(decoration1Tilemap, doorway);
            }

            if (decoration2Tilemap != null)
            {
                BlockAllDoorwayOnTilemapLayer(decoration2Tilemap, doorway);
            }

            if (frontTilemap != null)
            {
                BlockAllDoorwayOnTilemapLayer(frontTilemap, doorway);
            }
        }
    }

    private void BlockAllDoorwayOnTilemapLayer(Tilemap tilemap, Doorway doorway)
    {
        switch (doorway.orientation)
        {
            case Orientation.North:
            case Orientation.South:
                BlockDoorwayHorizontally(tilemap, doorway);
                break;

            case Orientation.East:
            case Orientation.West:
                BlockDoorwayVertically(tilemap, doorway);
                break;

            case Orientation.None:
                break;

            default:
                break;
        }
    }

    private void BlockDoorwayHorizontally(Tilemap tilemap, Doorway doorway)
    {
        Vector2Int startPosition = doorway.doorwayStartCopyPosition;

        // looping through all tiles to copy
        for (int xPos = 0; xPos < doorway.doorwayCopyTileWidth; xPos++)
        {
            for (int yPos = 0; yPos < doorway.doorwayCopyTileHeight; yPos++)
            {
                // Get rotation of tile being copied.
                Matrix4x4 transformMatrix = tilemap.GetTransformMatrix(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0));

                // copy tile
                tilemap.SetTile(new Vector3Int(startPosition.x + 1 + xPos, startPosition.y - yPos, 0),
                    tilemap.GetTile(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0)));

                // set rotation of tile copied
                tilemap.SetTransformMatrix(new Vector3Int(startPosition.x + 1, startPosition.y - yPos, 0), transformMatrix);
            }
        }
    }

    private void BlockDoorwayVertically(Tilemap tilemap, Doorway doorway)
    {
        Vector2Int startPosition = doorway.doorwayStartCopyPosition;

        for (int yPos = 0; yPos < doorway.doorwayCopyTileHeight; yPos++)
        {
            for (int xPos = 0; xPos < doorway.doorwayCopyTileWidth; xPos++)
            {
                 // Get rotation of tile being copied.
                Matrix4x4 transformMatrix = tilemap.GetTransformMatrix(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0));

                // copy tile
                tilemap.SetTile(new Vector3Int(startPosition.x + xPos, startPosition.y - 1 - yPos, 0),
                    tilemap.GetTile(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0)));

                // set rotation of tile copied
                tilemap.SetTransformMatrix(new Vector3Int(startPosition.x + xPos, startPosition.y - 1 - yPos, 0), transformMatrix);
            }
        }
    }

    private void AddObstaclesAndPrefferedPaths()
    {
        aStarMovementPenalty = new int[room.templateUpperBounds.x - room.templateLowerBounds.x + 1, 
            room.templateUpperBounds.y - room.templateLowerBounds.y + 1];

        for (int x = 0; x < room.templateUpperBounds.x - room.templateLowerBounds.x + 1; x++)
        {
            for (int y = 0; y < room.templateUpperBounds.y - room.templateLowerBounds.y + 1; y++)
            {
                aStarMovementPenalty[x, y] = Settings.defaultAStarMovementPenalty;

                TileBase tile = collisionTilemap.GetTile(new Vector3Int(x + room.templateLowerBounds.x, y + room.templateLowerBounds.y, 0));

                foreach (TileBase collisionTile in GameResources.Instance.enemyUnwalkableCollisionTilesArray)
                {
                    if (tile == collisionTile)
                    {
                        aStarMovementPenalty[x, y] = 0;
                        break;
                    }
                }

                // Add preffered path for enemies (1 is the preferred path value, default value for a grid location is specified in the Settings.
                if (tile == GameResources.Instance.preferredEnemyPathTile)
                {
                    aStarMovementPenalty[x, y] = Settings.prefferedAStarMovementPenalty;
                }
            }
        }
    }

    private void DisableRoomCollider()
    {
        boxCollider2D.enabled = false;
    }

    public void LockDoors()
    {
        Door[] doors = GetComponentsInChildren<Door>();

        foreach (Door door in doors)
        {
            door.LockDoor();
        }

        DisableRoomCollider();
    }
}