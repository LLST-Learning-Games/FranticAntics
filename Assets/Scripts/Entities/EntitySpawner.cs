using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using Unity.Mathematics;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class EntitySpawner : MonoBehaviour
{
    [Header("Entities")]
    public List<Entity> spawnableEntities = new List<Entity>();

    [Header("Criteria")]
    public float initialDelaySeconds;
    public float minDelaySeconds = 2;
    public float maxDelaySeconds = 10;
    public int maxActiveEntities;

    public bool spawnOnMe;

   
    private int spawnCount = 0;
    
    private float secondsUntilNextSpawn;
    private List<GameObject> activeEntities = new List<GameObject>();
    
    private void Start()
    {
        secondsUntilNextSpawn = initialDelaySeconds + UnityEngine.Random.Range(minDelaySeconds, maxDelaySeconds);
    }

    private void Update()
    {
        secondsUntilNextSpawn -= Time.deltaTime;
        RefreshActiveEntries();

        if (CanSpawnEntities())
        {
            SpawnRandomEntity();
        }
    }

    private bool CanSpawnEntities()
    {
        if (secondsUntilNextSpawn > 0.0f)
        {
            return false;
        }

        if (maxActiveEntities > 0 && activeEntities.Count >= maxActiveEntities)
        {
            return false;
        }

        return true;
    }

    private void RefreshActiveEntries()
    {
        for (int i = 0; i < activeEntities.Count; ++i)
        {
            GameObject entity = activeEntities[i];

            if (!entity)
            {
                activeEntities.RemoveAt(i--);
                
                // prevent immediate drop items
                secondsUntilNextSpawn = UnityEngine.Random.Range(minDelaySeconds, maxDelaySeconds);
                
                continue;
            }
        }
    }

    private void SpawnRandomEntity()
    {
        Entity entityToSpawn = PickRandomEntity();
        SpawnEntity(entityToSpawn);
        spawnCount++;
        secondsUntilNextSpawn = UnityEngine.Random.Range(minDelaySeconds, maxDelaySeconds);
    }

    private Entity PickRandomEntity()
    {
        float totalWeight = 0.0f;

        foreach(var entity in spawnableEntities)
        {
            totalWeight += entity.weight;
        }

        float randomWeight = UnityEngine.Random.Range(0.0f, totalWeight);

        foreach(var entity in spawnableEntities)
        {
            randomWeight -= entity.weight;

            if (randomWeight <= 0.0f)
            {
                return entity;
            }
        }

        Debug.LogError("Did not pick random entity");
        return spawnableEntities[0];
    }

    protected virtual void SpawnEntity(Entity entity)
    {
        GameObject obj = Instantiate(entity.prefab, transform, false);
        
        SpawnableEntitiy spawnableEntity = obj.GetComponent<SpawnableEntitiy>();

        
        var startPos = transform.position;

        var endingPos = new UnityEngine.Vector3(startPos.x, 0.0f, startPos.z);
        
        // Debug.LogWarning($"startPos: {startPos}");
        // Debug.LogWarning($"endingPos: {endingPos}");
        
        spawnableEntity.Initialise(startPos, endingPos);
        
        
        if (!spawnOnMe)
            obj.transform.SetParent(transform.parent, true);
        activeEntities.Add(obj);



        // old code
        //GameObject obj = Instantiate(entity.prefab, transform, false);

        //if (!spawnOnMe)
        //    obj.transform.SetParent(transform.parent, true);
        //activeEntities.Add(obj);

        
    }

    #region Embedded Types
    [Serializable]
    public class Entity
    {
        public GameObject prefab;
        public float weight;
    }
    #endregion
}
