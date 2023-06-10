using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnPoint : MonoBehaviour
{
    private MapObjectType type;
    
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
