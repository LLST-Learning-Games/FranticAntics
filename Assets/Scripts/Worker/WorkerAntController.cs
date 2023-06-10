using System;
using UnityEngine;

namespace Worker
{
    public class WorkerAntController : MonoBehaviour
    {
        [SerializeField] protected WorkerAntMovement _antMovement;

        public WorkerAntStatus Status;
        
        private bool _isStopped => _antMovement.IsStopped;
        private bool _isIdle => _antMovement.IsIdle;

        public Action<WorkerAntController> OnPathStarted;
        public Action<WorkerAntController> OnPathCompleted;

        private void Start()
        {
            _antMovement.Initialize(this);
            
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

        public bool SetDestination(Vector3 destination, Action<WorkerAntController> onPathCompleted)
        {
            OnPathCompleted += onPathCompleted;
            
            return _antMovement.SetDestination(destination);
        }
    }
}