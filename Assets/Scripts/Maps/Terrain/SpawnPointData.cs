using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "SpawnPointData", menuName = "FrANTic/SpawnPointData", order = 1)]
public class SpawnPointData : ScriptableObject
{
    public Vector3 MapPosition;
    public MapObjectType Type;
}

public enum MapObjectType
{
    PlayerStartLocation,
    Obstacle,
    Resource,
}