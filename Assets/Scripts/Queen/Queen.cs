using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace  AntQueen
{
    public class Queen : MonoBehaviour
    {

        [SerializeField] private InputReader _queenInput;

        [SerializeField] private float _speed;
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        { 
            Vector3 movement = new Vector3(_queenInput.movement.x, 0, _queenInput.movement.y);
            transform.Translate(movement * Time.deltaTime * _speed);
        }
    }   
}
