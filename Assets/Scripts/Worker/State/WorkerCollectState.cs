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

        private bool _waitingForEnoughToCollect;
        private bool _itemCollected;
        [SerializeField] private float _resourcesCollected;
        
        public override void Activate()
        {
            base.Activate();

            _workerAntController.Status = WorkerAntStatus.CollectFood;

            _workerAntController.OnAntDead += OnAntDead;
        }

        public override void Deactivate()
        {
            _workerAntController.OnAntDead -= OnAntDead;
            
            HandleDisruptedCarry();
            
            base.Deactivate();

            TargetCollectable = null;
            _endDestination = null;
            _collectedPiece = null;
            _waitingForEnoughToCollect = false;
            _itemCollected = false;
            _resourcesCollected = 0;
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
                if (TargetCollectable.ItemCollected && !_itemCollected)
                {
                    _workerAntController.Whistle(Vector3.zero);
                }

                if (!_itemCollected 
                    && Vector3.Distance(TargetCollectable.transform.position, transform.position) < TargetCollectable.AssignmentWaitDistance)
                {
                    if(!_waitingForEnoughToCollect)
                        TargetCollectable.AssignAnt(_workerAntController);

                    if (!TargetCollectable.HasEnoughAntsToCarry())
                    {
                        // Set strain animation and sweat effects
                        _workerAntController.SetDestination(transform.position);
                        _waitingForEnoughToCollect = true;
                        return;
                    }
                        
                    _itemCollected = true;
                    
                    if(!TargetCollectable.Mineable)
                    {
                        _collectedPiece = TargetCollectable.gameObject;
                    }
                    else
                    {
                        _collectedPiece = TargetCollectable.Mine();
                    }
                    
                    if(TargetCollectable.Mineable || TargetCollectable.CheckIfPrimaryCarrier(_workerAntController))
                    {
                        _resourcesCollected = TargetCollectable.GetResources();
                        _collectedPiece.transform.SetParent(_carryParent);
                        _collectedPiece.transform.DOLocalMove(Vector3.zero, .2f).OnComplete(() =>
                        {
                            if (!TargetCollectable.Mineable)
                                TargetCollectable.ItemCollected = true;
                        });
                    }
                }
                else if (_itemCollected && _collectedPiece != null)
                {
                    _workerAntController.SetDestination(_endDestination.position);

                    if (Vector3.Distance(transform.position, _endDestination.position) < .5f)
                    {
                        _collectedPiece.transform.SetParent(null);
                        _collectedPiece.transform.DOScale(Vector3.zero, .2f);
                        _collectedPiece.transform.DOMove(_endDestination.position, .2f);

                        float resources = 0;
                        foreach (var ant in TargetCollectable.AntsAssigned)
                        {
                            if (ant.GetCurrentStateController() is WorkerCollectState state)
                            {
                                resources += state._resourcesCollected;
                                state._resourcesCollected = 0;
                            }
                        }
                        TargetCollectable.Consume(_workerAntController.TeamController, resources);

                        _workerAntController.Whistle(Vector3.zero);
                    }
                }
            }
            catch (Exception e)
            {
                
                if(TargetCollectable)
                    TargetCollectable.UnassignAnt(_workerAntController);
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
            
            HandleDisruptedCarry();
        }
        
        private void HandleDisruptedCarry()
        {
            if (!TargetCollectable) return;

            TargetCollectable.UnassignAnt(_workerAntController);

            if (!TargetCollectable.HasEnoughAntsToCarry())
            {
                TargetCollectable.HandleLostAssignedAnts();
                TargetCollectable.transform.SetParent(null);
                return;
            }

            // If there are still enough ants to carry the cheeto, make another ant lead ant
            if (TargetCollectable.transform.parent == _carryParent
                && TargetCollectable.AntsAssigned[0].GetCurrentStateController() is WorkerCollectState state)
            {
                state.AssignThisAntAsLeadCarrier(TargetCollectable);
            }

            //TargetCollectable.transform.SetParent(null);
        }
        
        public void AssignThisAntAsLeadCarrier(CollectableItem target)
        {
            target.transform.SetParent(_carryParent);
            _collectedPiece = target.gameObject;
            _resourcesCollected = target.GetResources();
        }
    }
}