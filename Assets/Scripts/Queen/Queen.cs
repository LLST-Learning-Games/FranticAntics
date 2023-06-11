using Team;
using UnityEngine;
using Worker;

namespace  AntQueen
{
    public class Queen : MonoBehaviour
    {
        public TeamController TeamController;
        
        [SerializeField] private int _playerNumber;
        [SerializeField] private Transform _targetObject;
        [SerializeField] private Transform _playerModel;
        [SerializeField] private float _targetDistance = 1f;
        [SerializeField] private float _speed = 2f;
        [SerializeField] private float _spawnTriggerThreshold = 0.5f;

        [SerializeField] private WorkerAntController _antPrefab;

        [SerializeField] private Animator _animator;
        [SerializeField] private float _antSpawnCooldownTime = 0.5f;

        private float _antSpawnCooldown;
        
        void Start()
        {
            UnityEngine.Rendering.DebugManager.instance.enableRuntimeUI = false;
            _antSpawnCooldown = _antSpawnCooldownTime;

            for(int i = 0; i < TeamController.InitialAnts; ++i)
            {
                SpawnAnt();
            }
        }

        void Update() 
        {
            HandleMovement();
            HandleTarget();
            HandleSpawnAnt();
        }

        private void HandleMovement()
        {
            var leftInput = GetController("Left", _playerNumber);
            Vector3 movement = new Vector3(leftInput.x, 0, leftInput.y);
            transform.Translate(movement * Time.deltaTime * _speed);
        
            if(movement != Vector3.zero)
            {
                _playerModel.rotation = Quaternion.Slerp(
                    _playerModel.rotation, 
                    Quaternion.LookRotation(movement),
                    Time.deltaTime * 40f);
            }
            _animator.speed = movement.magnitude * _speed;
        }

        private void HandleTarget()
        {
            var rightInput = GetController("Right", _playerNumber);
            Vector3 target = new Vector3(rightInput.x, 0, rightInput.y);
            _targetObject.transform.SetLocalPositionAndRotation(target * _targetDistance, Quaternion.identity);
        }


        public void HandleSpawnAnt()
        {
            _antSpawnCooldown -= Time.deltaTime;

            if (!CanSpawnAnt())
            {
                return;
            }

            if (GetTrigger(_playerNumber) < _spawnTriggerThreshold)
            {
                return;
            }

            SpawnAnt();
            _antSpawnCooldown = _antSpawnCooldownTime;
            TeamController.Nectar -= TeamController.AntNectarCost;
        }

        private bool CanSpawnAnt()
        {
            if (_antSpawnCooldown > 0)
                return false;

            return TeamController.Nectar >= TeamController.AntNectarCost;
        }

        private void SpawnAnt()
        {
            var newAnt = Instantiate(_antPrefab, _targetObject.transform.position, Quaternion.identity);
            newAnt.TeamController = TeamController;
            newAnt.Initialize();
        }

        private Vector2 GetController(string stick, int controller)
        {
            return new Vector2(
                Input.GetAxis($"Horizontal-{stick}-{controller}"),
                Input.GetAxis($"Vertical-{stick}-{controller}")
            );
        }

        private float GetTrigger(int controller)
        {
            return Input.GetAxis($"Trigger-Right-{controller}");
        }

        public Vector3 GetForward()
        {
            return _playerModel.forward;
        }
    }   
}
