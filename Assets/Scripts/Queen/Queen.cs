using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace  AntQueen
{
    public class Queen : MonoBehaviour
    {

        [SerializeField] private InputReader _queenInput;
        [SerializeField] private Transform _targetObject;
        [SerializeField] private float _targetDistance;

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
            
            Vector3 target = new Vector3(_queenInput.target.x, 0, _queenInput.target.y);
            _targetObject.transform.SetLocalPositionAndRotation(target * 3f, Quaternion.identity);
        }
    }   
}
