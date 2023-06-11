using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [Header("Entities")]
    public List<Entity> spawnableEntities = new List<Entity>();

    [Header("Criteria")]
    public float initialDelaySeconds;
    public float minDelaySeconds;
    public int maxActiveEntities;
    public bool spawnOnMe;

    private float secondsUntilNextSpawn;
    private List<GameObject> activeEntities = new List<GameObject>();

    private void Start()
    {
        secondsUntilNextSpawn = initialDelaySeconds;
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
                continue;
            }
        }
    }

    private void SpawnRandomEntity()
    {
        Entity entityToSpawn = PickRandomEntity();
        SpawnEntity(entityToSpawn);
        secondsUntilNextSpawn = minDelaySeconds;
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

        if (!spawnOnMe)
            obj.transform.SetParent(transform.parent, true);
        activeEntities.Add(obj);
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
