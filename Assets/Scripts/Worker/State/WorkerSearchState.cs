using DG.Tweening;
using UnityEngine;

namespace Worker.State
{
    public class WorkerSearchState : WorkerStateBase
    {
        [Tooltip("Waiting time after reaching the search position.")]
        [SerializeField] private float _searchIdleTime = 2f;

        [SerializeField] private float _searchDistance = 10f;

        private Tween _waitingDelayTween;
        private Vector3 _searchPosition;
        
        public override void Activate()
        {
            base.Activate();

            _workerAntController.Status = WorkerAntStatus.SearchFood;
            
            var queen = _workerAntController.Queen;
            _searchPosition = queen.position + queen.forward.normalized * _searchDistance;
            _workerAntController.SetDestination(_searchPosition, null);
        }

        public override void Deactivate()
        {
            base.Deactivate();
            
            _waitingDelayTween?.Kill();
        }

        protected override void UpdateState()
        {
            base.UpdateState();
            
            if(Vector3.Distance(_searchPosition, transform.position) <= 1)
                OnPathCompleted();
        }

        private void OnPathCompleted()
        {
            _waitingDelayTween = DOVirtual.DelayedCall(_searchIdleTime, () =>
            {
                _workerAntController.ChangeState(WorkerAntStatus.Defense);
            });
        }
    }
}