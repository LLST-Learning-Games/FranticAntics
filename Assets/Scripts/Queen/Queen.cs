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

        [SerializeField] private GameObject _antPrefab;

        [SerializeField] private Animator _animator;
        
        void Start()
        {
            _queenInput.spawnAnt += OnSpawnAnt;
        }

        void Update()
        {
            HandleMovement();
        }

        private void HandleMovement()
        { 
            Vector3 movement = new Vector3(_queenInput.movement.x, 0, _queenInput.movement.y);
            transform.Translate(movement * Time.deltaTime * _speed);
            //transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (movement), Time.deltaTime * 40f);
            _animator.speed = movement.magnitude * _speed;
            
            Vector3 target = new Vector3(_queenInput.target.x, 0, _queenInput.target.y);
            _targetObject.transform.SetLocalPositionAndRotation(target * _targetDistance, Quaternion.identity);
        }

        public void OnSpawnAnt()
        {
            Instantiate(_antPrefab,_targetObject.transform.position,Quaternion.identity);
        }
    }   
}
