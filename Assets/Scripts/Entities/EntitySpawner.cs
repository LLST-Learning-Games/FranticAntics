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
    public float maxDelaySeconds;
    public int maxActiveEntities;

    public bool spawnOnMe;

   
    private int spawnCount = 0;


    private float secondsUntilNextSpawn;
    private List<GameObject> activeEntities = new List<GameObject>();

    [SerializeField] private float spwanHeight = 50;
    public Transform topLeftSpawnPoint;
    public Transform bottomRightSpawnPoint;
    
    private float spawnXStart;
    private float spawnXEnd;
    
    private float spawnZStart;
    private float spawnZEnd;
    
    private void Start()
    {
        secondsUntilNextSpawn = initialDelaySeconds;

        spawnXStart = topLeftSpawnPoint.transform.position.x;
        spawnXEnd = bottomRightSpawnPoint.transform.position.x;
        
        spawnZStart = topLeftSpawnPoint.transform.position.z;
        spawnZEnd = bottomRightSpawnPoint.transform.position.z;
        
        // var rand = UnityEngine.Random.Range( spawnXStart, spawnXEnd);;
        //
        // Debug.LogWarning($"x {spawnXStart} {spawnXEnd} ");
        // Debug.LogWarning($"z {spawnZStart} {spawnZEnd}");
        
    }

    private void Update()
    {
        secondsUntilNextSpawn -= Time.deltaTime;
        RefreshActiveEntries();

        if (CanSpawnEntities())
        {
            SpawnRandomEntity();
        }


        // var oldPos = transform.position;
        // float newY = oldPos.y * Time.deltaTime * 0.01f;
        //
        // transform.position = new Vector3(oldPos.x, newY, oldPos.z);
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
        spawnCount++;
        secondsUntilNextSpawn = UnityEngine.Random.Range( minDelaySeconds, maxDelaySeconds);
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
        // todo have a look
        
        var randX = UnityEngine.Random.Range( spawnXStart, spawnXEnd);
        var randZ = UnityEngine.Random.Range( spawnZStart, spawnZEnd);
        
        
        // Debug.LogWarning($"result {randX} {spwanHeight} {randZ}");
        
        GameObject obj = Instantiate(entity.prefab, new Vector3(randX, spwanHeight, randZ), Quaternion.identity);
        
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
