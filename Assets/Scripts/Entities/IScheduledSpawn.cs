using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScheduledSpawn : IEntitySpawnInfo
{
    float SecondsRemaining { get; set; }
}
