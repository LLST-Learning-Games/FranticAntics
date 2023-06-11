using UnityEngine;

namespace Worker.State
{
    public class WorkerCollectState : WorkerStateBase
    {
        [SerializeField] private Transform _carryParent;
        
        public override void Initialize(WorkerAntController workerAntController)
        {
            base.Initialize(workerAntController);
        }

        public override void Activate()
        {
            base.Activate();
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }
    }
}