using System;
using DG.Tweening;
using Entities;
using UnityEngine;

namespace Worker.State
{
    public class WorkerCollectState : WorkerStateBase
    {
        [SerializeField] private Transform _carryParent;

        public CollectableItem TargetCollectable;

        private Transform _endDestination;
        
        private bool _itemCollected;
        
        public override void Activate()
        {
            base.Activate();

            _workerAntController.Status = WorkerAntStatus.CollectFood;
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        public void SetTarget(CollectableItem item)
        {
            TargetCollectable = item;
            
            _workerAntController.SetDestination(TargetCollectable.transform.position);

            _endDestination = TargetCollectable.ForQueen
                ? _workerAntController.TeamController.Queen.transform
                : _workerAntController.TeamController.Colony.transform;
        }
        
        protected override void UpdateState()
        {
            if(TargetCollectable == null)
                return;
            
            base.UpdateState();
            
            if(TargetCollectable.ItemCollected && !_itemCollected)
                _workerAntController.Whistle(Vector3.zero);

            if (!_itemCollected && Vector3.Distance(TargetCollectable.transform.position, transform.position) < .5f)
            {
                _itemCollected = true;
                
                TargetCollectable.transform.SetParent(_carryParent);
                TargetCollectable.transform.DOLocalMove(Vector3.zero, .2f).OnComplete(() =>
                {
                    TargetCollectable.ItemCollected = true;
                });
            }
            else if (_itemCollected && TargetCollectable.ItemCollected)
            {
                _workerAntController.SetDestination(_endDestination.position);

                if (Vector3.Distance(transform.position, _endDestination.position) < .5f)
                {
                    TargetCollectable.transform.SetParent(null);
                    TargetCollectable.transform.DOScale(Vector3.zero, .2f);
                    TargetCollectable.transform.DOMove(_endDestination.position, .2f);
                    
                    _workerAntController.Whistle(Vector3.zero);
                }
            }
        }
    }
}