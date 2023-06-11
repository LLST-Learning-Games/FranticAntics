using System;
using UnityEngine;
using Worker.State;

namespace Worker
{
    public class WorkerEnemySeeker : MonoBehaviour
    {
        [SerializeField] private WorkerAntController _workerAntController;

        private void OnTriggerEnter(Collider other)
        {
            if(_workerAntController.IsDead)
                return;
            
            if(_workerAntController.Status == WorkerAntStatus.CollectFood || _workerAntController.Status == WorkerAntStatus.Attack)
                return;
            
            if(other == null)
                return;

            var enemyController = other.GetComponent<WorkerAntController>();
            if(enemyController == null || enemyController.TeamController == _workerAntController.TeamController)
                return;
            
            _workerAntController.ChangeState(WorkerAntStatus.Attack);
            var attackState = _workerAntController.GetCurrentStateController() as WorkerAttackState;
            attackState.SetEnemy(enemyController);
        }

        private void OnTriggerStay(Collider other)
        {
            if(_workerAntController.IsDead)
                return;
            
            if(_workerAntController.Status == WorkerAntStatus.CollectFood || _workerAntController.Status == WorkerAntStatus.Attack)
                return;
            
            if(other == null)
                return;

            var enemyController = other.GetComponent<WorkerAntController>();
            if(enemyController == null || enemyController.TeamController == _workerAntController.TeamController)
                return;
            
            _workerAntController.ChangeState(WorkerAntStatus.Attack);
            var attackState = _workerAntController.GetCurrentStateController() as WorkerAttackState;
            attackState.SetEnemy(enemyController);
        }
    }
}