using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Player owner { get; private set; }

    public virtual void InitializeEntity(IEntitySpawnInfo spawnInfo)
    {
        owner = spawnInfo.Owner;
    }
}
