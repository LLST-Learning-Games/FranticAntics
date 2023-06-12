using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;

public class SpawnableEntitiy : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 _startingPos;
    public Vector3 _endingPos;

    public float _groundLevelForModel = 0f;

    public float dropTime = 2f;
    private float _timelapse = 0f;

    private CollectableItem _collectableItem;
    
    private void Start()
    {
        _collectableItem = gameObject.GetComponent<CollectableItem>();
    }

    public void Initialise(Vector3 startingPos, Vector3 endingPos)
    {
        _startingPos = startingPos;
        _endingPos = new Vector3(endingPos.x, _groundLevelForModel, endingPos.z);

        _timelapse = 0;
    }
    
    void Update()
    {
        // todo move
        
        // close enough, turn off

        float progress = _timelapse / dropTime;
        
        UnityEngine.Vector3 pos = Vector3.Lerp(_startingPos, _endingPos, progress);
        transform.position = pos;
         // transform.po = pos;

         _timelapse += Time.deltaTime;
         
        if(Vector3.Distance(transform.position,_endingPos) < 0.03)
        {
            //we are close to our destination so do something
            this.enabled = false;
        }
    }

    public bool IsConsumed()
    {
        return _collectableItem == null || _collectableItem.ItemConsumed;
    }
}
