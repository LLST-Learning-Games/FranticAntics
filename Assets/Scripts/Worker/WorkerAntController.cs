using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Team;
using UnityEngine;
using Worker.State;

namespace Worker
{
    public class WorkerAntController : MonoBehaviour
    {
        public Action OnAntDead;

        public TeamController TeamController;

        public WorkerAntMovement Movement => _antMovement;
        public WorkerAntStatistics Statistics => _antStatistics;
        
        [SerializeField] private SkinnedMeshRenderer _meshRenderer;
        [SerializeField] protected WorkerAntMovement _antMovement;
        [SerializeField] private WorkerAntStatistics _antStatistics;

        public bool IsDead;
        public WorkerAntStatus Status;

        private Dictionary<WorkerAntStatus, WorkerStateBase> _allStateControllers = new();

        public void Initialize()
        {
            _antMovement.Initialize(this);
            _antStatistics.Initialize(this);
            
            _allStateControllers.Add(WorkerAntStatus.Defense, GetComponent<WorkerDefenceState>());
            _allStateControllers.Add(WorkerAntStatus.SearchFood, GetComponent<WorkerSearchState>());
            _allStateControllers.Add(WorkerAntStatus.CollectFood, GetComponent<WorkerCollectState>());
            _allStateControllers.Add(WorkerAntStatus.Attack, GetComponent<WorkerAttackState>());

            foreach (var stateController in _allStateControllers)
            {
                stateController.Value.Initialize(this);
            }
            
            TeamController.WorkerAntManager.RegisterWorkerAnt(this);
            _meshRenderer.materials[0].SetColor("_BaseColor", TeamController.WorkerAntColor);

            Whistle(Vector3.zero);
        }

        private void OnDestroy()
        {
            if(TeamController.WorkerAntManager != null)
                TeamController.WorkerAntManager.RemoveWorkerAnt(this);
        }

        private void Update()
        {
            _antMovement.ProcessAntMovement();
        }

        public void Whistle(Vector3 defenceOffset)
        {
            if(TeamController.Queen == null)
                return;
            
            TeamController.WorkerAntManager.DefenseManager.AddAntToDefense(this);
            
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
            IsDead = true;
            
            OnAntDead?.Invoke();
            OnAntDead = null;
            
            _antMovement.Disable();
            foreach (var stateController in _allStateControllers)
            {
                stateController.Value.enabled = false;
                stateController.Value.Deactivate();
            }
            
            TeamController.WorkerAntManager.RemoveWorkerAnt(this);
            
            _antMovement.Animator.SetTrigger("dead");
            transform.DOScale(0, .2f).SetDelay(.2f).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        }

        public void TakeDamage(int damage, bool instantDeath)
        {
            _antStatistics.TakeDamage(damage, instantDeath);
        }
    }
}