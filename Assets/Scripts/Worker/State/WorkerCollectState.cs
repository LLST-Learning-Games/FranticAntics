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

        private GameObject _collectedPiece;
        private Transform _endDestination;
        
        private bool _itemCollected;
        private float _resourcesCollected;
        
        public override void Activate()
        {
            base.Activate();

            _workerAntController.Status = WorkerAntStatus.CollectFood;

            _workerAntController.OnAntDead += OnAntDead;
        }

        public override void Deactivate()
        {
            _workerAntController.OnAntDead -= OnAntDead;
            
            base.Deactivate();

            TargetCollectable = null;
            _endDestination = null;
            _collectedPiece = null;
            
            _itemCollected = false;
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
            if(!TargetCollectable)
                return;
            
            base.UpdateState();

            try
            {
                if(TargetCollectable.ItemCollected && !_itemCollected)
                    _workerAntController.Whistle(Vector3.zero);

                if (!_itemCollected && Vector3.Distance(TargetCollectable.transform.position, transform.position) < .5f)
                {
                    _itemCollected = true;
                    _resourcesCollected = TargetCollectable.GetResources();
                
                    if(!TargetCollectable.Mineable)
                        _collectedPiece = TargetCollectable.gameObject;
                    else
                        _collectedPiece = TargetCollectable.Mine();
                
                    _collectedPiece.transform.SetParent(_carryParent);
                    _collectedPiece.transform.DOLocalMove(Vector3.zero, .2f).OnComplete(() =>
                    {
                        if(!TargetCollectable.Mineable)
                            TargetCollectable.ItemCollected = true;
                    });
                }
                else if (_itemCollected && _collectedPiece != null)
                {
                    _workerAntController.SetDestination(_endDestination.position);

                    if (Vector3.Distance(transform.position, _endDestination.position) < .5f)
                    {
                        _collectedPiece.transform.SetParent(null);
                        _collectedPiece.transform.DOScale(Vector3.zero, .2f);
                        _collectedPiece.transform.DOMove(_endDestination.position, .2f);
                        TargetCollectable.Consume(_workerAntController.TeamController, _resourcesCollected);
                        Destroy(TargetCollectable.gameObject);

                        _workerAntController.Whistle(Vector3.zero);
                    }
                }
            }
            catch (Exception e)
            {
                _workerAntController.Whistle(Vector3.zero);
            }
        }
        
        private void OnAntDead()
        {
            if(TargetCollectable == null)
                return;

            if (TargetCollectable.Mineable)
            {
                Destroy(_collectedPiece);
                return;
            }
            
            TargetCollectable.transform.SetParent(null);
            TargetCollectable.ItemCollected = false;
        }
    }
}