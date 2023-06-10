using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Worker.State
{
    public class WorkerDefenceState : WorkerStateBase
    {
        private Vector3 _defencePositionOffset;
        [SerializeField] private float _defenseRange = 1f;
        
        public override void Activate()
        {
            base.Activate();
            
            _workerAntController.Status = WorkerAntStatus.Defense;

            _defencePositionOffset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        }

        protected override void UpdateState()
        {
            base.UpdateState();

            _workerAntController.SetDestination(GetDefensePosition(), null);
        }

        private Vector3 GetDefensePosition()
        {
            return _workerAntController.Queen.position + _defencePositionOffset * _defenseRange;
        }
        
        
    }
}