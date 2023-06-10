using System;
using UnityEngine;

namespace Worker.State
{
    public class WorkerStateBase : MonoBehaviour
    {
        protected WorkerAntController _workerAntController;
        protected bool _isActive;
        
        public virtual void Initialize(WorkerAntController workerAntController)
        {
            _workerAntController = workerAntController;
        }

        public virtual void Activate()
        {
            _isActive = true;
        }

        public virtual void Deactivate()
        {
            _isActive = false;
        }

        protected virtual void UpdateState()
        {
            
        }
        
        private void Update()
        {
            if(!_isActive)
                return;
            
            UpdateState();
        }
    }
}