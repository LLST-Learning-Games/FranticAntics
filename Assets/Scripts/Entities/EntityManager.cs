using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public static EntityManager instance { get; private set; }

    private LinkedList<IScheduledSpawn> scheduledSpawns = new LinkedList<IScheduledSpawn>();

    // Start is called before the first frame update
    private void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    private void Update()
    {
        float deltaTime = Time.deltaTime;
        UpdateScheduledSpawns(deltaTime);
    }

    private void UpdateScheduledSpawns(float deltaTime)
    {
        foreach(var scheduledSpawn in scheduledSpawns.ToList())
        {
            scheduledSpawn.SecondsRemaining -= deltaTime;

            if (scheduledSpawn.SecondsRemaining <= 0.0f)
            {
                scheduledSpawns.Remove(scheduledSpawn);
                SpawnEntity(scheduledSpawn);
            }
        }
    }

    public void SpawnEntity(IEntitySpawnInfo spawnInfo)
    {
        GameObject obj = Instantiate(spawnInfo.Prefab, spawnInfo.SpawnLocation.transform, false);
        Entity entity = obj.GetComponent<Entity>();
        entity.InitializeEntity(spawnInfo);
    }

    public void ScheduleSpawn(IScheduledSpawn scheduledSpawn)
    {
        scheduledSpawns.AddLast(scheduledSpawn);
    }
}
