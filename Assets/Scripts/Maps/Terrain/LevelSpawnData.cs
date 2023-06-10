using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnData", menuName = "FrANTic/SpawnData", order = 1)]
public class LevelSpawnData : ScriptableObject
{
    public List<SpawnPointData> LevelSpawnPoints;
}