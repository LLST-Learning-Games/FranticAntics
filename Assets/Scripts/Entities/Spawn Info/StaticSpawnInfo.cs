using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StaticSpawnInfo : IEntitySpawnInfo
{
    public Player owner;
    public Entity prefab;
    public SpawnLocation spawnLocation;

    Player IEntitySpawnInfo.Owner => owner;
    GameObject IEntitySpawnInfo.Prefab => prefab.gameObject;
    SpawnLocation IEntitySpawnInfo.SpawnLocation => spawnLocation;
}
