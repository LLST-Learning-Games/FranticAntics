using UnityEngine;

namespace Worker
{

    public class WorkerAntStatistics : MonoBehaviour
    {
        private WorkerAntController _workerAntController;
        
        [SerializeField][Range(1, 10)] private int _defaultHp = 3;
        
        [SerializeField][Range(0, 10)] private float _defaultMovementSpeed = 2;
        [SerializeField][Range(1, 10)] private int _defaultAttackPower = 1;
        
        private int _hp = 3;
        private float _movementSpeed = 2;
        private int _attackPower = 1;

        public int Hp => _hp;
        public float MovementSpeed => _movementSpeed;
        public int AttackPower => _attackPower;
        
        private void Start()
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
    }
}