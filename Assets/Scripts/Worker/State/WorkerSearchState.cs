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
        
        public override void Activate()
        {
            base.Activate();

            var queen = _workerAntController.Queen;
            var searchPosition = queen.position + queen.forward.normalized * _searchDistance;
            _workerAntController.SetDestination(searchPosition, OnPathCompleted);
        }

        public override void Deactivate()
        {
            base.Deactivate();
            
            _waitingDelayTween?.Kill();
        }

        private void OnPathCompleted(WorkerAntController _)
        {
            _workerAntController.OnPathCompleted -= OnPathCompleted;

            _waitingDelayTween = DOVirtual.DelayedCall(_searchIdleTime, () =>
            {
                _workerAntController.ChangeState(WorkerAntStatus.Defense);
            });
        }
    }
}