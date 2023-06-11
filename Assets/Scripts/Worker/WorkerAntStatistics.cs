using UnityEngine;
using UnityEngine.AI;

namespace Worker
{

    public class WorkerAntStatistics : MonoBehaviour
    {
        private WorkerAntController _workerAntController;
        [SerializeField] protected NavMeshAgent _navMeshAgent;

        [SerializeField] private int _defaultHp = 3;
        [SerializeField] private float _defaultMovementSpeed = 2;
        [SerializeField] private int _defaultAttackPower = 1;
        
        private int _hp = 3;
        private float _movementSpeed = 2;
        private int _attackPower = 1;

        public int Hp => _hp;
        public float MovementSpeed => _movementSpeed;
        public int AttackPower => _attackPower;
        
        private void Awake()
        { 
            ResetToDefaultValues();
        }

        private void ResetToDefaultValues()
        {
            _hp = _defaultHp;
            _movementSpeed = _defaultMovementSpeed;
            _attackPower = _defaultAttackPower;
        }

        public void Initialize(WorkerAntController workerAntController)
        {
            _workerAntController = workerAntController;
        }
        
        public void TakeDamage(int damage = 1, bool instantDeath = false)
        {
            _hp -= damage;

            if(_hp < 0 || instantDeath)
            {
                _workerAntController.Die();
            }
        }

        public void SetMovementSpeed(float movementSpeed)
        {
            _movementSpeed = movementSpeed;
            _navMeshAgent.speed = _movementSpeed;
        }
    }
}