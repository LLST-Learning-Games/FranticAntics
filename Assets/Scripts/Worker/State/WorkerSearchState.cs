using System;
using DG.Tweening;
using Entities;
using UnityEngine;
using UnityEngine.AI;

namespace Worker.State
{
    public class WorkerSearchState : WorkerStateBase
    {
        [Tooltip("Waiting time after reaching the search position.")]
        [SerializeField] private float _searchIdleTime = 2f;

        [SerializeField] private float _searchDistance = 10f;

        private Tween _waitingDelayTween;
        private Vector3 _searchPosition;

        private NavMeshAgent _navMeshAgent;
        
        public override void Activate()
        {
            base.Activate();

            _workerAntController.Status = WorkerAntStatus.SearchFood;
            _navMeshAgent = GetComponent<NavMeshAgent>();
            
            var queen = _workerAntController.TeamController.Queen;
            _searchPosition = queen.transform.position + queen.GetForward().normalized * _searchDistance;
            _workerAntController.SetDestination(_searchPosition);
        }

        public override void Deactivate()
        {
            base.Deactivate();

            _waitingDelayTween?.Kill();
            _waitingDelayTween = null;
        }

        protected override void UpdateState()
        {
            base.UpdateState();

            if(_waitingDelayTween == null && Vector3.Distance(_searchPosition, transform.position) <= 1)
                OnPathCompleted();
            
            if(_waitingDelayTween == null && !_navMeshAgent.hasPath)
            {
                _waitingDelayTween = DOVirtual.DelayedCall(3, OnPathCompleted);
            }
        }

        private void OnPathCompleted()
        {
            _waitingDelayTween = DOVirtual.DelayedCall(_searchIdleTime, () =>
            {
                _workerAntController.Whistle(Vector3.zero);
            });
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!_isActive)
                return;

            if(!other.transform.CompareTag("collectable"))
                return;

            CollectableItem collectableItem = other.GetComponent<CollectableItem>();

            if (collectableItem == null)
                return;

            if (!collectableItem.CanCollect)
                return;

            _workerAntController.ChangeState(WorkerAntStatus.CollectFood);
            (_workerAntController.GetCurrentStateController() as WorkerCollectState).SetTarget(collectableItem);
        }

        private void OnTriggerStay(Collider other)
        {
            if(!_isActive)
                return;

            if(!other.transform.CompareTag("collectable"))
                return;

            CollectableItem collectableItem = other.GetComponent<CollectableItem>();

            if (collectableItem == null)
                return;

            if (collectableItem.ResourcesRemaining <= 0 || collectableItem.ItemCollected)
                return;

            _workerAntController.ChangeState(WorkerAntStatus.CollectFood);
            (_workerAntController.GetCurrentStateController() as WorkerCollectState).SetTarget(collectableItem);
        }
    }
}
