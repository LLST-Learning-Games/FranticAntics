using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Worker.State
{
    public class WorkerDefenceState : WorkerStateBase
    {
        public Vector3 DefencePositionOffset;
        [SerializeField] private float _defenseRange = 1f;
        
        public override void Activate()
        {
            base.Activate();
            
            _workerAntController.Status = WorkerAntStatus.Defense;
        }

        protected override void UpdateState()
        {
            base.UpdateState();

            _workerAntController.SetDestination(GetDefensePosition());
        }

        private Vector3 GetDefensePosition()
        {
            return _workerAntController.Queen.position + DefencePositionOffset * _defenseRange;
        }
        
        
    }
}