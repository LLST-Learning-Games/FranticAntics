using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    public LevelSpawnData LevelSpawnData;
    public List<SpawnPoint> ActiveSpawnPoints;
    public SpawnPoint SpawnPrefab;

    public GameObject Player1Queen;
    public GameObject Player2Queen;
    
    private bool player1Assigned;
    private bool player2Assigned;
    
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

    public void TryAssignToPlayer(int playerIndex, Vector3 startPosition)
    {
        if (playerIndex == 1 && !player1Assigned)
        {
            Player1Queen.transform.position = startPosition;
            player1Assigned = true;
        }

        if (playerIndex == 2 && !player2Assigned)
        {
            Player2Queen.transform.position = startPosition;
            player2Assigned = true;
        }
    }
}
