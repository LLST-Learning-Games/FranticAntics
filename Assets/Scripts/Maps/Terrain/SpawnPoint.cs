using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnPoint : MonoBehaviour
{
    private MapObjectType type = MapObjectType.Obstacle;
    private Vector3 initRotation;
    
    // Start is called before the first frame update
    public void Initialize(SpawnPointData initData)
    {
        if (initData == null)
        {
            Debug.LogError($"[SPAWNPOINT] No init data for {this}");
            return;
        }

        transform.position = initData.MapPosition;
        type = initData.Type;
        
        if (initData.RotationOverride != Vector3.zero)
        {
            initRotation = initData.RotationOverride;
        }
        else
        {
            float newYRotation = Random.Range(0, 360);
            initRotation = new Vector3(0, newYRotation, 0);
        }
        SpawnVisual(initData.PrefabOverride);
    }

    private void SpawnVisual(GameObject prefabOverride = null)
    {

        
        if (prefabOverride != null)
        {
            GameObject newVisual = Instantiate(prefabOverride);
            SetVisualPosition(newVisual);
            return;
        }
        
        switch (type)
        {
            case MapObjectType.Obstacle:
                SpawnObstacle();
                break;
            case MapObjectType.PlayerStartLocation:
                break;
            case MapObjectType.Resource:
                break;
        }
    }

    private void SpawnObstacle()
    {
        int randomIndex = Random.Range(0, SpawnPointManager.Instance.LevelSpawnData.ObstaclePrefabs.Count - 1);
        GameObject obstacle = Instantiate(SpawnPointManager.Instance.LevelSpawnData.ObstaclePrefabs[randomIndex]);
        SetVisualPosition(obstacle);
    }

    private void SetVisualPosition(GameObject visualObject)
    {
        visualObject.transform.parent = transform;
        Vector3 terrainPos = GetTerrainPos(transform.position.x, transform.position.z);
        visualObject.transform.localPosition = new Vector3(0, terrainPos.y, 0);
        visualObject.transform.rotation = Quaternion.Euler(initRotation);
    }
    
    //Returns a position on your terrain
    static Vector3 GetTerrainPos(float x, float z)
    {
        //Create object to store raycast data
        RaycastHit hit;
 
        //Create origin for raycast that is above the terrain. I chose 100.
        Vector3 origin = new Vector3(x, 100, z);
 
        //Send the raycast.
        Physics.Raycast(origin, Vector3.down, out hit, Mathf.Infinity);
 
        Debug.Log("Terrain location found at " + hit.point);
        return hit.point;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
