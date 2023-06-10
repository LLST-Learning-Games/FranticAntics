using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntitySpawnInfo
{
    Player Owner { get; }
    SpawnLocation SpawnLocation { get; }
    GameObject Prefab { get; }
}
