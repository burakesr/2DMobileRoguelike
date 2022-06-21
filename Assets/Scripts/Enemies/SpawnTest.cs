using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnTest : MonoBehaviour
{
    private List<SpawnableObjectByLevel<EnemyDetailsSO>> testLevelSpawnList;
    private RandomSpawnableObject<EnemyDetailsSO> randomEnemyHelperClass;
    private List<GameObject> instantiatedEnemies = new List<GameObject>();

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
        if (instantiatedEnemies != null && instantiatedEnemies.Count > 0)
        {
            foreach (GameObject enemy in instantiatedEnemies)
            {
                Destroy(enemy);
            }
        }

        RoomTemplateSO roomTemplate = DungeonBuilder.Instance.GetRoomTemplate(roomChangedEventArgs.room.templateID);

        if (roomTemplate != null)
        {
            testLevelSpawnList = roomTemplate.enemiesByLevelList;
            // Create RandomSpawnableObect helper class
            randomEnemyHelperClass = new RandomSpawnableObject<EnemyDetailsSO>(testLevelSpawnList);
        }
    }

    private void Update()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            EnemyDetailsSO enemy = randomEnemyHelperClass.GetItem();

            if (enemy != null)
            {
                instantiatedEnemies.Add(Instantiate(enemy.enemyPrefab,
                    HelperUtilities.GetSpawnPositionNearestToPlayer(HelperUtilities.GetMouseWorldPosition()),
                    Quaternion.identity));
            }
        }
    }
}
