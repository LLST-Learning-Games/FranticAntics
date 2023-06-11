using Team;
using UnityEngine;
using UnityEngine.Serialization;
using Worker;

namespace  AntQueen
{
    public class Queen : MonoBehaviour
    {
        public TeamController TeamController;
        
        [Header("Team and Player Setup")]
        [SerializeField] private int _playerNumber;
        [SerializeField] private Transform _targetObject;
        [SerializeField] private Transform _playerModel;
        [SerializeField] private float _targetDistance = 1f;
        [SerializeField] private float _spawnTriggerThreshold = 0.5f;
        [SerializeField] private WorkerAntController _antPrefab;
        [SerializeField] private Animator _animator;
        
        
        [Header("Tunables")]
        [SerializeField] private float _speed = 2f;
        [SerializeField] private float _antSpawnStartDelayTime = 0.2f;
        [SerializeField] private float _antSpawnCooldownTime = 0.5f;
        [SerializeField] private float _sendAntMovementPauseCooldownTime = 0.5f;
        [SerializeField] private float _sendAntCooldownTime = 0.2f;


        private bool _antSpawnButtonDown = false;
        private float _antSpawnStartDelay;
        private float _antSpawnCooldown;
        private float _sendAntMovementPauseCooldown;
        private float _sendAntCooldown;
        private InputPlatformMode _inputPlatformMode;
        
        void Start()
        {
            UnityEngine.Rendering.DebugManager.instance.enableRuntimeUI = false;
            
            ResetCooldowns();

            for(int i = 0; i < TeamController.InitialAnts; ++i)
            {
                SpawnAnt();
            }
            
            if(Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                Debug.Log("Input mode is windows");
                _inputPlatformMode = InputPlatformMode.Windows;
            }
            else
            {
                Debug.Log("Input mode is mac");
                _inputPlatformMode = InputPlatformMode.Mac;
            }
            
        }

        private void ResetCooldowns()
        {
            _antSpawnCooldown = _antSpawnCooldownTime;
            _sendAntCooldown = _sendAntCooldownTime;
            _sendAntMovementPauseCooldown = -1;
            _antSpawnStartDelay = -1;
        }

        void Update()
        {
            UpdateCooldowns();
            HandleMovement();
            HandleTarget();
            HandleCommand();
            HandleSpawnAnt();
        }

        private void UpdateCooldowns()
        {
            var time = Time.deltaTime;
            _antSpawnCooldown -= time;
            _antSpawnStartDelay -= time;
            _sendAntCooldown -= time;
            _sendAntMovementPauseCooldown -= time;
        }

        private void HandleMovement()
        {
            var leftInput = GetStickInput("Left", _playerNumber);
            Vector3 movement = new Vector3(leftInput.x, 0, leftInput.y);
            
            if(CanMove())
            {
                _animator.SetBool("IsCommanding",false);
                transform.Translate(movement * Time.deltaTime * _speed);
                _animator.speed = movement.magnitude * _speed;
            }
            else
            {
                _animator.speed = 0;
            }
            
            if(movement != Vector3.zero)
            {
                _playerModel.rotation = Quaternion.Slerp(
                    _playerModel.rotation, 
                    Quaternion.LookRotation(movement),
                    Time.deltaTime * 40f);
            }
        }

        private bool CanMove()
        {
            if (_sendAntMovementPauseCooldown > 0 || _antSpawnButtonDown)
            {
                return false;
            }

            return true;
        }

        private void HandleTarget()
        {
            var rightInput = GetStickInput("Right", _playerNumber);
            
            Vector3 target = new Vector3(rightInput.x, 0, rightInput.y);
            _targetObject.transform.SetLocalPositionAndRotation(target * _targetDistance, Quaternion.identity);
        }

        private void HandleCommand()
        {
            var aInputString = GetButtonInputName("A", _playerNumber);
            var bInputString = GetButtonInputName("B", _playerNumber);

            if(Input.GetButtonDown(aInputString) || Input.GetKeyDown(KeyCode.Space))
            {
                //Debug.LogWarning("A button calling");
                SendWorkerAntForSearch();
            }

            else if(Input.GetButtonDown(bInputString))
            {
                //Debug.LogWarning("B button calling");
                TeamController.WorkerAntManager.Whistle();
            }
        }

        private void SendWorkerAntForSearch()
        {
            if (_sendAntCooldown < 0)
            {
                _animator.SetBool("IsCommanding",true);
                
                TeamController.WorkerAntManager.SendForSearch();
                _sendAntCooldown = _sendAntCooldownTime;
                _sendAntMovementPauseCooldown = _sendAntMovementPauseCooldownTime;
            }
        }

        public void HandleSpawnAnt()
        {
            if (GetTrigger(_playerNumber) < _spawnTriggerThreshold)
            {
                _antSpawnStartDelay = 0;
                _antSpawnButtonDown = false;
                return;
            }

            if (!_antSpawnButtonDown)
            {
                // todo: cue straining animation
                Debug.Log("Start straining now");
                _antSpawnStartDelay = _antSpawnStartDelayTime;
            }
            
            _antSpawnButtonDown = true;

            if (!CanSpawnAnt())
            {
                return;
            }
            
            SpawnAnt();
            _antSpawnCooldown = _antSpawnCooldownTime;
            TeamController.Nectar -= TeamController.AntNectarCost;
        }

        private bool CanSpawnAnt()
        {
            if (_antSpawnStartDelay > 0)
                return false;
            
            if (_antSpawnCooldown > 0)
                return false;

            return TeamController.Nectar >= TeamController.AntNectarCost;
        }

        private void SpawnAnt()
        {
            // todo: cue ant laying animation
            Debug.Log($"Laying ant now!");
            var newAnt = Instantiate(_antPrefab, _targetObject.transform.position, Quaternion.identity);
            newAnt.TeamController = TeamController;
            newAnt.Initialize();
            TeamController.workers.Add(newAnt);
        }

        private Vector2 GetStickInput(string stick, int controller)
        {
            return new Vector2(
                Input.GetAxis($"Horizontal-{stick}-{controller}-{_inputPlatformMode}"),
                Input.GetAxis($"Vertical-{stick}-{controller}-{_inputPlatformMode}")
            );
        }


        private string GetButtonInputName(string button, int controller)
        {
            return $"{button}-{controller}-{_inputPlatformMode}";
        }
        

        private float GetTrigger(int controller)
        {
            return Input.GetAxis($"Trigger-Right-{controller}");
        }

        public Vector3 GetForward()
        {
            return _playerModel.forward;
        }


        public enum InputPlatformMode
        {
            Windows,
            Mac,
        }
    }   
}
