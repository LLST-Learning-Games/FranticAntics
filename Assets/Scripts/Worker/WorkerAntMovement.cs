using UnityEngine;
using UnityEngine.AI;

namespace Worker
{
    public class WorkerAntMovement : MonoBehaviour
    {
        private WorkerAntController _workerAntController;
        
        [SerializeField] protected Transform _navMeshTarget;
        [SerializeField] protected NavMeshAgent _navMeshAgent;
        [SerializeField] protected Animator _animator;
        
        public bool IsStopped;
        public bool IsIdle;

        public void Initialize(WorkerAntController workerAntController)
        {
            _workerAntController = workerAntController;
        }
        
        public void ProcessAntMovement()
        {
            if(_navMeshTarget != null)
                SetDestination(_navMeshTarget.position);
            
            if(_navMeshAgent.path == null)
                return;

            // if the ant stopped play idle animation
            if (_navMeshAgent.velocity == Vector3.zero && !IsIdle)
            {
                AntPathStarted();
                return;
            }

            if (_navMeshAgent.velocity != Vector3.zero && IsStopped)
            {
                AntPathCompleted();
            }
        }

        private void StartRandomIdleAnimation()
        {
            _animator.SetTrigger("idle_" + Random.Range(0,3));
            IsIdle = true;
            IsStopped = true;
        }

        private void PlayMovementAnimation()
        {
            _animator.SetTrigger("walk");
            IsIdle = false;
            IsStopped = false;
        }
        
        public bool SetDestination(Vector3 destination)
        {
            var result = _navMeshAgent.SetDestination(destination);
            return result;
        }
        
        public virtual void AntPathStarted()
        {
            Debug.Log("[WorkerAntController] Ant stopped");
            StartRandomIdleAnimation();
        }

        public virtual void AntPathCompleted()
        {
            Debug.Log("[WorkerAntController] Ant started moving");
            PlayMovementAnimation();
        }
    }
}