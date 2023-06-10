using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduledSpawnInfo : StaticSpawnInfo, IScheduledSpawn
{
    public float timeToSpawn;

    float IScheduledSpawn.SecondsRemaining
    {
        get => timeToSpawn;
        set => timeToSpawn = value;
    }
}