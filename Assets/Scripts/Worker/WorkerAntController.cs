using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Worker.State;

namespace Worker
{
    public class WorkerAntController : MonoBehaviour
    {
        [SerializeField] protected WorkerAntMovement _antMovement;
        [SerializeField] private WorkerAntStatistics _antStatistics;
        public Transform Queen;

        public WorkerAntStatus Status;
        
        private bool _isStopped => _antMovement.IsStopped;
        private bool _isIdle => _antMovement.IsIdle;

        private Dictionary<WorkerAntStatus, WorkerStateBase> _allStateControllers = new();

        private void Awake()
        {
            _antMovement.Initialize(this);
            _antStatistics.Initialize(this);
            
            _allStateControllers.Add(WorkerAntStatus.Defense, GetComponent<WorkerDefenceState>());
            _allStateControllers.Add(WorkerAntStatus.SearchFood, GetComponent<WorkerSearchState>());

            foreach (var stateController in _allStateControllers)
            {
                stateController.Value.Initialize(this);
            }
            
            WorkerAntManager.Instance.RegisterWorkerAnt(this);
        }

        private void OnDestroy()
        {
            if(WorkerAntManager.Instance != null)
                WorkerAntManager.Instance.RemoveWorkerAnt(this);
        }

        private void Update()
        {
            _antMovement.ProcessAntMovement();
        }

        public void Whistle(Vector3 defenceOffset)
        {
            if(Queen == null)
                return;
            
            WorkerAntManager.Instance.DefenseManager.AddAntToDefense(this);
            
            ChangeState(WorkerAntStatus.Defense);
            var defenceState = GetCurrentStateController() as WorkerDefenceState;
            if (defenceState)
                defenceState.DefencePositionOffset = defenceOffset;
        }

        public void SendSearch()
        {
            ChangeState(WorkerAntStatus.SearchFood);
        }
        
        public void ChangeState(WorkerAntStatus newState)
        {
            if(_allStateControllers.ContainsKey(Status))
                _allStateControllers[Status]?.Deactivate();
            
            if(_allStateControllers.ContainsKey(newState))
                _allStateControllers[newState]?.Activate();   
        }

        public WorkerStateBase GetCurrentStateController()
        {
            if (!_allStateControllers.ContainsKey(Status))
                return null;
            
            return _allStateControllers[Status];
        }
        
        public bool SetDestination(Vector3 destination)
        {
            return _antMovement.SetDestination(destination);
        }

        public void Die()
        {
            Debug.LogWarning($"{this.name} die now.");
        }
    }
}