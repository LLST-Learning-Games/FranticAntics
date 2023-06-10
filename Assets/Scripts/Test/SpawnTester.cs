using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTester : MonoBehaviour
{
    [SerializeField] private StaticSpawnInfo entityToSpawn;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            SpawnNow();
        }
        
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            SpawnInSeconds(3f);
        }
    }

    private void SpawnNow()
    {
        EntityManager.instance.SpawnEntity(entityToSpawn);
    }

    private void SpawnInSeconds(float seconds)
    {
        ScheduledSpawnInfo scheduledSpawn = new ScheduledSpawnInfo()
        {
            owner = entityToSpawn.owner,
            prefab = entityToSpawn.prefab,
            spawnLocation = entityToSpawn.spawnLocation,
            timeToSpawn = seconds,
        };

        EntityManager.instance.ScheduleSpawn(scheduledSpawn);
    }
}
