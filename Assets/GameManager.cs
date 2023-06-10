using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Range(0, 500)] private float _gamePlayDuration = 300f;

    private float _timer = 300f;


    // private int _teamOneScore = 0;
    // private int _teamTwoScore = 0;
    
    private void Start()
    {
        instance = this;  // may have a better approach
        Reset();
    }
    
    private void Reset()
    {
        _timer = _gamePlayDuration;
    }
}
