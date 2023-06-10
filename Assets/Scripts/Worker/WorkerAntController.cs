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

        public Action<WorkerAntController> OnPathStarted;
        public Action<WorkerAntController> OnPathCompleted;

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

        public void Whistle()
        {
            if(Queen == null)
                return;
            
            ChangeState(WorkerAntStatus.Defense);
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
        
        public bool SetDestination(Vector3 destination, Action<WorkerAntController> onPathCompleted)
        {
            OnPathCompleted += onPathCompleted;
            
            return _antMovement.SetDestination(destination);
        }

        public void Die()
        {
            Debug.LogWarning($"{this.name} die now.");
        }
    }
}