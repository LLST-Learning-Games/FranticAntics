using PowerUps;
using UnityEngine;

namespace Worker.State
{
    public class WorkerAttackState : WorkerStateBase
    {
        private WorkerAntMovement _movement;

        private WorkerAntController _enemy;
        
        private float _attackTimer;
        private int _attackPower;
        
        public override void Initialize(WorkerAntController workerAntController)
        {
            base.Initialize(workerAntController);

            _movement = _workerAntController.Movement;
            _attackPower = _workerAntController.Statistics.AttackPower;

            _workerAntController.TeamController.OnPowerUpStarted += OnPowerUpStarted;
            _workerAntController.TeamController.OnPowerUpStarted += OnPowerUpFinished;
        }

        public override void Activate()
        {
            base.Activate();
            
            _workerAntController.Status = WorkerAntStatus.Attack;
        }

        public override void Deactivate()
        {
            base.Deactivate();

            _enemy = null;
            _attackTimer = 0f;
        }

        public void SetEnemy(WorkerAntController enemy)
        {
            _enemy = enemy;
        }
        
        protected override void UpdateState()
        {
            if(_enemy == null)
                return;
            
            base.UpdateState();

            if (_enemy.IsDead)
            {
                _workerAntController.Whistle(Vector3.zero);
                return;
            }
            
            var distanceToEnemy = Vector3.Distance(transform.position, _enemy.transform.position);

            if (distanceToEnemy > 0.7f)
            {
                _workerAntController.SetDestination(_enemy.transform.position);
                return;
            }

            _attackTimer -= Time.deltaTime;
            if(_attackTimer > 0)
                return;

            _attackTimer = 2f;
            
            _movement.Animator.SetTrigger("attack_" + Random.Range(0,2));
            _enemy.TakeDamage(_attackPower, false);
        }
        
        private void OnPowerUpStarted(PowerUpData powerUpData)
        {
            if (powerUpData.PowerUpType == PowerUpType.SPEED)
                _attackPower *= powerUpData.PowerUpMultiplier;
        }
        
        private void OnPowerUpFinished(PowerUpData powerUpData)
        {
            if (powerUpData.PowerUpType == PowerUpType.SPEED)
                _attackPower /= powerUpData.PowerUpMultiplier;
        }
    }
}