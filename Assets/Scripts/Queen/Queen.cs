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
        [SerializeField] private Egg _eggPrefab;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _whistleRange;

        [Header("Controls")]
        [SerializeField] private ControllerButtonDirection _sendKey = ControllerButtonDirection.South;
        [SerializeField] private ControllerButtonDirection _recallKey = ControllerButtonDirection.East;
        [SerializeField] private ControllerButtonDirection _spawnKey = ControllerButtonDirection.West;

        [Header("Tunables")]
        [SerializeField] private float _speed = 2f;
        [SerializeField] private float _antSpawnStartDelayTime = 0.2f;
        [SerializeField] private float _antSpawnCooldownTime = 0.5f;
        [SerializeField] private float _sendAntMovementPauseCooldownTime = 0.5f;
        [SerializeField] private float _sendAntCooldownTime = 0.2f;

        [Header("Particles/Animations")]
        [SerializeField] private ParticleSystem _whistleParticleSystem;
        [SerializeField] private float _whistleScaler = 0.5f;
        [SerializeField] private ParticleSystem _calloutParticleSystem;
        [SerializeField] private GameObject _spawnDelayParticleLoopingObject;

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
                SpawnAntEgg();
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
            _antSpawnStartDelay = _antSpawnStartDelayTime;
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
            _sendAntCooldown -= time;
            _sendAntMovementPauseCooldown -= time;
        }

        private void HandleMovement()
        {
            var leftInput = GetStickInput("Left", _playerNumber);
            Vector3 movement = new Vector3(leftInput.x, 0, leftInput.y);
            
            if(CanMove())
            {
                //_animator.SetBool("IsCommanding",false);
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
            if (_sendAntMovementPauseCooldown > 0 || _antSpawnButtonDown || InputUtility.IsButtonPressed(_playerNumber, _spawnKey))
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
            if(InputUtility.IsButtonDown(_playerNumber, _sendKey) || Input.GetKeyDown(KeyCode.Space))
            {
                //Debug.LogWarning("A button calling");
                SendWorkerAntForSearch();
                _calloutParticleSystem.gameObject.SetActive(true);
                _calloutParticleSystem.Play();
            }

            else if(InputUtility.IsButtonDown(_playerNumber, _recallKey))
            {
                //Debug.LogWarning("B button calling");
                TeamController.WorkerAntManager.Whistle(_whistleRange, transform.position);
                _whistleParticleSystem.gameObject.SetActive(true);
                _whistleParticleSystem.transform.localScale = Vector3.one * _whistleRange * _whistleScaler;
                _whistleParticleSystem.Play();
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
            // Recently spawned
            if (_antSpawnCooldown > 0.0f)
            {
                _spawnDelayParticleLoopingObject.SetActive(false);
                return;
            }
            
            // Button let go
            bool spawnButtonPressed = InputUtility.IsButtonPressed(_playerNumber, _spawnKey);
            if (!spawnButtonPressed)
            {
                _spawnDelayParticleLoopingObject.SetActive(false);
                _antSpawnStartDelay = _antSpawnStartDelayTime;
                return;
            }
            
            // Can't afford
            if (!CanSpawnAnt())
                return;
            
            // Button pressed
            _antSpawnStartDelay -= Time.deltaTime;
            _spawnDelayParticleLoopingObject.SetActive(true);


            if (_antSpawnStartDelay > 0)
                return;
            
            SpawnAntEgg();
            _antSpawnStartDelay = _antSpawnStartDelayTime;
            _antSpawnCooldown = _antSpawnCooldownTime;
            TeamController.Nectar -= TeamController.AntNectarCost;
        }

        private bool CanSpawnAnt()
        {
            return TeamController.Nectar >= TeamController.AntNectarCost;
        }

        private void SpawnAntEgg()
        {
            Debug.Log($"Laying ant now!");
            
            var newEgg = Instantiate(_eggPrefab, transform.position, Quaternion.identity);
            Vector3 spawnTarget = transform.TransformPoint(Quaternion.Inverse(_playerModel.rotation) * Vector3.one);
            spawnTarget.y = 0;
            newEgg.Initialize(spawnTarget, TeamController);
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
