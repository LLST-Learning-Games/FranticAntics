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
            
            _allStateControllers.Add(WorkerAntStatus.Defense, gameObject.AddComponent<WorkerDefenceState>());

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

        private void ChangeState(WorkerAntStatus newState)
        {
            if(_allStateControllers.ContainsKey(Status))
                _allStateControllers[Status]?.Deactivate();
            
            if(_allStateControllers.ContainsKey(WorkerAntStatus.Defense))
                _allStateControllers[WorkerAntStatus.Defense]?.Activate();   
        }
        
        public bool SetDestination(Vector3 destination, Action<WorkerAntController> onPathCompleted)
        {
            OnPathCompleted += onPathCompleted;
            
            return _antMovement.SetDestination(destination);
        }
    }
}