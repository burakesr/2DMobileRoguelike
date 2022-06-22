using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(InstantiatedRoom))]
[DisallowMultipleComponent]
public class RoomLightingControl : MonoBehaviour
{
    private InstantiatedRoom instantiatedRoom;

    private void Awake()
    {
        instantiatedRoom = GetComponent<InstantiatedRoom>();
    }

    private void OnEnable()
    {
        StaticEventHandler.OnRoomChanged += OnRoomChanged;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnRoomChanged -= OnRoomChanged;
    }

    private void OnRoomChanged(RoomChangedEventArgs roomChangedEventArgs)
    {
        // If this is the room entered and the room isn't already lit, then fade in the room lighting.
        if (roomChangedEventArgs.room == instantiatedRoom.room && !instantiatedRoom.room.isList)
        {
            FadeInRoomLighting();

            FadeInDoors();

            instantiatedRoom.room.isList = true;
        }
    }

    private void FadeInDoors()
    {
        Door[] doors = GetComponentsInChildren<Door>();

        foreach (Door door in doors)
        {
            DoorLightingControl doorLightingControl = door.GetComponentInChildren<DoorLightingControl>();

            doorLightingControl.FadeInDoor(door);
        }
    }

    private void FadeInRoomLighting()
    {
        StartCoroutine(FadeInRoomLightingRoutine(instantiatedRoom));
    }

    private IEnumerator FadeInRoomLightingRoutine(InstantiatedRoom instantiatedRoom)
    {
        // Create a new material to fade in.
        Material material = new Material(GameResources.Instance.variableLitShader);
        
        ChangeTilemapMaterial(instantiatedRoom, material);

        for (float i = 0.05f; i <= 1.0f; i += Time.deltaTime / Settings.fadeInTime)
        {
            material.SetFloat("Alpha_Slider", i);
            yield return null;
        }

        ChangeTilemapMaterial(instantiatedRoom, GameResources.Instance.litMaterial);
    }

    private static void ChangeTilemapMaterial(InstantiatedRoom instantiatedRoom, Material material)
    {
        instantiatedRoom.groundTilemap.GetComponent<TilemapRenderer>().material = material;
        instantiatedRoom.decoration1Tilemap.GetComponent<TilemapRenderer>().material = material;
        instantiatedRoom.decoration2Tilemap.GetComponent<TilemapRenderer>().material = material;
        instantiatedRoom.frontTilemap.GetComponent<TilemapRenderer>().material = material;
        instantiatedRoom.minimapTilemap.GetComponent<TilemapRenderer>().material = material;
    }
}