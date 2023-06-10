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
        [SerializeField] private int _playerNumber;
        [SerializeField] private Transform _targetObject;
        [SerializeField] private float _targetDistance;
        [SerializeField] private float _speed;

        [SerializeField] private GameObject _antPrefab;

        [SerializeField] private Animator _animator;
        
        void Start()
        {
            //_queenInput.spawnAnt += OnSpawnAnt;
        }

        void Update() 
        {
        //     Debug.Log("1: " + GetController("Left", 0));
        //     Debug.Log("2: " + GetController("Left", 1));
            HandleMovement();
        }

        private void HandleMovement()
        {
            var leftInput = GetController("Left", _playerNumber);
            Vector3 movement = new Vector3(leftInput.x, 0, leftInput.y);
            transform.Translate(movement * Time.deltaTime * _speed);
            //transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (movement), Time.deltaTime * 40f);
            _animator.speed = movement.magnitude * _speed;
        }

        private void HandleTarget()
        {
            var rightInput = GetController("Right", _playerNumber);
            Vector3 target = new Vector3(rightInput.x, 0, rightInput.y);
            _targetObject.transform.SetLocalPositionAndRotation(target * _targetDistance, Quaternion.identity);
        }

        private void HandleMovementNewInput()
        { 
            //Vector3 movement = new Vector3(_queenInput.movement.x, 0, _queenInput.movement.y);
            //transform.Translate(movement * Time.deltaTime * _speed);
            ////transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (movement), Time.deltaTime * 40f);
            //_animator.speed = movement.magnitude * _speed;
            
            //Vector3 target = new Vector3(_queenInput.target.x, 0, _queenInput.target.y);
            //_targetObject.transform.SetLocalPositionAndRotation(target * _targetDistance, Quaternion.identity);
        }

        public void OnSpawnAnt()
        {
            Instantiate(_antPrefab,_targetObject.transform.position,Quaternion.identity);
        }

        private Vector2 GetController(string stick, int controller)
        {
            return new Vector2(
                Input.GetAxis($"Horizontal-{stick}-{controller}"),
                Input.GetAxis($"Vertical-{stick}-{controller}")
            );
        }
    }   
}
