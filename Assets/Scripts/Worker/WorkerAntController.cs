using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Worker
{
    public class WorkerAntController : MonoBehaviour
    {
        [SerializeField] protected Animator _animator;

        [Header("AI")]
        [SerializeField] protected Transform _navMeshTarget;
        [SerializeField] protected NavMeshAgent _navMeshAgent;

        private bool _isStopped;
        private bool _isIdle;

        public Action<WorkerAntController> OnPathStarted;
        public Action<WorkerAntController> OnPathCompleted;

        private void Start()
        {
        }

        private void Update()
        {
            SetDestination(null);

            ProcessAntMovement();
        }

        private void ProcessAntMovement()
        {
            if(_navMeshAgent.path == null)
                return;

            // if the ant stopped play idle animation
            if (_navMeshAgent.velocity == Vector3.zero && !_isIdle)
            {
                AntPathStarted();
                return;
            }

            if (_navMeshAgent.velocity != Vector3.zero && _isStopped)
            {
                AntPathCompleted();
            }
        }

        private void StartRandomIdleAnimation()
        {
            _animator.SetTrigger("idle_" + Random.Range(0,3));
            _isIdle = true;
            _isStopped = true;
        }

        private void PlayMovementAnimation()
        {
            _animator.SetTrigger("walk");
            _isIdle = false;
            _isStopped = false;
        }

        public bool SetDestination(Action<WorkerAntController> onPathCompleted)
        {
            var result = _navMeshAgent.SetDestination(_navMeshTarget.position);
            OnPathCompleted += onPathCompleted;
            
            return result;
        }

        public virtual void AntPathStarted()
        {
            Debug.Log("[WorkerAntController] Ant stopped");
            StartRandomIdleAnimation();
            
            OnPathStarted?.Invoke(this);
        }

        public virtual void AntPathCompleted()
        {
            Debug.Log("[WorkerAntController] Ant started moving");
            PlayMovementAnimation();
            
            OnPathCompleted?.Invoke(this);
        }
    }
}