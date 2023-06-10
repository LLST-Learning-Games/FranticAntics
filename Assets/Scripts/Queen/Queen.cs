using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Queen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       // HandleInput();
    }

    private void HandleInput()
    {
        var horizontal_L = Input.GetAxis("Horizontal-L");
        var vertical_L = Input.GetAxis("Vertical-L");
        
        if (horizontal_L > 0.1f)
            Debug.Log(horizontal_L);
        if (vertical_L > 0.1f)
            Debug.Log(vertical_L);
    }
}
