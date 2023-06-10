using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLocation : MonoBehaviour
{
    [SerializeField] private GameObject debugIndicator;

    private void Start()
    {
        debugIndicator.SetActive(false);
    }
}