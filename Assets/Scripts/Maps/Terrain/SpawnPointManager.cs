using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    public LevelSpawnData LevelSpawnData;
    public List<SpawnPoint> ActiveSpawnPoints;
    public SpawnPoint SpawnPrefab;
    
    #region SINGLETON
    public static SpawnPointManager Instance;

    void Awake()
    {
        if (Instance != null)
            return;
        Instance = this;
    }
    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (SpawnPointData spawnPointData in LevelSpawnData.LevelSpawnPoints)
        {
            SpawnPoint newSpawn = Instantiate(SpawnPrefab);
            newSpawn.transform.parent = transform;
            newSpawn.Initialize(spawnPointData);
            newSpawn.gameObject.name = spawnPointData.name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
